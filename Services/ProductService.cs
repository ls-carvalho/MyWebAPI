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
        var entityList = await _context.Products
            .Include(p => p.Addons)
            .OrderBy(product => product.Id)
            .ToListAsync();

        var dtoList = new List<ProductDto>();

        foreach (var entity in entityList)
        {
            var dto = new ProductDto()
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Value = entity.Value,
                Addons = entity.Addons.Select(addon => new AddonWithoutProductIdDto()
                {
                    Id = addon.Id,
                    Name = addon.Name,
                }).ToList()
            };

            dtoList.Add(dto);
        }

        return dtoList;
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        var entity = await _context.Products
            .Include(p => p.Addons)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (entity == null) return null;

        var dto = new ProductDto()
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Value = entity.Value,
            Addons = entity.Addons.Select(addon => new AddonWithoutProductIdDto()
            {
                Id = addon.Id,
                Name = addon.Name,
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
        _logger.LogInformation("Created a addonsToProductDto with Id: {Id}", entity.Id);

        var returnDto = new ProductDto()
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Value = entity.Value,
            Addons = entity.Addons.Select(addon => new AddonWithoutProductIdDto()
            {
                Id = addon.Id,
                Name = addon.Name,
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

        var entity = await _context.Products
            .Include(p => p.Addons)
            .FirstOrDefaultAsync(p => p.Id == product.Id);

        if (entity is null)
        {
            _logger.LogWarning("Product not found with Id: {Id}", product.Id);
            throw new InvalidOperationException($"Product not found with Id: {product.Id}");
        }

        entity.Name = product.Name;
        entity.Description = product.Description;
        entity.Value = product.Value;

        await _context.SaveChangesAsync();
        _logger.LogInformation("Updated a addonsToProductDto with Id: {Id}", product.Id);

        var returnDto = new ProductDto()
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Value = entity.Value,
            Addons = entity.Addons.Select(addon => new AddonWithoutProductIdDto()
            {
                Id = addon.Id,
                Name = addon.Name,
            }).ToList()
        };

        return returnDto;
    }

    public async Task<ProductDto> AddAddonsAsync(AddAddonsToProductDto addonsToProductDto)
    {
        var product = await _context.Products
            .Include(p => p.Addons)
            .FirstOrDefaultAsync(p => p.Id == addonsToProductDto.Id);

        if (product == null)
        {
            _logger.LogWarning("Product not found with Id: {Id}", addonsToProductDto.Id);
            throw new InvalidOperationException($"Product not found with Id: {addonsToProductDto.Id}");
        }

        foreach (var addon in addonsToProductDto.Addons)
        {
            var addonEntity = AddonDtoToEntityAsync(addon, product);

            var existingAddonNames = product.Addons.Select(a => a.Name);
            if (existingAddonNames.Contains(addon.Name))
            {
                _logger.LogWarning("Addon with name '{addonName}' already exists", addon.Name);
                throw new Exception(($"Addon with name '{addon.Name}' already exists"));
            }

            product.Addons.Add(addonEntity);
        }

        await _context.SaveChangesAsync();
        _logger.LogInformation("Added Addons to a addonsToProductDto with Id: {Id}", addonsToProductDto.Id);

        var returnDto = new ProductDto()
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Value = product.Value,
            Addons = product.Addons.Select(addon => new AddonWithoutProductIdDto()
            {
                Id = addon.Id,
                Name = addon.Name,
            }).ToList()
        };

        return returnDto;
    }

    public async Task<ProductDto> DeleteProductAsync(int id)
    {
        var entity = await _context.Products
            .Include(p => p.Addons)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (entity is null)
        {
            _logger.LogWarning("Product not found");
            throw new KeyNotFoundException($"Product not found with Id {id}");
        }

        _context.Products.Remove(entity);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Deleted a addonsToProductDto with Id: {Id}", id);

        var returnDto = new ProductDto()
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Value = entity.Value,
            Addons = entity.Addons.Select(addon => new AddonWithoutProductIdDto()
            {
                Id = addon.Id,
                Name = addon.Name,
            }).ToList()
        };
        return returnDto;
    }

    private Addon AddonDtoToEntityAsync(CreateAddonWithoutProductIdDto addon, Product product)
    {
        if (string.IsNullOrWhiteSpace(addon.Name))
        {
            _logger.LogWarning("Addon name is empty or null");
            throw new InvalidOperationException("Addon name cannot be empty");
        }

        var entity = new Addon()
        {
            Name = addon.Name,
            Product = product
        };

        return entity;
    }
}
