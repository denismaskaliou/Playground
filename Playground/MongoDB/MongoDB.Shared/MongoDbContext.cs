using MongoDB.Driver;
using MongoDB.Shared.Options;

namespace MongoDB.Shared;

public class MongoDbContext
{
    public IMongoDatabase Database { get; }

    public MongoDbContext(MongoDbBaseOptions options)
    {
        var client = new MongoClient(options.ConnectionString);
        Database = client.GetDatabase(options.DatabaseName);
    }
}