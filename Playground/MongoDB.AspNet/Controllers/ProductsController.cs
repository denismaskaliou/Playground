using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.AspNet.Models;
using MongoDB.Shared.Entities;
using MongoDB.Shared.Repository;

namespace MongoDB.AspNet.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController(
    IMapper mapper,
    IMongoDbRepository<Product> repository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IList<ProductDto>>> GetProductsAsync()
    {
        var products = await repository.GetAllAsync();
        return Ok(mapper.Map<IList<ProductDto>>(products));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProductAsync(string id)
    {
        var product = await repository.GetByIdAsync(id);
        return Ok(mapper.Map<ProductDto>(product));
    }

    [HttpPost]
    public async Task<ActionResult> CrateProductAsync(ProductDto dto)
    {
        var product = mapper.Map<Product>(dto);
        product = await repository.CreateAsync(product);
        
        return Created( $"api/products/{product.Id}", product);
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateProductAsync(string id, ProductDto dto)
    {
        var product = mapper.Map<Product>(dto);
        await repository.UpdateAsync(id, product);
       
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProductAsync(string id)
    {
        await repository.DeleteAsync(id);
        
        return NoContent();
    }
}