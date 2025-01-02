using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Tests.Infrastructure;
using MongoDB.Tests.Scripts;

namespace MongoDB.Tests.Tests;

public class DeleteTests(MongoDbTestContext testContext) : IClassFixture<MongoDbTestContext>
{
    private IMongoCollection<Order> Orders => testContext.Database.GetCollection<Order>("Orders");

    [Fact]
    public async Task Remove_ProductsFeomOrder_Async()
    {
        var productIds = new[]
        {
            "product_51194641-8843-44d5-89c0-3a8472f91876",
            "product_a018f350-ead9-465a-aa21-283b705491b6"
        };

        var filter = Builders<Order>.Filter
            .ElemMatch(o => o.Products, p => productIds.Contains(p.ProductId));

        var updateDefinition = Builders<Order>.Update
            .PullFilter(t => t.Products, p => productIds.Contains(p.ProductId));

        var updateResult = await Orders.UpdateManyAsync(filter, updateDefinition);
    }
}