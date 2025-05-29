namespace NetSubs.ApiService.Domain.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public List<Subscription> Subscriptions { get; set; } = [];
}