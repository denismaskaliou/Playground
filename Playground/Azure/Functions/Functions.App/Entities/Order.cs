using CosmosDb.Shared.Entities;
using Functions.App.Models;

namespace Functions.App.Entities;

public class Order : BaseEntity
{
    public string Name { get; init; } = default!;
    public OrderStatus OrderStatus { get; init; }
    public DateTime UpdatedAt { get; init; }
    public string UpdatedBy { get; init; } = default!;
    public DateTime CreatedDate { get; init; }
}