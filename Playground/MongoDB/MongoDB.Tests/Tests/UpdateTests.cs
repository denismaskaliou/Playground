using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Tests.Context;
using MongoDB.Tests.Scripts;

namespace MongoDB.Tests.Tests;

public class UpdateTests(MongoDbTestContext testContext) : IClassFixture<MongoDbTestContext>
{
    private IMongoCollection<Order> Orders => testContext.Database.GetCollection<Order>("Orders");

    [Fact]
    public async Task Update_Order_Async()
    {
        var filter = Builders<Order>.Filter
            .Where(o => o.Id == "676c20e9d1266ad51216965e");

        var updateDefinition = Builders<Order>.Update
            .Set(t => t.OrderDate, DateTime.Now);

        var updateResult = await Orders.UpdateOneAsync(filter, updateDefinition);
    }

    [Fact]
    public async Task Update_OrderProducts_Async()
    {
        // Arrange
        var orderId = "order_6";
        var products = await GetOrderProductsAsync(orderId);

        foreach (var product in products)
            product.Name = $"Name_{product.ProductId}";

        // Act
        var updateOptions = new UpdateOptions
        {
            ArrayFilters = GetProductIdsFilters(products.Select(t => t.ProductId))
        };

        var updateDefinitions = new List<UpdateDefinition<Order>>();

        foreach (var product in products)
            updateDefinitions.Add(
                Builders<Order>.Update.Set(
                    t => t.Products.AllMatchingElements(MapToAlphabeticString(product.ProductId.GetHashCode())),
                    product));

        // Assert
        var updateResult = await Orders.UpdateOneAsync(
            o => o.OrderId == orderId,
            Builders<Order>.Update.Combine(updateDefinitions),
            updateOptions
        );
    }

    [Fact]
    public async Task Update_OrdersProducts_Async()
    {
        // Arrange
        var orderIds = new[] { "order_7", "order_8" };
        var products = await GetOrdersProductsAsync(orderIds);

        var productsToUpdate = products
            .Where(p => p.Name == "Shoes")
            .ToList();

        foreach (var product in productsToUpdate)
            product.Price = 1004;

        // Act
        var arrayFilters = new List<ArrayFilterDefinition>();

        var updateDefinitions = productsToUpdate.Select(product =>
        {
            var identifier = MapToAlphabeticString(product.ProductId.GetHashCode());
            arrayFilters.Add(GetProductIdFilter(identifier, product.ProductId));

            return Builders<Order>.Update.Set(t => t.Products.AllMatchingElements(identifier), product);
        });

        // Assert
        var updateResult = await Orders.UpdateManyAsync(
            Builders<Order>.Filter.In(o => o.OrderId, orderIds),
            Builders<Order>.Update.Combine(updateDefinitions),
            new() { ArrayFilters = arrayFilters }
        );
    }

    private string MapToAlphabeticString(long number)
    {
        var stringBuilder = new StringBuilder();
        var current = Math.Abs(number);

        while (current > 0)
        {
            var letter = (char)('a' + current % 10);
            stringBuilder.Append(letter);
            current /= 10;
        }

        return stringBuilder.ToString();
    }

    private ArrayFilterDefinition GetProductIdFilter(string identifier, string id)
    {
        return new BsonDocumentArrayFilterDefinition<Product>(new()
        {
            { $"{identifier}.product_id", id }
        });
    }

    private IEnumerable<ArrayFilterDefinition> GetProductIdsFilters(IEnumerable<string> ids)
    {
        return ids
            .Select(id =>
            {
                var identifier = MapToAlphabeticString(id.GetHashCode());
                var filter = new BsonDocumentArrayFilterDefinition<Product>(new()
                {
                    { $"{identifier}.product_id", id }
                });

                return filter;
            })
            .ToList();
    }

    private async Task<List<Product>> GetOrderProductsAsync(string orderId)
    {
        var orderFilter = Builders<Order>.Filter
            .Where(o => o.OrderId == orderId);

        var orderProducts = await Orders
            .Find(orderFilter)
            .Project(t => t.Products)
            .ToListAsync();

        return orderProducts
            .SelectMany(p => p)
            .ToList();
    }

    private async Task<List<Product>> GetOrdersProductsAsync(IEnumerable<string> ordersIds)
    {
        var orderFilter = Builders<Order>.Filter
            .In(o => o.OrderId, ordersIds);

        var orderProducts = await Orders
            .Find(orderFilter)
            .Project(t => t.Products)
            .ToListAsync();

        return orderProducts
            .SelectMany(p => p)
            .ToList();
    }
}