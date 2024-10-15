using Microsoft.EntityFrameworkCore;
using MyWebAPI.Context;
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

    public async Task<AddonDto> CreateAddonAsync(AddonDto addon)
    {
        await ValidateDto(addon);

        // TO-DO: 2ª chamada ao banco para a mesma coisa. Não sei a melhor
        // forma de tirar isso ainda
        var product = _context.Products.Find(addon.ProductId);
        var entity = new Addon()
        {
            Name = addon.Name,
            ProductId = addon.ProductId,
            Product = product
        };

        _context.Addons.Add(entity);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Created an addon with Id: {Id}", entity.Id);

        addon.Id = entity.Id;
        return addon;
    }

    public async Task<AddonDto> UpdateAddonAsync(AddonDto addon)
    {
        await ValidateDto(addon);

        var entity = await _context.Addons.FindAsync(addon.Id);
        ValidateExists(entity, addon.Id);

        entity.Name = addon.Name;
        entity.ProductId = addon.ProductId;

        await _context.SaveChangesAsync();
        _logger.LogInformation("Updated an addon with Id: {Id}", addon.Id);
        return addon;
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

    private async Task ValidateDto(AddonDto addon)
    {
        // Não tem efeito prático, precisa mudar
        if (addon is null)
        {
            _logger.LogWarning("Request body invalid");
            throw new ArgumentNullException(nameof(addon), "Request body invalid");
        }

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
    }

    private void ValidateExists(Addon? entity, int id)
    {
        if (entity is null)
        {
            throw new InvalidOperationException($"Addon not found with Id: {id}");
        }
    }
}
