namespace Functions.App.Models;

public class AuditLogDto
{
    public string EntityId { get; init; } = default!;
    public string EventName { get; init; } = default!;
    public DateTime CreatedDate { get; init; }
    public string CorrelationId { get; init; } = default!;
}