using MyWebAPI.DataTransferObject.Product;
using MyWebAPI.Models;

namespace MyWebAPI.Services.Interfaces;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<Product?> GetProductByIdAsync(int id);
    Task<Product> CreateProductAsync(CreateProductDto product);
    Task<Product> UpdateProductAsync(UpdateProductDto product);
    Task<Product> DeleteProductAsync(int id);
}
