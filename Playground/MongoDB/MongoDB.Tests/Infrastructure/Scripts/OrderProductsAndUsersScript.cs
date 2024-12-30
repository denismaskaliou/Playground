using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace MongoDB.Tests.Scripts;

public class OrderProductsAndUsersScript
{
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<Order> _ordersCollection;
    private readonly IMongoCollection<User> _usersCollection;

    public OrderProductsAndUsersScript(IMongoDatabase database)
    {
        _database = database ?? throw new ArgumentNullException(nameof(database));
        _ordersCollection = _database.GetCollection<Order>("Orders");
        _usersCollection = _database.GetCollection<User>("Users");
        ConfigureMappings(); // Configure property mappings
    }

    public async Task CreateAndFillWithMockDataAsync(bool suppress)
    {
        if (suppress) return;

        // Drop and recreate collections if needed
        await _database.DropCollectionAsync("Orders");
        await _database.DropCollectionAsync("Users");

        // Generate and insert users
        var users = GenerateUsers(10_000); // Generate 100 users
        await _usersCollection.InsertManyAsync(users);
        Console.WriteLine("Inserted 100 users.");

        // Generate and insert orders
        var orders = GenerateOrders(1_000_000, users); // Generate 1,000 orders
        await _ordersCollection.InsertManyAsync(orders);
        Console.WriteLine("Inserted 1,000 orders.");
    }

    private void ConfigureMappings()
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(User)))
        {
            BsonClassMap.RegisterClassMap<User>(map =>
            {
                map.AutoMap();
                map.SetIgnoreExtraElements(true);
                map.MapIdMember(c => c.Id).SetElementName("_id"); // Maps the _id field
                map.MapMember(c => c.UserId).SetElementName("userId");
                map.MapMember(c => c.Name).SetElementName("name");
                map.MapMember(c => c.Email).SetElementName("email");
                map.MapMember(c => c.DateJoined).SetElementName("date_joined");
                map.MapMember(c => c.Address).SetElementName("address");
            });
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(Order)))
        {
            BsonClassMap.RegisterClassMap<Order>(map =>
            {
                map.AutoMap();
                map.SetIgnoreExtraElements(true);
                map.MapIdMember(c => c.Id).SetElementName("_id"); // Maps the _id field
                map.MapMember(c => c.OrderId).SetElementName("orderId");
                map.MapMember(c => c.UserId).SetElementName("user_id");
                map.MapMember(c => c.OrderDate).SetElementName("order_date");
                map.MapMember(c => c.Products).SetElementName("products");
            });
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(Product)))
        {
            BsonClassMap.RegisterClassMap<Product>(map =>
            {
                map.AutoMap();
                map.SetIgnoreExtraElements(true);
                map.MapMember(c => c.ProductId).SetElementName("product_id");
                map.MapMember(c => c.Name).SetElementName("name");
                map.MapMember(c => c.Price).SetElementName("price");
                map.MapMember(c => c.Sizes).SetElementName("sizes");
            });
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(Size)))
        {
            BsonClassMap.RegisterClassMap<Size>(map =>
            {
                map.AutoMap();
                map.SetIgnoreExtraElements(true);
                map.MapMember(c => c.Label).SetElementName("label");
                map.MapMember(c => c.Quantity).SetElementName("quantity");
            });
        }
    }

    private static List<User> GenerateUsers(int count)
    {
        var random = new Random();
        var users = new List<User>();

        for (int i = 1; i <= count; i++)
        {
            users.Add(new User
            {
                Id = ObjectId.GenerateNewId().ToString(),
                UserId = $"user_{i}",
                Name = $"User_{i}",
                Email = $"user{i}@example.com",
                DateJoined = DateTime.UtcNow.AddDays(-random.Next(1, 365)), // Joined within the last year
                Address = $"Address {random.Next(1, 100)}"
            });
        }

        return users;
    }

    private static List<Order> GenerateOrders(int count, List<User> users)
    {
        var random = new Random();
        var orders = new List<Order>();

        for (int i = 1; i <= count; i++)
        {
            // Randomly pick a user from the users list
            var user = users[random.Next(users.Count)];

            var order = new Order
            {
                Id = ObjectId.GenerateNewId().ToString(),
                OrderId = $"order_{i}",
                UserId = user.UserId,
                OrderDate = DateTime.UtcNow.AddDays(-random.Next(1, 30)), // Ordered within the last month
                Products = GenerateProducts(random)
            };

            orders.Add(order);
        }

        return orders;
    }

    private static List<Product> GenerateProducts(Random random)
    {
        var productNames = new[] { "T-Shirt", "Jeans", "Shoes", "Hat", "Jacket", "Sweater" };
        var products = new List<Product>();

        for (int i = 0; i < random.Next(1, 5); i++) // Each order has 1 to 4 products
        {
            var product = new Product
            {
                ProductId = $"product_{Guid.NewGuid()}",
                Name = productNames[random.Next(productNames.Length)],
                Price = Math.Round(random.NextDouble() * 100, 2), // Random price between 0 and 100
                Sizes = GenerateSizes(random)
            };

            products.Add(product);
        }

        return products;
    }

    private static List<Size> GenerateSizes(Random random)
    {
        var sizeLabels = new[] { "Small", "Medium", "Large", "One Size", "30", "32", "34" };
        var sizes = new List<Size>();

        for (int i = 0; i < random.Next(1, 4); i++) // Each product has 1 to 3 sizes
        {
            sizes.Add(new Size
            {
                Label = sizeLabels[random.Next(sizeLabels.Length)],
                Quantity = random.Next(1, 20) // Quantity between 1 and 20
            });
        }

        return sizes;
    }
}

// User class
public class User
{
    public string Id { get; set; } // MongoDB's default _id field
    public string UserId { get; set; } // Application-specific ID
    public string Name { get; set; }
    public string Email { get; set; }
    public DateTime DateJoined { get; set; }
    public string Address { get; set; }
}

// Order class
public class Order
{
    public string Id { get; set; } // MongoDB's default _id field
    public string OrderId { get; set; } // Application-specific ID
    public string UserId { get; set; } // Reference to the User collection
    public DateTime OrderDate { get; set; }
    public List<Product> Products { get; set; } = new();
}

// Product class
public class Product
{
    public string ProductId { get; set; } // Unique identifier for the product
    public string Name { get; set; } // Name of the product
    public double Price { get; set; } // Price of the product
    public List<Size> Sizes { get; set; } = new();
}

// Size class
public class Size
{
    public string Label { get; set; } // Size label (e.g., Small, Medium, Large)
    public int Quantity { get; set; } // Quantity available for the size
}