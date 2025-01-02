using System.Linq.Expressions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Tests.Infrastructure;
using MongoDB.Tests.Infrastructure.Models;
using MongoDB.Tests.Scripts;

namespace MongoDB.Tests.Tests;

public class FilteringTests(MongoDbTestContext db) : IClassFixture<MongoDbTestContext>
{
    [Fact]
    public async Task Find_ProductsByFilter()
    {
        var query = db.Orders
            .Find(Builders<Order>.Filter.ElemMatch(o => o.Products, t => t.Name == "Shoes"))
            .Project(t => t.Products.Where(p => p.Name == "Shoes"));

        var orderProducts = await query.ToListAsync();
        var products = orderProducts.SelectMany(p => p).ToList();
    }

    [Fact]
    public async Task Find_ProductsWithFilterByExpression_Async()
    {
        Expression<Func<Product, bool>> filter = t => t.Name == "Shoes";

        var query = db.Orders
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

        var query = db.Orders.Aggregate()
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

        var query = db.Orders.Aggregate()
            .Match(Builders<Order>.Filter.ElemMatch(o => o.Products, orderFilter))
            .Unwind<Order, UnwindProducts>(o => o.Products)
            .Match(orderFilter.ToUnwindProductsFilter())
            .Project(t => t.Products);

        var products = await query.ToListAsync();
    }

    [Fact]
    public async Task Find_ProductsWithQueryable_Async()
    {
        var query = db.Orders.AsQueryable()
            .SelectMany(t => t.Products.Where(p => p.Name == "Shoes"));

        var products = await query.ToListAsync();
    }

    [Fact]
    public async Task Find_ProductsWithReplaceRoot_Async()
    {
        Expression<Func<Product, bool>> orderFilter = p =>
            p.Name == "Shoes" &
            p.Sizes.Any(s => s.Label == "Small");

        var query = db.Orders.Aggregate()
            .Match(Builders<Order>.Filter.ElemMatch(o => o.Products, orderFilter))
            .Unwind<Order, UnwindProducts>(o => o.Products)
            .ReplaceRoot(t => t.Products)
            .Match(orderFilter);

        var products = await query.ToListAsync();
    }

    [Fact]
    public async Task Find_ProductsForeach_Async()
    {
        var productslist = new List<Product>();

        await db.Orders
            .Find(Builders<Order>.Filter.ElemMatch(o => o.Products, t => t.Name == "Shoes"))
            .ForEachAsync(order => { productslist.AddRange(order.Products.Where(p => p.Name == "Shoes")); });
    }
}