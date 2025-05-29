using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetSubs.ApiService.Infrastructure.Persistence.Context;

namespace NetSubs.ApiService.Infrastructure.Persistence.Migrations;

public class MigrateDb
{
    public static void Main(string[] args)
    {
        var connectionString = $"User ID={Environment.GetEnvironmentVariable("DB_USER")};Password={Environment.GetEnvironmentVariable("DB_PASS")};Host=subs-db;Port=5432;Database=subs;";
        
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentException("DATABASE_CONNECTION environment variable is required.");
        }

        try
        {
            var dbContextFactory = new DbContextOptionsBuilder<SubscriptionsDbContext>()
                .UseNpgsql(connectionString)
                .Options;
            
            using var dbContext = new SubscriptionsDbContext(dbContextFactory);
            dbContext.Database.Migrate();
            Console.WriteLine("Migrations applied successfully!");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred during migration: {ex.Message}");
            Environment.Exit(-1);
        }
    }
}