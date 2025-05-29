using Microsoft.EntityFrameworkCore;
using NetSubs.ApiService.Domain.Entities;
using NetSubs.ApiService.Infrastructure.Persistence.Configuration;

namespace NetSubs.ApiService.Infrastructure.Persistence.Context;

public class SubscriptionsDbContext : DbContext
{
    public SubscriptionsDbContext(DbContextOptions<SubscriptionsDbContext> options) : base(options) {}

    public DbSet<User> Users { get; set; }
    public DbSet<SubscriptionType> SubscriptionTypes { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new SubscriptionTypeConfiguration());
        modelBuilder.ApplyConfiguration(new SubscriptionConfiguration());
    }
}