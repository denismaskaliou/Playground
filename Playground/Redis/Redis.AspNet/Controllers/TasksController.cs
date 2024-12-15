using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Redis.AspNet.Controllers;

[ApiController]
[Route("api/tasks")]
public class TasksController(
    IDistributedCache distributedCache) : ControllerBase
{
    [HttpPost("send-message")]
    public async Task<ActionResult> SendMessageAsync()
    {
        return Accepted();
    }
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