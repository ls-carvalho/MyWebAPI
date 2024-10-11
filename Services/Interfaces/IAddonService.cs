using MyWebAPI.DataTransferObject;
using MyWebAPI.Models;

namespace MyWebAPI.Services.Interfaces;
public interface IAddonService
{
    Task<IEnumerable<Addon>> GetAllAddonsAsync();
    Task<Addon?> GetAddonByIdAsync(int id);
    Task<Addon> CreateAddonAsync(CreateAddonDto addon);
    Task<Addon> UpdateAddonAsync(UpdateAddonDto addon);
    Task<Addon> DeleteAddonAsync(int id);
}
