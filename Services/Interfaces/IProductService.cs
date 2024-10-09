using Microsoft.AspNetCore.Mvc;
using MyWebAPI.Models;

namespace MyWebAPI.Services.Interfaces;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<Product?> GetProductById(int id);
    Task<Product> CreateProduct(CreateProductDto product);
    Task<Product> UpdateProduct(UpdateProductDto product);
    Task<Product> DeleteProduct(int id);


}
