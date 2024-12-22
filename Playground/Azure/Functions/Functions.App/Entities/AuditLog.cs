using CosmosDb.Shared.Entities;

namespace Functions.App.Entities;

public class AuditLog : BaseEntity
{
    public string EntityId { get; init; } = default!;
    public string EntityName { get; init; } = default!;
    public AuditType EventName { get; init; }
    public List<AuditChange> Changes { get; init; } = new();
    public DateTime ModifiedAt { get; init; }
    public string ModifiedBy { get; init; } = default!;
}

public class AuditChange
{
    public string Property { get; init; } = default!;
    public string ValueFrom { get; init; } = default!;
    public string ValueTo { get; init; } = default!;
}

public enum AuditType
{
    Created,
    Updated,
    Deleted
}