namespace MongoDB.Shared.Options;

public class MongoDbBaseOptions
{
    public string ConnectionString { get; init; } = default!;
    public string DatabaseName { get; init; } = default!;
}