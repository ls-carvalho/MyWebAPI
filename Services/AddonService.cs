using Microsoft.EntityFrameworkCore;
using MyWebAPI.Context;
using MyWebAPI.Controllers;
using MyWebAPI.DataTransferObject;
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
            throw new ArgumentNullException(nameof(addon), "Request body invalid");
        }

        var entity = new Addon()
        {
            Name = addon.Name,
            Product = addon.Product,
        };

        _context.Addons.Add(entity);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Created an addon with Id: {Id}", entity.Id);

        return entity;
    }

    public async Task<Addon> UpdateAddonAsync(UpdateAddonDto addon)
    {
        // Não tem efeito prático, precisa mudar
        if (addon is null)
        {
            _logger.LogWarning("Request body invalid");
            throw new ArgumentNullException(nameof(addon), "Request body invalid");
        }

        var entity = await _context.Addons.FindAsync(addon.Id);

        if (entity is null)
        {
            _logger.LogWarning("Addon not found with Id: {Id}", addon.Id);
            throw new KeyNotFoundException($"Addon not found with Id: {addon.Id}");
        }

        entity.Name = addon.Name;
        entity.Product = addon.Product;

        await _context.SaveChangesAsync();
        _logger.LogInformation("Updated an addon with Id: {Id}", addon.Id);
        return entity;
    }

    public async Task<Addon> DeleteAddonAsync(int id)
    {
        var entity = await _context.Addons.FindAsync(id);
        if (entity is null)
        {
            _logger.LogWarning("Addon not found with Id: {Id}", id);
            throw new KeyNotFoundException($"Addon not found with Id: {id}");
        }

        _context.Addons.Remove(entity);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Deleted an addon with Id: {Id}", id);

        return entity;
    }
}
