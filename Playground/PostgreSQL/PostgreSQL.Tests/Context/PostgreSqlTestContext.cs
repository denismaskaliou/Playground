using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PostgreSQL.Tests.Scripts;

namespace PostgreSQL.Tests.Context;

public class PostgreSqlTestContext
{
    public AppDbContext DbDbContext { get; } = new();

    public async Task GenerateMockDataAsync(bool suppress)
    {
        await new OrderProductsAndUsersScript(DbDbContext).CreateAndFillWithMockDataAsync(suppress);
    }
}

public class AppDbContext : DbContext
{
    public const string ConnectionString = "Host=localhost;Database=Playground;Username=user;Password=1234";

    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(ConnectionString);
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("Users");

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Orders");
            entity.HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId);
            entity.HasMany(o => o.Products)
                .WithMany(p => p.Orders)
                .UsingEntity(j => j.ToTable("OrderProducts"));
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");
            entity.HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryId);
        });

        modelBuilder.Entity<ProductCategory>().ToTable("ProductCategories");
    }
}

public class DesignTimeAppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        return new AppDbContext();
    }
}