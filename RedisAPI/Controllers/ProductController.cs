using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RedisAPI.Entities;
using RedisAPI.Infra;
using RedisAPI.Infra.Caching;
using RedisAPI.ViewModels;
using JsonConverter = System.Text.Json.Serialization.JsonConverter;

namespace RedisAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly ProductListDbContext _context;
    private readonly ICachingService _cache;

    public ProductController(ICachingService cache,ProductListDbContext context)
    {
        _context = context;
        _cache = cache;
    }

    [HttpPost]
    public async Task<IActionResult> Post(ProductViewModel model)
    {
        var product = new Product(Guid.NewGuid(), model.Name, model.Description);

        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var productCache = await _cache.GetAsync(id.ToString());

        Product? product;

        if (!string.IsNullOrWhiteSpace(productCache))
        {
            product = JsonConvert.DeserializeObject<Product>(productCache);
            return Ok(product);
        }

        product = _context.Products.FirstOrDefault(x => x.Id == id);
        
        if (product == null)
            return NotFound();

        await _cache.SetAsync(id.ToString(), JsonConvert.SerializeObject(product));
        return Ok(product);
    }
    

}