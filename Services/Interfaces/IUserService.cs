using MyWebAPI.DataTransferObject;
using MyWebAPI.DataTransferObject.ReturnDtos;

namespace MyWebAPI.Services.Interfaces;
public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<UserDto?> GetUserByIdAsync(int id);
    Task<UserDto> CreateUserAsync(CreateUserDto user);
    Task<UserDto> UpdateUserAsync(UpdateUserDto user);
    Task<UserDto> DeleteUserAsync(int id);
}
