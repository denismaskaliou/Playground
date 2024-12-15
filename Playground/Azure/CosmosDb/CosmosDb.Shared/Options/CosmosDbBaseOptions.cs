namespace CosmosDb.Shared.Options;

public class CosmosDbBaseOptions
{
    public string ConnectionString { get; init; } = default!;
    public string DatabaseName { get; init; } = default!;
}