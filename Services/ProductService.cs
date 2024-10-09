using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebAPI.Context;
using MyWebAPI.DataTransferObject;
using MyWebAPI.Models;
using MyWebAPI.Services.Interfaces;
using System.Reflection.Metadata.Ecma335;

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

    public async Task<Product?> GetProductById(int id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<Product> CreateProduct(CreateProductDto product)
    {
        // Não tem efeito prático, precisa mudar
        if (product is null)
        {
            throw new ArgumentNullException(nameof(product));
        }

        var entity = new Product()
        {
            Name = product.Name,
            Description = product.Description,
            Value = product.Value,
        };

        _context.Products.Add(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    public async Task<Product> UpdateProduct(UpdateProductDto product)
    {
        // Não tem efeito prático, precisa mudar
        if (product is null)
        {
            throw new ArgumentNullException(nameof(product));
        }

        var entity = await _context.Products.FindAsync(product.Id);

        if (entity is null)
        {
            throw new KeyNotFoundException(nameof(product));
        }

        entity.Name = product.Name;
        entity.Description = product.Description;
        entity.Value = product.Value;

        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Product> DeleteProduct(int id)
    {
        var entity = await _context.Products.FindAsync(id);
        if (entity is null)
        {
            throw new KeyNotFoundException("{id}");
        }

        _context.Products.Remove(entity);
        await _context.SaveChangesAsync();
        return entity;
    }



}
