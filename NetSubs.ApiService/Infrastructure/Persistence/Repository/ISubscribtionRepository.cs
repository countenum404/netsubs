using NetSubs.ApiService.Domain.Entities;

namespace NetSubs.ApiService.Infrastructure.Persistence.Repository;

public interface ISubscribtionRepository
{
    Task<List<Subscription>> GetAllSubscriptionsAsync();   
    Task<Subscription> GetSubscriptionByIdAsync(Guid subscriptionId);  
    Task AddSubscriptionAsync(Subscription subscription);      
    Task UpdateSubscriptionByIdAsync(Subscription subscription);     
    Task RemoveSubscriptionAsync(Guid subscriptionId);            
    Task<bool> ExistsAsync(Guid subscriptionId); 
}