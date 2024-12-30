using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Tests.Context;

namespace MongoDB.Tests.Tests;

public class CollectionsTests(MongoDbTestContext testContext) : IClassFixture<MongoDbTestContext>
{
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
}