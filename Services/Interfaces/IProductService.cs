using MyWebAPI.Models;

namespace MyWebAPI.Services.Interfaces;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllProductsAsync();
}
