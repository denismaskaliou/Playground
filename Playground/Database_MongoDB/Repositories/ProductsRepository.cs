using AutoMapper;
using Database_MongoDB.Models;
using Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Database_MongoDB.Repositories;

public class ProductsRepository
{
    private const string ConnectionString = "mongodb+srv://dmaskaliou:Berlin2025@test-cluster.fbvnx.mongodb.net/";
    private const string DatabaseName = "test_db";
    private const string CollectionName = "Products";
    
    private readonly IMapper _mapper;
    private readonly IMongoCollection<ProductEntity> _products;

    public ProductsRepository(IMapper mapper)
    {
        _mapper = mapper;
        
        var client = new MongoClient(ConnectionString);
        var database = client.GetDatabase(DatabaseName);
        _products = database.GetCollection<ProductEntity>(CollectionName);
    }
    
    public async Task<IList<Product>> GetProductsAsync()
    {
        var entities =  await _products.Find(new BsonDocument()).ToListAsync();
        return _mapper.Map<IList<Product>>(entities);
    }
    
    public async Task<Product> CreateProductAsync(Product product)
    {
        var entity = _mapper.Map<ProductEntity>(product);
        await _products.InsertOneAsync(entity);
        return _mapper.Map<Product>(entity);
    }
}