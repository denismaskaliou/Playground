using CosmosDb.Shared.Entities;

namespace Functions.App.Models;

public class Order: BaseEntity
{
    public string Name { get; init; } = default!;
    public OrderStatus OrderStatus { get; init; }
    public DateTime UpdatedDate { get; init; }
    public DateTime CreatedDate { get; init; } 
}