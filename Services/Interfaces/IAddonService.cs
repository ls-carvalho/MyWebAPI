using MyWebAPI.DataTransferObject;
using MyWebAPI.Models;

namespace MyWebAPI.Services.Interfaces;
public interface IAddonService
{
    Task<IEnumerable<Addon>> GetAllAddonsAsync();
    Task<Addon?> GetAddonByIdAsync(int id);
    Task<CreateAddonDto> CreateAddonAsync(CreateAddonDto addon);
    Task<UpdateAddonDto> UpdateAddonAsync(UpdateAddonDto addon);
    Task<Addon> DeleteAddonAsync(int id);
}
