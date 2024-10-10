using Microsoft.EntityFrameworkCore;
using MyWebAPI.Context;
using MyWebAPI.Controllers;
using MyWebAPI.DataTransferObject.Addon;
using MyWebAPI.Models;
using MyWebAPI.Services.Interfaces;

namespace MyWebAPI.Services;

public class AddonService : IAddonService
{
    private readonly ILogger<AddonController> _logger;
    public readonly AppDbContext _context;

    public AddonService(ILogger<AddonController> logger, AppDbContext context)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Addon>> GetAllAddonsAsync()
    {
        return await _context.Addons.OrderBy(addon => addon.Id).ToListAsync();
    }

    public async Task<Addon?> GetAddonByIdAsync(int id)
    {
        return await _context.Addons.FindAsync(id);
    }

    public async Task<Addon> CreateAddonAsync(CreateAddonDto addon)
    {
        // Não tem efeito prático, precisa mudar
        if (addon is null)
        {
            _logger.LogWarning("Request body invalid");
            throw new ArgumentNullException(nameof(addon));
        }

        var entity = new Addon()
        {
            Name = addon.Name,
            Product = addon.Product,
        };

        _context.Addons.Add(entity);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Created a addon with Id: {Id}", entity.Id);

        return entity;
    }

    public async Task<Addon> UpdateAddonAsync(UpdateAddonDto addon)
    {
        // Não tem efeito prático, precisa mudar
        if (addon is null)
        {
            _logger.LogWarning("Request body invalid");
            throw new ArgumentNullException(nameof(addon));
        }

        var entity = await _context.Addons.FindAsync(addon.Id);

        if (entity is null)
        {
            _logger.LogWarning("Addon not found");
            throw new KeyNotFoundException(nameof(addon));
        }

        entity.Name = addon.Name;
        entity.Product = addon.Product;

        await _context.SaveChangesAsync();
        _logger.LogInformation("Updated a addon with Id: {Id}", addon.Id);
        return entity;
    }

    public async Task<Addon> DeleteAddonAsync(int id)
    {
        var entity = await _context.Addons.FindAsync(id);
        if (entity is null)
        {
            _logger.LogWarning("Addon not found");
            throw new KeyNotFoundException("{id}");
        }

        _context.Addons.Remove(entity);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Deleted a addon with Id: {Id}", id);

        return entity;
    }
}
