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
        var result = await _productService.GetProductById(id);
        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    [Route("create")]
    public async Task<ActionResult<Product>> CreateProduct(CreateProductDto product)
    {
        try
        {
            var result = await _productService.CreateProduct(product);
            _logger.LogInformation("Created a product with Id: {Id}", result.Id);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, ex.Message);
            return NotFound();
        }
    }

    [HttpPost]
    [Route("update")]
    public async Task<ActionResult<Product>> UpdateProduct(UpdateProductDto product)
    {
        try
        {
            var entity = await _productService.UpdateProduct(product);
            _logger.LogInformation("Updated a product with Id: {Id}", product.Id);
            return Ok(product);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogWarning(ex, ex.Message);
            return BadRequest();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, ex.Message);
            return NotFound();
        }
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult<Product>> DeleteProduct(int id)
    {
        try
        {
            var entity = await _productService.DeleteProduct(id);
            _logger.LogInformation("Deleted a product with Id: {Id}", entity!.Id);
            return Ok(entity);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, ex.Message);
            return NotFound();
        }
    }
}
