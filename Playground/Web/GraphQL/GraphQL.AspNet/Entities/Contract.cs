using CosmosDb.Shared.Entities;

namespace GraphQL.AspNet.Entities;

public class Contract : BaseEntity
{
    public string Name { get; init; } = null!;

    public ContractStatus Status { get; init; }

    public DateTime? CreatedAt { get; init; }

    public List<Program> Programs { get; init; } = [];
}