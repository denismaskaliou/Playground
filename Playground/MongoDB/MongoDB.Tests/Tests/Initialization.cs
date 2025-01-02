using MongoDB.Tests.Infrastructure;

namespace MongoDB.Tests.Tests;

public class Initialization(MongoDbTestContext db) : IClassFixture<MongoDbTestContext>
{
    [Fact]
    public async Task InitData_Async()
    {
        await db.InitDataAsync(shouldSuppress: true);
    }
}