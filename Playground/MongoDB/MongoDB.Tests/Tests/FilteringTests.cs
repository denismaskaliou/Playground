using System.Diagnostics;
using System.Linq.Expressions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Tests.Context;
using MongoDB.Tests.Infrastructure.Models;
using MongoDB.Tests.Scripts;
using Xunit.Abstractions;

namespace MongoDB.Tests.Tests;

public class FilteringTests(
    MongoDbTestContext testContext,
    ITestOutputHelper output) : IClassFixture<MongoDbTestContext>
{
    private IMongoCollection<Order> Orders => testContext.Database.GetCollection<Order>("Orders");

    [Fact]
    public async Task InitData_Async()
    {
        await testContext.InitDataAsync(shouldSuppress: true);
    }

    [Fact]
    public async Task Find_ProductsWithFilter_Async()
    {
        var query = Orders
            .Find(Builders<Order>.Filter.ElemMatch(o => o.Products, t => t.Name == "Shoes"))
            .Project(t => t.Products.Where(p => p.Name == "Shoes"));

        var orderProducts = await query.ToListAsync();
        var products = orderProducts.SelectMany(p => p).ToList();
    }

    [Fact]
    public async Task Find_ProductsWithFilterByExpression_Async()
    {
        Expression<Func<Product, bool>> filter = t => t.Name == "Shoes";

        var query = Orders
            .Find(Builders<Order>.Filter.ElemMatch(o => o.Products, filter))
            .Project(o => o.Products.Where(t => t.Name == "Shoes"));

        var orderProducts = await query.ToListAsync();
        var products = orderProducts.SelectMany(p => p).ToList();
    }

    [Fact]
    public async Task Find_ProductsWithAggregations_Async()
    {
        var ordersFilter = Builders<Order>.Filter.ElemMatch(o => o.Products, t => t.Name == "Shoes");
        var unwindProductsFilter = Builders<UnwindProducts>.Filter.And(
            Builders<UnwindProducts>.Filter.Where(t => t.Products.Name == "Shoes"),
            Builders<UnwindProducts>.Filter.Exists(t => t.Products.Name)
        );

        var query = Orders.Aggregate()
            .Match(ordersFilter)
            .Unwind<Order, UnwindProducts>(o => o.Products)
            .Match(unwindProductsFilter)
            .Project(t => t.Products);

        var products = await query.ToListAsync();
    }

    [Fact]
    public async Task Find_ProductsWithExpressionAggregations_Async()
    {
        Expression<Func<Product, bool>> orderFilter = p =>
            p.Name == "Shoes" &
            p.Sizes.Any(s => s.Label == "Small");

        var query = Orders.Aggregate()
            .Match(Builders<Order>.Filter.ElemMatch(o => o.Products, orderFilter))
            .Unwind<Order, UnwindProducts>(o => o.Products)
            .Match(orderFilter.ToUnwindProductsFilter())
            .Project(t => t.Products);

        var products = await query.ToListAsync();
    }

    [Fact]
    public async Task Find_ProductsWithQueryable_Async()
    {
        var query = Orders.AsQueryable()
            .SelectMany(t => t.Products.Where(p => p.Name == "Shoes"));

        var products = await query.ToListAsync();
    }

    [Fact]
    public async Task Find_ProductsWithReplaceRoot_Async()
    {
        Expression<Func<Product, bool>> orderFilter = p =>
            p.Name == "Shoes" &
            p.Sizes.Any(s => s.Label == "Small");

        var query = Orders.Aggregate()
            .Match(Builders<Order>.Filter.ElemMatch(o => o.Products, orderFilter))
            .Unwind<Order, UnwindProducts>(o => o.Products)
            .ReplaceRoot(t => t.Products)
            .Match(orderFilter);

        var products = await query.ToListAsync();
    }
}