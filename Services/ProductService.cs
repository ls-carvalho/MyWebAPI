using Microsoft.EntityFrameworkCore;
using MyWebAPI.Context;
using MyWebAPI.Models;
using MyWebAPI.Services.Interfaces;

namespace MyWebAPI.Services;

public class ProductService : IProductService
{
    public readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _context.Products.OrderBy(product => product.Id).ToListAsync();
    }
}
