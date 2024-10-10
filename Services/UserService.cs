using Microsoft.EntityFrameworkCore;
using MyWebAPI.Context;
using MyWebAPI.Controllers;
using MyWebAPI.DataTransferObject.User;
using MyWebAPI.Models;
using MyWebAPI.Services.Interfaces;

namespace MyWebAPI.Services;

public class UserService : IUserService
{
    private readonly ILogger<UserController> _logger;
    public readonly AppDbContext _context;

    public UserService(ILogger<UserController> logger, AppDbContext context)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _context.Users.OrderBy(user => user.Id).ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User> CreateUserAsync(CreateUserDto user)
    {
        // Não tem efeito prático, precisa mudar
        if (user is null)
        {
            _logger.LogWarning("Request body invalid");
            throw new ArgumentNullException(nameof(user));
        }

        var entity = new User()
        {
            Username = user.Username,
            Password = user.Password,
            Account = user.Account,
        };

        _context.Users.Add(entity);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Created a user with Id: {Id}", entity.Id);

        return entity;
    }

    public async Task<User> UpdateUserAsync(UpdateUserDto user)
    {
        // Não tem efeito prático, precisa mudar
        if (user is null)
        {
            _logger.LogWarning("Request body invalid");
            throw new ArgumentNullException(nameof(user));
        }

        var entity = await _context.Users.FindAsync(user.Id);

        if (entity is null)
        {
            _logger.LogWarning("User not found");
            throw new KeyNotFoundException(nameof(user));
        }

        entity.Name = user.Name;
        entity.Description = user.Description;
        entity.Value = user.Value;

        await _context.SaveChangesAsync();
        _logger.LogInformation("Updated a user with Id: {Id}", user.Id);
        return entity;
    }

    public async Task<User> DeleteUserAsync(int id)
    {
        var entity = await _context.Users.FindAsync(id);
        if (entity is null)
        {
            _logger.LogWarning("User not found");
            throw new KeyNotFoundException("{id}");
        }

        _context.Users.Remove(entity);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Deleted a user with Id: {Id}", id);

        return entity;
    }
}
