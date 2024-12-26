using System.Diagnostics;
using MongoDB.Driver;
using MongoDB.Shared.Mappings;
using MongoDB.Tests.Scripts;
using Xunit.Abstractions;

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

    public async Task InitDataAsync(bool shouldSuppress = true)
    {
        await new OrderProductsAndUsersScript(Database).CreateAndFillWithMockDataAsync(shouldSuppress);
    }

    public void Dispose()
    {
        Database.Client?.Dispose();
    }
}

public static class StopwatchExtensions
{
    public static void StopAndPrint(this Stopwatch stopwatch, ITestOutputHelper output)
    {
        output.WriteLine($"Test completed: {stopwatch.ElapsedMilliseconds / 1000.0:F4} sec");
        stopwatch.Reset();
    }
}