using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetSubs.ApiService.Domain.Entities;

namespace NetSubs.ApiService.Infrastructure.Persistence.Configuration;

public class SubscriptionConfiguration :  IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .ValueGeneratedOnAdd()
            .IsRequired();
        
        builder
            .HasOne(s => s.User)
            .WithMany(s => s.Subscriptions)
            .HasForeignKey(s => s.UserId);
        
        builder
            .HasOne(s => s.SubscriptionType)
            .WithMany(st => st.Subscriptions)
            .HasForeignKey(s => s.SubscriptionTypeId);
    }
}

public class UserConfiguration :  IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .ValueGeneratedOnAdd()
            .IsRequired();
        builder
            .HasMany(u => u.Subscriptions)
            .WithOne(s => s.User);
    }
}

public class SubscriptionTypeConfiguration :  IEntityTypeConfiguration<SubscriptionType>
{
    public void Configure(EntityTypeBuilder<SubscriptionType> builder)
    {
        builder.HasKey(st => st.Id);
        builder.Property(st => st.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .ValueGeneratedOnAdd()
            .IsRequired();
        
        builder
            .HasMany(st => st.Subscriptions)
            .WithOne(s => s.SubscriptionType);
    }
}