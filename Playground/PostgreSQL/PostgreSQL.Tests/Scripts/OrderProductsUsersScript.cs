using PostgreSQL.Tests.Context;

namespace PostgreSQL.Tests.Scripts;

public class OrderProductsAndUsersScript
{
    private const int UserCount = 100;
    private const int OrderCount = 1000;

    private readonly AppDbContext _context;

    public OrderProductsAndUsersScript(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task CreateAndFillWithMockDataAsync(bool suppress)
    {
        if (suppress) return;

        // Recreate database
        await _context.Database.EnsureDeletedAsync();
        await _context.Database.EnsureCreatedAsync();

        var dataGenerator = new DataGenerator();

        // Generate and insert users
        var users = dataGenerator.GenerateUsers(UserCount);
        await _context.Users.AddRangeAsync(users);
        await _context.SaveChangesAsync();
        Console.WriteLine($"Inserted {UserCount} users.");

        // Generate and insert product categories
        var categories = dataGenerator.GenerateCategories();
        await _context.ProductCategories.AddRangeAsync(categories);
        await _context.SaveChangesAsync();
        Console.WriteLine("Inserted product categories.");

        // Generate and insert products
        var products = dataGenerator.GenerateProducts(categories);
        await _context.Products.AddRangeAsync(products);
        await _context.SaveChangesAsync();
        Console.WriteLine("Inserted products.");

        // Generate and insert orders
        var orders = dataGenerator.GenerateOrders(OrderCount, users, products);
        await _context.Orders.AddRangeAsync(orders);
        await _context.SaveChangesAsync();
        Console.WriteLine($"Inserted {OrderCount} orders.");
    }
}

public class DataGenerator
{
    public List<User> GenerateUsers(int count)
    {
        var random = new Random();
        var users = new List<User>();

        for (int i = 1; i <= count; i++)
        {
            users.Add(new User
            {
                Name = $"User_{i}",
                Email = $"user{i}@example.com",
                Address = $"Address {random.Next(1, 100)}",
                CreatedDate = DateTime.UtcNow.AddDays(-random.Next(1, 365))
            });
        }

        return users;
    }

    public List<ProductCategory> GenerateCategories()
    {
        return new List<ProductCategory>
        {
            new ProductCategory { Name = "Electronics" },
            new ProductCategory { Name = "Clothing" },
            new ProductCategory { Name = "Home Goods" },
            new ProductCategory { Name = "Books" },
            new ProductCategory { Name = "Toys" }
        };
    }

    public List<Product> GenerateProducts(List<ProductCategory> categories)
    {
        var random = new Random();
        var products = new List<Product>();

        for (int i = 1; i <= 500; i++)
        {
            var category = categories[random.Next(categories.Count)];

            products.Add(new Product
            {
                Name = $"Product_{i}",
                Price = Math.Round(random.NextDouble() * 100, 2),
                CategoryId = category.Id,
                Category = category
            });
        }

        return products;
    }

    public List<Order> GenerateOrders(int count, List<User> users, List<Product> products)
    {
        var random = new Random();
        var orders = new List<Order>();

        for (int i = 1; i <= count; i++)
        {
            var user = users[random.Next(users.Count)];

            var order = new Order
            {
                UserId = user.Id,
                User = user,
                OrderDate = DateTime.UtcNow.AddDays(-random.Next(1, 30)),
                Products = GenerateOrderProducts(products, random)
            };

            orders.Add(order);
        }

        return orders;
    }

    private List<Product> GenerateOrderProducts(List<Product> products, Random random)
    {
        var orderProducts = new List<Product>();
        for (int i = 0; i < random.Next(1, 5); i++)
        {
            orderProducts.Add(products[random.Next(products.Count)]);
        }

        return orderProducts;
    }
}

public class User
{
    public int Id { get; set; } // Auto-generated primary key
    public string Name { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime OrderDate { get; set; }

    public User User { get; set; }
    public List<Product> Products { get; set; } = new();
}

public class Product
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }

    public ProductCategory Category { get; set; }
    public List<Order> Orders { get; set; } = new();
}

public class ProductCategory
{
    public int Id { get; set; }
    public string Name { get; set; }
}