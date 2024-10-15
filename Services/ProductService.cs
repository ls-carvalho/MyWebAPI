using Microsoft.EntityFrameworkCore;
using MyWebAPI.Context;
using MyWebAPI.DataTransferObject;
using MyWebAPI.DataTransferObject.ReturnDtos;
using MyWebAPI.Models;
using MyWebAPI.Services.Interfaces;

namespace MyWebAPI.Services;

public class ProductService : IProductService
{
    private readonly ILogger<ProductService> _logger;
    public readonly AppDbContext _context;

    public ProductService(ILogger<ProductService> logger, AppDbContext context)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        var entityList = await _context.Products.Include(p => p.Addons).Include(p => p.Accounts).OrderBy(product => product.Id).ToListAsync();
        var dtoList = new List<ProductDto>();

        foreach (var entity in entityList)
        {
            var dto = new ProductDto()
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Value = entity.Value,
                Addons = entity.Addons.Select(addon => new AddonDto()
                {
                    Id = addon.Id,
                    Name = addon.Name,
                    ProductId = addon.ProductId,
                }).ToList()
            };

            dtoList.Add(dto);
        }

        return dtoList;
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        var entity = await _context.Products.Include(p => p.Addons).FirstOrDefaultAsync(p => p.Id == id);

        if (entity == null) return null;

        var dto = new ProductDto()
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Value = entity.Value,
            Addons = entity.Addons.Select(addon => new AddonDto()
            {
                Id = addon.Id,
                Name = addon.Name,
                ProductId = addon.ProductId,
            }).ToList()
        };

        return dto;
    }

    public async Task<ProductDto> CreateProductAsync(CreateProductDto product)
    {
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

        var entity = new Product()
        {
            Name = product.Name,
            Description = product.Description,
            Value = product.Value,
        };

        _context.Products.Add(entity);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Created a product with Id: {Id}", entity.Id);

        var returnDto = new ProductDto()
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Value = entity.Value,
            Addons = entity.Addons.Select(addon => new AddonDto()
            {
                Id = addon.Id,
                Name = addon.Name,
                ProductId = addon.ProductId,
            }).ToList()
        };
        return returnDto;
    }

    public async Task<ProductDto> UpdateProductAsync(UpdateProductDto product)
    {
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

        var entity = await _context.Products.Include(p => p.Addons).FirstOrDefaultAsync(p => p.Id == product.Id);

        if (entity is null)
        {
            _logger.LogWarning("Product not found with Id: {Id}", product.Id);
            throw new InvalidOperationException($"Product not found with Id: {product.Id}");
        }

        entity.Name = product.Name;
        entity.Description = product.Description;
        entity.Value = product.Value;

        await _context.SaveChangesAsync();
        _logger.LogInformation("Updated a product with Id: {Id}", product.Id);

        var returnDto = new ProductDto()
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Value = entity.Value,
            Addons = entity.Addons.Select(addon => new AddonDto()
            {
                Id = addon.Id,
                Name = addon.Name,
                ProductId = addon.ProductId,
            }).ToList()
        };
        return returnDto;
    }

    public async Task<ProductDto> AddProductAddonsAsync(AddProductAddonsDto product)
    {
        var entity = await _context.Products.FindAsync(product.Id);
        if (entity == null)
        {
            _logger.LogWarning("Product not found with Id: {Id}", product.Id);
            throw new InvalidOperationException($"Product not found with Id: {product.Id}");
        }

        foreach (var addonId in product.AddonIds)
        {
            var addonEntity = await _context.Addons.FindAsync(addonId);
            if (addonEntity is null) throw new Exception("Addon não encontrado");

            if (entity.Addons.Contains(addonEntity)) throw new Exception("Addon já adicionado anteriormente");

            entity.Addons.Add(addonEntity);
        }

        await _context.SaveChangesAsync();
        _logger.LogInformation("Updated a product with Id: {Id}", product.Id);

        var returnDto = new ProductDto()
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Value = entity.Value,
            Addons = entity.Addons.Select(addon => new AddonDto()
            {
                Id = addon.Id,
                Name = addon.Name,
                ProductId = addon.ProductId,
            }).ToList()
        };
        return returnDto;
    }

    public async Task<ProductDto> DeleteProductAsync(int id)
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

        var returnDto = new ProductDto()
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Value = entity.Value,
            Addons = entity.Addons.Select(addon => new AddonDto()
            {
                Id = addon.Id,
                Name = addon.Name,
                ProductId = addon.ProductId,
            }).ToList()
        };
        return returnDto;
    }
}
