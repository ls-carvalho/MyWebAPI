using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    private readonly IMapper _mapper;

    public ProductService(ILogger<ProductService> logger, AppDbContext context, IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        return await _context.Products
            .Include(p => p.Addons)
            .OrderBy(product => product.Id)
            .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        return await _context.Products
            .Include(p => p.Addons)
            .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(p => p.Id == id);
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

        var entity = _mapper.Map<Product>(product);

        _context.Products.Add(entity);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Created a addonsToProductDto with Id: {Id}", entity.Id);

        return _mapper.Map<ProductDto>(entity);
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

        return _mapper.Map<ProductDto>(entity);
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

        return _mapper.Map<ProductDto>(product);
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

        return _mapper.Map<ProductDto>(entity);
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
