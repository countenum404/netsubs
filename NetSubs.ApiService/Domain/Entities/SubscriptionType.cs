using NetSubs.ApiService.Domain.Entities;

namespace NetSubs.ApiService.Domain.Entities;

public class SubscriptionType
{
    public Guid Id { get; set; } = Guid.Empty;
    public string TypeName { get; set; } = string.Empty;
    public decimal MonthlyPrice { get; set; } = Decimal.Zero;
    public bool IsActive { get; set; } = false;
    public List<Subscription> Subscriptions { get; set; }
}