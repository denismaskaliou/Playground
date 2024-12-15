using System.Text.Json;
using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;

namespace Database_Redis.Services;

public class ProductsCachedService
{
    private const string ProductCacheKey = "products";
    
    private readonly IMapper _mapper;
    private readonly IDistributedCache _distributedCache;

    public ProductsCachedService(
        IMapper mapper,
        IDistributedCache distributedCache)
    {
        _mapper = mapper;
        _distributedCache = distributedCache;
    }

    // public async Task<Product> CreateProductAsync(Product product)
    // {
    //     // product = await _productRepository.CreateProductAsync(product);
    //     await  _distributedCache.RefreshAsync(ProductCacheKey);
    //     
    //     return product;
    // }

    // public async Task<IList<Product>> GetProductsAsync()
    // {
    //     var cachedResult = await _distributedCache.GetStringAsync(ProductCacheKey);
    //     
    //     if (!string.IsNullOrEmpty(cachedResult))
    //         return JsonSerializer.Deserialize<IList<Product>>(cachedResult)!;
    //     
    //     var products = new List<Product>();
    //     await _distributedCache.SetStringAsync(
    //         ProductCacheKey, 
    //         JsonSerializer.Serialize(products), 
    //         new DistributedCacheEntryOptions
    //         {
    //             AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1),
    //         });
    //     
    //     return products;
    // }
}