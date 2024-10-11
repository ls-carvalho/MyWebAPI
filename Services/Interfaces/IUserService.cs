using MyWebAPI.DataTransferObject;
using MyWebAPI.Models;

namespace MyWebAPI.Services.Interfaces;
public interface IUserService
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(int id);
    Task<User> CreateUserAsync(CreateUserDto user);
    Task<User> UpdateUserAsync(UpdateUserDto user);
    Task<User> DeleteUserAsync(int id);
}
