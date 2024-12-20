using CosmosDb.Shared.Entities;

namespace Functions.App.Entities;

public class AuditLog : BaseEntity
{
    public string EntityId { get; init; } = default!;
    public string EventName { get; init; } = default!;
    public DateTime CreatedDate { get; init; }
    public string CorrelationId { get; init; } = default!;
}