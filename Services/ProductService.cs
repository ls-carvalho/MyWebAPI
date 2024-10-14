using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebAPI.Context;
using MyWebAPI.Controllers;
using MyWebAPI.DataTransferObject;
using MyWebAPI.Models;
using MyWebAPI.Services.Interfaces;
using System.Reflection.Metadata.Ecma335;

namespace MyWebAPI.Services;

public class ProductService : IProductService
{
    private readonly ILogger<ProductController> _logger;
    public readonly AppDbContext _context;

    public ProductService(ILogger<ProductController> logger, AppDbContext context)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _context.Products.Include(p => p.Addons).Include(p => p.Accounts).OrderBy(product => product.Id).ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<Product> CreateProductAsync(CreateProductDto product)
    {
        ValidateDto(product);

        var entity = new Product()
        {
            Name = product.Name,
            Description = product.Description,
            Value = product.Value,
            Addons = product.Addons,
        };

        _context.Products.Add(entity);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Created a product with Id: {Id}", entity.Id);

        return entity;
    }

    public async Task<Product> UpdateProductAsync(UpdateProductDto product)
    {
        ValidateDto(product);

        var entity = await _context.Products.FindAsync(product.Id);
        ValidateExists(entity, product.Id);

        entity.Name = product.Name;
        entity.Description = product.Description;
        entity.Value = product.Value;
        entity.Addons = product.Addons;

        await _context.SaveChangesAsync();
        _logger.LogInformation("Updated a product with Id: {Id}", product.Id);
        return entity;
    }

    public async Task<Product> AddProductAddonsAsync(AddProductAddonsDto product)
    {
        var entity = await _context.Products.FindAsync(product.Id);
        ValidateExists(entity, product.Id);

        foreach (var item in product.Addons)
        {
            entity.Addons.Add(item);
        }

        await _context.SaveChangesAsync();
        _logger.LogInformation("Updated a product with Id: {Id}", product.Id);
        return entity;
    }

    public async Task<Product> DeleteProductAsync(int id)
    {
        var entity = await _context.Products.FindAsync(id);
        if (entity is null)
        {
            _logger.LogWarning("Product not found");
            throw new KeyNotFoundException($"Product not found with Id {id}");
        }

        _context.Products.Remove(entity);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Deleted a product with Id: {Id}", id);

        return entity;
    }

    private void ValidateDto(CreateProductDto product)
    {
        // Não tem efeito prático, precisa mudar
        if (product is null)
        {
            _logger.LogWarning("Request body invalid");
            throw new ArgumentNullException(nameof(product), "Request body invalid");
        }

        if (string.IsNullOrWhiteSpace(product.Name))
        {
            _logger.LogWarning("Product name is empty or null");
            throw new InvalidOperationException("Product name cannot be empty");
        }

        if (string.IsNullOrWhiteSpace(product.Description))
        {
            _logger.LogWarning("Product description is empty or null");
            throw new InvalidOperationException("Product description cannot be empty");
        }
    }

    private void ValidateDto(UpdateProductDto product)
    {
        // Não tem efeito prático, precisa mudar
        if (product is null)
        {
            _logger.LogWarning("Request body invalid");
            throw new ArgumentNullException(nameof(product), "Request body invalid");
        }

        if (string.IsNullOrWhiteSpace(product.Name))
        {
            _logger.LogWarning("Product name is empty or null");
            throw new InvalidOperationException("Product name cannot be empty");
        }

        if (string.IsNullOrWhiteSpace(product.Description))
        {
            _logger.LogWarning("Product description is empty or null");
            throw new InvalidOperationException("Product description cannot be empty");
        }
    }

    private void ValidateExists(Product? entity, int id)
    {
        if (entity is null)
        {
            throw new InvalidOperationException($"Product not found with Id: {id}");
        }
    }
}
