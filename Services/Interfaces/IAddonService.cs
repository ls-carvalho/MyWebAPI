using MyWebAPI.DataTransferObject.ReturnDtos;

namespace MyWebAPI.Services.Interfaces;
public interface IAddonService
{
    Task<IEnumerable<AddonDto>> GetAllAddonsAsync();
    Task<AddonDto?> GetAddonByIdAsync(int id);
    Task<AddonDto> CreateAddonAsync(AddonDto addon);
    Task<AddonDto> UpdateAddonAsync(AddonDto addon);
    Task<AddonDto> DeleteAddonAsync(int id);
}
