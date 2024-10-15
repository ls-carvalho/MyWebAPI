using MyWebAPI.DataTransferObject;
using MyWebAPI.DataTransferObject.ReturnDtos;
using MyWebAPI.Models;

namespace MyWebAPI.Services.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllProductsAsync();
    Task<ProductDto?> GetProductByIdAsync(int id);
    Task<Product> CreateProductAsync(CreateProductDto product);
    Task<Product> UpdateProductAsync(UpdateProductDto product);
    Task<Product> AddProductAddonsAsync(AddProductAddonsDto product);
    Task<Product> DeleteProductAsync(int id);
}
