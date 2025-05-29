using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NetSubs.ApiService.Api.Dto;
using NetSubs.ApiService.Domain.Entities;
using NetSubs.ApiService.Domain.Service;
using NetSubs.ApiService.Infrastructure.Persistence.Repository;
using NetSubs.ApiService.Infrastructure.Persistence.Context;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "NetSubs API",
        Version = "v1",
        Description = "API for managing subs"
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

void RegisterDependencies(WebApplicationBuilder appBuilder)
{
    appBuilder.Services.AddLogging(builder =>
    {
        builder.AddConsole();
        builder.AddDebug();
        builder.AddEventSourceLogger();
    });
    appBuilder.Services.AddDbContext<SubscriptionsDbContext>(options =>
        {
            var host = Environment.GetEnvironmentVariable("DB_HOST");
            var user = Environment.GetEnvironmentVariable("DB_USER");
            var password = Environment.GetEnvironmentVariable("DB_PASSWORD");

            var connectionString = $"User ID={user};Password={password};Host={host};Port=5432;Database=subs;";
            options.UseNpgsql(connectionString);
        }
    );
    appBuilder.Services.AddScoped<ISubscribtionRepository, SubRepository>();
    appBuilder.Services.AddScoped<ISubService, SubService>();
}

RegisterDependencies(builder);

var app = builder.Build();


var db = app.Services.GetRequiredService<SubscriptionsDbContext>().Database;

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Connection string is:" + app.Services.GetRequiredService<SubscriptionsDbContext>().Database.GetConnectionString());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "NetSubs API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.MapGet("/api/subs", async (ISubService subService) => 
        Results.Ok(await subService.GetAllActiveSubscriptionsAsync(CancellationToken.None))
    )
    .WithName("SomeSubService");

app.MapPost("/api/subs", async ([FromBody] CreateSubscriptionRequest req, [FromServices] ISubService subService, ILogger<Program> logger) =>
    {
        try
        {
             var sub = await subService.CreateSubscriptionAsync(
                user: req.UserGuid,
                type: req.SubscriptionGuid,
                start: req.Start,
                end: req.End,
                cancellationToken: CancellationToken.None
            );
            return Results.Ok(new SuccessOperation<Subscription>("subscription created", sub));
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return Results.BadRequest(new FailedOperation<CreateSubscriptionRequest>($"subscription not created: {ex.Message}", req));
        }
    })
    .WithName("AddSubService");

app.MapDelete("/api/subs/{guid:guid}", async (Guid guid, [FromServices] ISubService subService) =>
    {
        try
        {
            await subService.DeleteSubscriptionAsync(guid);
        }
        catch (Exception ex)
        {
            return Results.NotFound(new FailedOperation<Guid>($"Failed to delete subscription: {ex.Message}", guid));
        }
        return Results.Ok(new SuccessOperation<Guid>($"subscription has been deleted", guid));
    })
    .WithName("DeleteSubService");

app.MapPut("/api/subs", async ([FromBody] UpdateSubscriptionRequest update, [FromServices] ISubService subService) =>
{
    try
    {
        await subService.UpdateSubscriptionAsync(update.UserGuid, update.SubGuid, update.Subscription); 
    } catch (Exception ex)
    {
        return Results.NotFound(new FailedOperation<UpdateSubscriptionRequest>($"Failed to update subscription: {ex.Message}", update));
    }
    return Results.Ok(new SuccessOperation<UpdateSubscriptionRequest>($"subscription updated", update));
}).WithName("UpdateSubService");

app.MapDefaultEndpoints();
app.UseCors("AllowAll");
app.Run();