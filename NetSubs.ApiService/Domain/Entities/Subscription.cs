using System.Text;
using System.Text.Json.Serialization;
using NetSubs.ApiService.Api.Converters;

namespace NetSubs.ApiService.Domain.Entities;

public class Subscription
{
    public Guid Id { get; set; } = Guid.Empty;
    public Guid UserId { get; set; } = Guid.Empty;
    
    [JsonIgnore]
    public virtual User? User { get; set; }
    public Guid SubscriptionTypeId { get; set; } = Guid.Empty;
    
    [JsonIgnore]
    public virtual SubscriptionType? SubscriptionType { get; set; }
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime StartDate { get; set; } = DateTime.Today;
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime EndDate { get; set; } = DateTime.Today;
    public bool IsActive { get; set; } = false;

    public override string ToString()
    {

        return new StringBuilder().AppendLine(this.GetType().Name).Append("[")
                .Append(this.Id)
                .Append(this.UserId)
                .Append(this.IsActive)
                .Append(this.StartDate)
                .Append(this.StartDate.ToString("dd.MM.yyyy"))
                .Append(this.EndDate.ToString("dd.MM.yyyy"))
                .Append("]")
            .ToString();
    }
}