using Microsoft.EntityFrameworkCore;
using PostgreSQL.Tests.Context;

namespace PostgreSQL.Tests.Tests;

public class CreateTests(PostgreSqlTestContext testContext) : IClassFixture<PostgreSqlTestContext>
{
    [Fact]
    public async Task FillWithMockDataAsync()
    {
        await testContext.GenerateMockDataAsync(false);
    }

    [Fact]
    public async Task Get_ProductsAsync()
    {
        var db = testContext.DbDbContext;
        var products = await db.Products.ToListAsync();
    }
}