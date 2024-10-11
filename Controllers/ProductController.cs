using Microsoft.AspNetCore.Mvc;
using MyWebAPI.Context;
using MyWebAPI.DataTransferObject.Product;
using MyWebAPI.Models;
using MyWebAPI.Services.Interfaces;

namespace MyWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(ILogger<ProductController> logger, AppDbContext context, IProductService productService)
    {
        _logger = logger;
        _context = context;
        _productService = productService;
    }

    [HttpGet]
    [Route("all")]
    public async Task<ActionResult<IEnumerable<Product>>> GetAllProductsAsync()
    {
        var result = await _productService.GetAllProductsAsync();
        return Ok(result);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<Product>> GetProductByIdAsync(int id)
    {
        var result = await _productService.GetProductByIdAsync(id);
        if (result is null)
        {
            return NotFound("Product not found");
        }

        return Ok(result);
    }

    [HttpPost]
    [Route("create")]
    public async Task<ActionResult<Product>> CreateProductAsync(CreateProductDto product)
    {
        try
        {
            var result = await _productService.CreateProductAsync(product);
            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            return NotFound("Request body invalid");
        }
    }

    [HttpPost]
    [Route("update")]
    public async Task<ActionResult<Product>> UpdateProductAsync(UpdateProductDto product)
    {
        try
        {
            var entity = await _productService.UpdateProductAsync(product);
            return Ok(entity);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest("Request body invalid");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound("Product not found for update");
        }
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult<Product>> DeleteProductAsync(int id)
    {
        try
        {
            var entity = await _productService.DeleteProductAsync(id);
            return Ok(entity);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound("Product not found for deletion");
        }

    }
}
