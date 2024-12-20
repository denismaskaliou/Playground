using CosmosDb.Shared.Options;

namespace Functions.App.Options;

public class CosmosDbOptions : CosmosDbBaseOptions
{
    public const string SectionName = "CosmosDb";
    
    public string OrdersContainerName { get; init; } = default!;
}