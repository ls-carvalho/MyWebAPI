using Microsoft.EntityFrameworkCore;
using MyWebAPI.Context;
using MyWebAPI.DataTransferObject;
using MyWebAPI.DataTransferObject.ReturnDtos;
using MyWebAPI.Models;
using MyWebAPI.Services.Interfaces;

namespace MyWebAPI.Services;

public class AddonService : IAddonService
{
    private readonly ILogger<AddonService> _logger;
    public readonly AppDbContext _context;

    public AddonService(ILogger<AddonService> logger, AppDbContext context)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<AddonDto?> GetAddonByIdAsync(int id)
    {
        var entity = await _context.Addons.Include(a => a.Product).FirstOrDefaultAsync(a => a.Id == id);

        if (entity == null) return null;

        var dto = new AddonDto()
        {
            Id = entity.Id,
            Name = entity.Name,
            ProductId = entity.ProductId,
        };

        return dto;
    }

    public async Task<AddonDto> CreateAddonAsync(CreateAddonDto addon)
    {
        if (string.IsNullOrWhiteSpace(addon.Name))
        {
            _logger.LogWarning("Addon name is empty or null");
            throw new InvalidOperationException("Addon name cannot be empty");
        }

        var product = await _context.Products.FindAsync(addon.ProductId);
        if (product is null)
        {
            _logger.LogWarning("Product not found with Id: {Id}", addon.ProductId);
            throw new InvalidOperationException($"Product not found with Id: {addon.ProductId}");
        }

        var entity = new Addon()
        {
            Name = addon.Name,
            ProductId = addon.ProductId,
            Product = product
        };

        _context.Addons.Add(entity);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Created an addon with Id: {Id}", entity.Id);

        var returnDto = new AddonDto()
        {
            Id = entity.Id,
            Name = entity.Name,
            ProductId = entity.ProductId

        };
        return returnDto;
    }

    public async Task<AddonDto> UpdateAddonAsync(UpdateAddonDto addon)
    {
        if (string.IsNullOrWhiteSpace(addon.Name))
        {
            _logger.LogWarning("Addon name is empty or null");
            throw new InvalidOperationException("Addon name cannot be empty");
        }

        var entity = await _context.Addons.FindAsync(addon.Id);

        if (entity is null)
        {
            _logger.LogWarning("Addon not found with Id: {Id}", addon.Id);
            throw new InvalidOperationException($"Addon not found with Id: {addon.Id}");
        }

        entity.Name = addon.Name;

        await _context.SaveChangesAsync();
        _logger.LogInformation("Updated an addon with Id: {Id}", addon.Id);

        var returnDto = new AddonDto()
        {
            Id = entity.Id,
            Name = entity.Name,
            ProductId = entity.ProductId

        };
        return returnDto;
    }

    public async Task<AddonDto> DeleteAddonAsync(int id)
    {
        var entity = await _context.Addons.FindAsync(id);
        if (entity is null)
        {
            _logger.LogWarning("Addon not found with Id: {Id}", id);
            throw new InvalidOperationException($"Addon not found with Id: {id}");
        }

        _context.Addons.Remove(entity);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Deleted an addon with Id: {Id}", id);

        var dto = new AddonDto()
        {
            Id = entity.Id,
            Name = entity.Name,
            ProductId = entity.ProductId
        };

        return dto;
    }
}
