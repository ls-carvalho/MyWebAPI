using Microsoft.EntityFrameworkCore;
using MyWebAPI.Context;
using MyWebAPI.Controllers;
using MyWebAPI.DataTransferObject;
using MyWebAPI.Models;
using MyWebAPI.Services.Interfaces;

namespace MyWebAPI.Services;

public class UserService : IUserService
{
    private readonly ILogger<UserController> _logger;
    private readonly AppDbContext _context;

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
        return await _context.Users.Include(u => u.Account).FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<User> CreateUserAsync(CreateUserDto user)
    {
        // Não tem efeito prático, precisa mudar
        if (user is null)
        {
            _logger.LogWarning("Request body invalid");
            throw new ArgumentNullException(nameof(user), "Request body invalid");
        }

        // Validações pertinentes seriam realizadas nesse momento

        var entity = new User()
        {
            Username = user.Username,
            Password = user.Password,
            Account = new Account()
            {
                DisplayName = user.Account.DisplayName,
            },
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
            throw new ArgumentNullException(nameof(user), "Request body invalid");
        }

        var entity = await _context.Users.FindAsync(user.Id);

        if (entity is null)
        {
            _logger.LogWarning("User not found with Id: {Id}", user.Id);
            throw new KeyNotFoundException($"User not found with Id: {user.Id}");
        }

        entity.Username = user.Username;
        entity.Password = user.Password;

        await _context.SaveChangesAsync();
        _logger.LogInformation("Updated a user with Id: {Id}", user.Id);
        return entity;
    }

    public async Task<User> DeleteUserAsync(int id)
    {
        var entity = await _context.Users.Include(u => u.Account).FirstOrDefaultAsync(u => u.Id == id);
        if (entity is null)
        {
            _logger.LogWarning("User not found with Id: {Id}", id);
            throw new KeyNotFoundException($"User not found with Id: {id}");
        }

        if (entity.Account is not null)
        {
            var account = entity.Account;
            _context.Accounts.Remove(account);
            _logger.LogInformation("Deleted an account with Id: {Id}", account.Id);
        }

        _context.Users.Remove(entity);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Deleted a user with Id: {Id}", id);

        return entity;
    }
}
