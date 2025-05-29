using Microsoft.EntityFrameworkCore;
using NetSubs.ApiService.Domain.Entities;
using NetSubs.ApiService.Infrastructure.Persistence.Context;

namespace NetSubs.ApiService.Infrastructure.Persistence.Repository;

public class SubRepository : ISubscribtionRepository
{
    private readonly SubscriptionsDbContext _dbContext;
    
    public SubRepository(SubscriptionsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Subscription>> GetAllSubscriptionsAsync()
    {
        return await _dbContext.Subscriptions.AsNoTracking().ToListAsync();
    }

    public async Task<Subscription> GetSubscriptionByIdAsync(Guid subscriptionId)
    {
        return await _dbContext.Subscriptions.AsNoTracking().FirstOrDefaultAsync(s => s.Id == subscriptionId);
    }

    public async Task AddSubscriptionAsync(Subscription subscription)
    {
        using var transaction = _dbContext.Database.BeginTransactionAsync();
        
        try
        {
            var existingType = await _dbContext.SubscriptionTypes.FindAsync(subscription.SubscriptionTypeId);
            if (existingType == null)
            {
                throw new KeyNotFoundException($"Subscription type GUID '{subscription.SubscriptionTypeId}' not found.");
            }
            
            var existingUser = await _dbContext.Users.FindAsync(subscription.UserId);
            if (existingUser == null)
            {
                throw new KeyNotFoundException($"User '{subscription.UserId}' not found");
            }
            
            await _dbContext.Subscriptions.AddAsync(subscription);
            await _dbContext.SaveChangesAsync();
            await transaction.Result.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.Result.RollbackAsync();
            throw new InvalidOperationException($"Error in saving subscription to db: {ex.Message}");
        }
    }

    public async Task UpdateSubscriptionByIdAsync(Subscription subscription)
    {
        await _dbContext.Subscriptions
            .Where(s => s.Id == subscription.Id)
            .ExecuteUpdateAsync(s => 
                s.SetProperty(sub => sub.Id, subscription.Id)
                    .SetProperty(sub => sub.UserId, subscription.UserId)
                    .SetProperty(sub => sub.SubscriptionTypeId, subscription.SubscriptionTypeId)
                    .SetProperty(sub => sub.StartDate, subscription.StartDate)
                    .SetProperty(sub => sub.EndDate, subscription.EndDate)
                    .SetProperty(sub => sub.IsActive, subscription.IsActive
                    ));
    }

    public async Task RemoveSubscriptionAsync(Guid subscriptionId)
    {
        await _dbContext.Subscriptions
            .Where(sub => sub.Id == subscriptionId)
            .ExecuteDeleteAsync();
    }

    public async Task<bool> ExistsAsync(Guid subscriptionId)
    {
        return await _dbContext.Subscriptions.AnyAsync(s => s.Id == subscriptionId);
    }
}