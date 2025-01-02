using MongoDB.Driver;
using MongoDB.Tests.Infrastructure;
using MongoDB.Tests.Scripts;

namespace MongoDB.Tests.Tests;

public class InsertsTests(MongoDbTestContext db) : IClassFixture<MongoDbTestContext>
{
    [Fact]
    public async Task Insert_OneProduct_Into_OneOrder()
    {
        // Arrange
        var orderId = "order_11";

        var product = new Product
        {
            ProductId = Guid.NewGuid().ToString(),
            Name = "New product 2",
            Price = 51
        };

        // Act
        var updateResult = await db.Orders.UpdateOneAsync(
            o => o.OrderId == orderId,
            Builders<Order>.Update.Push(p => p.Products, product)
        );
    }

    [Fact]
    public async Task Insert_ManyProducts_Into_OneOrder()
    {
        // Arrange
        var orderId = "order_11";
        var products = new List<Product>
        {
            new()
            {
                ProductId = Guid.NewGuid().ToString(),
                Name = $"New product {Guid.NewGuid()}",
                Price = 54
            },
            new()
            {
                ProductId = Guid.NewGuid().ToString(),
                Name = $"New product {Guid.NewGuid()}",
                Price = 55
            },
        };

        // Act
        var updateResult = await db.Orders.UpdateOneAsync(
            o => o.OrderId == orderId,
            Builders<Order>.Update.PushEach(p => p.Products, products)
        );
    }

    [Fact]
    public async Task Insert_ManyProducts_Into_MayOrder()
    {
        // Arrange
        var orderIds = new[] { "order_11", "order_12" };
        
        var products = new List<Product>
        {
            new()
            {
                ProductId = Guid.NewGuid().ToString(),
                Name = $"New product {Guid.NewGuid()}",
                Price = 54
            },
            new()
            {
                ProductId = Guid.NewGuid().ToString(),
                Name = $"New product {Guid.NewGuid()}",
                Price = 55
            },
        };
        
        var bulkOperations = new List<WriteModel<Order>>
        {
            new UpdateOneModel<Order>(
                Builders<Order>.Filter.Eq(o => o.OrderId, orderIds[0]),
                Builders<Order>.Update.Push(o => o.Products, products[0])
            ),
            new UpdateOneModel<Order>(
                Builders<Order>.Filter.Eq(o => o.OrderId, orderIds[1]),
                Builders<Order>.Update.Push(o => o.Products, products[1])
            )
        };

        // Act
        var updateResult = await db.Orders.BulkWriteAsync(bulkOperations);
    }
}