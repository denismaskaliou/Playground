using MongoDB.Driver;
using MongoDB.Tests.Context;
using MongoDB.Tests.Scripts;
using Xunit.Abstractions;

namespace MongoDB.Tests.Tests;

public class IndexesTests(
    MongoDbTestContext testContext,
    ITestOutputHelper output) : IClassFixture<MongoDbTestContext>
{
    private IMongoCollection<Order> Orders => testContext.Database.GetCollection<Order>("Orders");

    [Fact]
    public async Task CreateIndex_CreateIndexByProductName_Async()
    {
        var indexModel = new CreateIndexModel<Order>(
            Builders<Order>.IndexKeys.Ascending("Products.name"),
            new CreateIndexOptions { Name = "pl_index_products_name" }
        );

        await Orders.Indexes.CreateOneAsync(indexModel);
    }

    [Fact]
    public async Task DropIndex_DropIndexByProductName_Async()
    {
        await Orders.Indexes.DropOneAsync("pl_index_products_name");
    }
}