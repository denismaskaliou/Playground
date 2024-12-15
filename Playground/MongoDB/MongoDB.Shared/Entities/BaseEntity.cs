namespace MongoDB.Shared.Entities;

public abstract class BaseEntity
{
    public string Id { get; set; } = default!;
}