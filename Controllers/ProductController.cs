using Microsoft.AspNetCore.Mvc;
using MyWebAPI.DataTransferObject;
using MyWebAPI.DataTransferObject.ReturnDtos;
using MyWebAPI.Services.Interfaces;

namespace MyWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    [Route("all")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProductsAsync()
    {
        var result = await _productService.GetAllProductsAsync();
        return Ok(result);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<ProductDto>> GetProductByIdAsync(int id)
    {
        var result = await _productService.GetProductByIdAsync(id);
        if (result is null)
        {
            return BadRequest($"Product not found with Id: {id}");
        }

        return Ok(result);
    }

    [HttpPost]
    [Route("create")]
    public async Task<ActionResult<ProductDto>> CreateProductAsync(CreateProductDto product)
    {
        try
        {
            var result = await _productService.CreateProductAsync(product);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Route("update")]
    public async Task<ActionResult<ProductDto>> UpdateProductAsync(UpdateProductDto product)
    {
        try
        {
            var entity = await _productService.UpdateProductAsync(product);
            return Ok(entity);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Route("add-addons")]
    public async Task<ActionResult<ProductDto>> AddProductAddonsAsync(AddProductAddonsDto product)
    {
        try
        {
            var entity = await _productService.AddProductAddonsAsync(product);
            return Ok(entity);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult<ProductDto>> DeleteProductAsync(int id)
    {
        try
        {
            var entity = await _productService.DeleteProductAsync(id);
            return Ok(entity);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
