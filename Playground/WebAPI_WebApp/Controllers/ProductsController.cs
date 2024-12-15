using System.Diagnostics;
using AutoMapper;
using Database_MongoDB.Models;
using Database_MongoDB.Repositories;
using Database_Redis.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using WebAPI_WebApp.Models;

namespace WebAPI_WebApp.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController(
    IMapper mapper,
    ProductsCachedService productsService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IList<ProductDto>>> GetAllAsync()
    {
        var products = await productsService.GetProductsAsync();
        return Ok(mapper.Map<IList<ProductDto>>(products));
    }

    [HttpGet("{id}")]
    public IActionResult FindAsync(string id)
    {
        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult> CrateProductAsync(ProductDto dto)
    {
        var product = mapper.Map<Product>(dto);
        product = await productsService.CreateProductAsync(product);
        
        return Created( $"api/products/{product.Id}", product);
    }
}