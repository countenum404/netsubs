using NetSubs.ApiService.Domain.Entities;
using NetSubs.ApiService.Infrastructure.Persistence.Repository;

namespace NetSubs.ApiService.Api.Dto;

public record CreateSubscriptionRequest(Guid UserGuid, Guid SubscriptionGuid, DateTime Start, DateTime End);
public record CreatedSubscriptionResponse(String Message, Subscription subscription);
public record MessageResponse(string Message);
public record UpdateSubscriptionRequest(Guid UserGuid, Guid SubGuid, Subscription Subscription);

public class FailedOperation<T>(string message, T response)
{
    public string Message { get; set; } = message;
    public T Response { get; set; } = response;
}

public class SuccessOperation<T>(string message, T response)
{
    public string Message { get; set; } = message;
    public T Response { get; set; } = response;
}