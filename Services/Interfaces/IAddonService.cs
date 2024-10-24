using MyWebAPI.DataTransferObject;
using MyWebAPI.DataTransferObject.ReturnDtos;

namespace MyWebAPI.Services.Interfaces;
public interface IAddonService
{
    Task<AddonDto?> GetAddonByIdAsync(int id);
    Task<AddonDto> CreateAddonAsync(CreateAddonDto addon);
    Task<AddonDto> UpdateAddonAsync(UpdateAddonDto addon);
    Task<AddonDto> DeleteAddonAsync(int id);
}
