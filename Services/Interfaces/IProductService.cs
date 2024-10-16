using MyWebAPI.DataTransferObject;
using MyWebAPI.DataTransferObject.ReturnDtos;

namespace MyWebAPI.Services.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllProductsAsync();
    Task<ProductDto?> GetProductByIdAsync(int id);
    Task<ProductDto> CreateProductAsync(CreateProductDto product);
    Task<ProductDto> UpdateProductAsync(UpdateProductDto product);
    Task<ProductDto> AddAddonsAsync(AddAddonsToProductDto product);
    Task<ProductDto> DeleteProductAsync(int id);
}
