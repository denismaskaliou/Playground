namespace MongoDB.Shared.Entities;

public class Product : BaseEntity
{
    public string Name { get; init; } = null!;
    public int Price { get; init; }
}