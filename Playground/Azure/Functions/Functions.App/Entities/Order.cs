using CosmosDb.Shared.Entities;
using Functions.App.Models;

namespace Functions.App.Entities;

public class Order: BaseEntity
{
    public string Name { get; init; } = default!;
    public OrderStatus OrderStatus { get; init; }
    public DateTime UpdatedDate { get; init; }
    public DateTime CreatedDate { get; init; } 
}