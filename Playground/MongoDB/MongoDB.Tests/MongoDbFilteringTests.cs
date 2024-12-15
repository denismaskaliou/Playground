using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Shared.Entities;
using MongoDB.Tests.Context;

namespace MongoDB.Tests;

public class MongoDbFilteringTests(MongoDbTestContext testContext) : IClassFixture<MongoDbTestContext>
{
    private IMongoCollection<Product> Products => testContext.Database.GetCollection<Product>("products-tests");

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
    public async Task FilterByNameAsync()
    {
        // Arrange
        await Products.InsertManyAsync([
            new Product { Name = "Test1", Price = 45 },
            new Product { Name = "Test2", Price = 490 },
        ]);

        // Act
        var cursor = await Products.FindAsync(p => p.Price == 45);
        var actual = await cursor.FirstAsync();

        // Assert
        Assert.Equal(45, actual.Price);
        Assert.Equal("Test1", actual.Name);
    }

    static async Task<bool> CollectionExistsAsync(IMongoDatabase database, string collectionName)
    {
        var filter = new BsonDocument("name", collectionName);
        var collections = await database.ListCollectionsAsync(new ListCollectionsOptions { Filter = filter });

        return await collections.AnyAsync();
    }
}