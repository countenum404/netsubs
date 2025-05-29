using NetSubs.ApiService.Domain.Entities;
using NetSubs.ApiService.Infrastructure.Persistence.Repository;

namespace NetSubs.ApiService.Domain.Service;

public interface ISubService
{
    Task<List<Subscription>> GetAllActiveSubscriptionsAsync(CancellationToken cancellationToken = default);
    Task<Subscription> CreateSubscriptionAsync(Guid user, Guid type, DateTime start, DateTime end, CancellationToken cancellationToken = default);
    Task DeleteSubscriptionAsync(Guid subscriptionId, CancellationToken cancellationToken = default);
    Task UpdateSubscriptionAsync(Guid userGuid, Guid subGuid, Subscription subscription, CancellationToken cancellationToken = default);
}

public class SubService : ISubService
{
    private readonly ISubscribtionRepository _subscribtionRepository;
    private readonly ILogger<SubService> _logger;
    
    public SubService(ISubscribtionRepository  subscribtionRepository, ILogger<SubService> logger)
    {
        _subscribtionRepository = subscribtionRepository;
        _logger = logger;
    }

    public async Task<List<Subscription>> GetAllActiveSubscriptionsAsync(CancellationToken cancellationToken = default)
    {
        return await _subscribtionRepository.GetAllSubscriptionsAsync();
    }

    public async Task<Subscription> CreateSubscriptionAsync(Guid user, Guid type, DateTime start, DateTime end, CancellationToken cancellationToken = default)
    {
        var s = new Subscription();
        s.Id = Guid.NewGuid();
        s.UserId = user;
        s.SubscriptionTypeId = type;
        s.StartDate = start;
        s.EndDate = end;
        s.IsActive = true;
        _logger.LogInformation("Creating new subscription");
        _logger.LogInformation(s.ToString());
        try
        {
            await _subscribtionRepository.AddSubscriptionAsync(s);
            _logger.LogInformation($"Created subscription: {s.Id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new InvalidOperationException("Failed to create new subscription");
        }
        return await Task.FromResult(s);
    }

    public async Task DeleteSubscriptionAsync(Guid subscriptionId, CancellationToken cancellationToken = default)
    {
        if (await _subscribtionRepository.ExistsAsync(subscriptionId))
        {
            await _subscribtionRepository.RemoveSubscriptionAsync(subscriptionId);
        }
        else
        {
            throw new Exception($"Subscription with id {subscriptionId} not found.");
        }
    }

    public async Task UpdateSubscriptionAsync(Guid userGuid, Guid subGuid, Subscription subscription,
        CancellationToken cancellationToken = default)
    {
        if (userGuid != subscription.UserId)
        {
            throw new ArgumentException("UserId does not match subscription user");
        }
        
        var sub = await _subscribtionRepository.GetSubscriptionByIdAsync(subGuid);
        
        if (sub.UserId != userGuid)
        {
            throw new Exception($"Subscription with id {subGuid} not found for user id {userGuid}.");
        }
        await _subscribtionRepository.UpdateSubscriptionByIdAsync(subscription);
    }
    
}

