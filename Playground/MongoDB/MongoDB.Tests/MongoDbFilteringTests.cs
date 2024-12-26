using System.Diagnostics;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Tests.Context;
using MongoDB.Tests.Scripts;
using Xunit.Abstractions;

namespace MongoDB.Tests;

public class MongoDbFilteringTests(
    MongoDbTestContext testContext,
    ITestOutputHelper output) : IClassFixture<MongoDbTestContext>
{
    private IMongoCollection<Order> Orders => testContext.Database.GetCollection<Order>("Orders");
    private IMongoCollection<User> Users => testContext.Database.GetCollection<User>("Users");

    [Fact]
    public async Task InitData_Async()
    {
        await testContext.InitDataAsync(shouldSuppress: false);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task CheckCollectionExistsAsync(bool collectionExists)
    {
        // Arrange
        var collectionName = "products-test-1";
        var filter = new BsonDocument("name", collectionName);
        var collections = await testContext.Database.ListCollectionsAsync(
            new ListCollectionsOptions { Filter = filter }
        );

        if (collectionExists && !await collections.AnyAsync())
            await testContext.Database.CreateCollectionAsync(collectionName);

        if (!collectionExists && !await collections.AnyAsync())
            await testContext.Database.DropCollectionAsync(collectionName);

        // Assert
        collections = await testContext.Database.ListCollectionsAsync(
            new ListCollectionsOptions { Filter = filter }
        );

        Assert.Equal(collectionExists, await collections.AnyAsync());

        await testContext.Database.DropCollectionAsync(collectionName);
    }

    [Fact]
    public async Task Find_BySubEntityName_ReturnShoes_Async()
    {
        var filter = Builders<Order>.Filter.ElemMatch(
            order => order.Products,
            t => t.Name == "Shoes"
        );

        var projection = Builders<Order>.Projection
            .Include(t => t.Products);


        // var explainResult = Orders.Find(filter).Project(projection).ToString();

        var timer = Stopwatch.StartNew();
        
        _ = await Orders.Find(filter).Project(projection).ToListAsync();

        timer.StopAndPrint(output);
    }

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