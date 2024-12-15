using MongoDB.Driver;
using MongoDB.Shared.Mappings;

namespace MongoDB.Tests.Context;

public class MongoDbTestContext: IDisposable
{
    private const string ConnectionString = "mongodb://admin:1234@localhost:27017/";
    private const string DatabaseName = "playground";
    
    public IMongoDatabase Database { get; set; }
    
    public MongoDbTestContext()
    {
        ProductsMapping.Map();
        var client = new MongoClient(ConnectionString);
        Database = client.GetDatabase(DatabaseName);
    }

    public void Dispose()
    {
        Database.Client?.Dispose();
    }
}