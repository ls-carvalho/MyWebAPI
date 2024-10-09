using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebAPI.Context;
using MyWebAPI.DataTransferObject;
using MyWebAPI.Models;
using MyWebAPI.Services.Interfaces;

namespace MyWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly ILogger<ProductController> _logger;
    private readonly AppDbContext _context;
    private readonly IProductService _productService;

    public ProductController(ILogger<ProductController> logger, AppDbContext context, IProductService productService)
    {
        _logger = logger;
        _context = context;
        _productService = productService;
    }

    [HttpGet]
    [Route("all")]
    public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
    {
        var result = await _productService.GetAllProductsAsync();
        return Ok(result);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<Product>> GetProductById(int id)
    {
        var result = await _context.Products.FindAsync(id);
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    [Route("create")]
    public async Task<ActionResult<Product>> CreateProduct(CreateProductDto product)
    {
        var entity = new Product()
        {
            Name = product.Name,
            Description = product.Description,
            Value = product.Value,
        };
        _context.Products.Add(entity);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Created a product with Id: {Id}", entity.Id);
        return Ok(entity);
    }

    [HttpPost]
    [Route("update")]
    public async Task<ActionResult<Product>> UpdateProduct(UpdateProductDto product)
    {
        if(product is null)
        {
            return BadRequest();
        }

        var entity = await _context.Products.FindAsync(product.Id);

        if (entity == null) 
        {
            return NotFound();
        }

        entity.Name = product.Name;
        entity.Description = product.Description;
        entity.Value = product.Value;

        await _context.SaveChangesAsync();
        _logger.LogInformation("Updated a product with Id: {Id}", entity.Id);

        return Ok(entity);
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult<Product>> DeleteProduct(int id)
    {
        var entity = await _context.Products.FindAsync(id);
        if(entity == null)
        {
            return NotFound();
        }

        _context.Products.Remove(entity);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Deleted a product with Id: {Id}", entity.Id);

        return Ok(entity);
    }
}
