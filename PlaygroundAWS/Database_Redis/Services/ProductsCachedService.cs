using System.Text.Json;
using AutoMapper;
using Database_MongoDB.Repositories;
using Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;

namespace Database_Redis.Services;

public class ProductsCachedService
{
    private const string ProductCacheKey = "products";
    
    private readonly IMapper _mapper;
    private readonly IDistributedCache _distributedCache;
    private readonly ProductsRepository _productRepository;

    public ProductsCachedService(
        IMapper mapper,
        IDistributedCache distributedCache,
        ProductsRepository productRepository)
    {
        _mapper = mapper;
        _distributedCache = distributedCache;
        _productRepository = productRepository;
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        product = await _productRepository.CreateProductAsync(product);
        await  _distributedCache.RefreshAsync(ProductCacheKey);
        
        return product;
    }

    public async Task<IList<Product>> GetProductsAsync()
    {
        var cachedResult = await _distributedCache.GetStringAsync(ProductCacheKey);
        
        if (!string.IsNullOrEmpty(cachedResult))
            return JsonSerializer.Deserialize<IList<Product>>(cachedResult)!;
        
        var products = await _productRepository.GetProductsAsync();
        await _distributedCache.SetStringAsync(
            ProductCacheKey, 
            JsonSerializer.Serialize(products), 
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1),
            });
        
        return products;
    }
}