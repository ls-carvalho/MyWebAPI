using Microsoft.EntityFrameworkCore;
using MyWebAPI.Context;
using MyWebAPI.DataTransferObject;
using MyWebAPI.DataTransferObject.ReturnDtos;
using MyWebAPI.Models;
using MyWebAPI.Services.Interfaces;
using System.Text.RegularExpressions;

namespace MyWebAPI.Services;

public class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly AppDbContext _context;

    public UserService(ILogger<UserService> logger, AppDbContext context)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var entityList = await _context.Users
            .Include(u => u.Account)
            .OrderBy(product => product.Id)
            .ToListAsync();

        var dtoList = new List<UserDto>();

        foreach (var entity in entityList)
        {
            var dto = new UserDto()
            {
                Id = entity.Id,
                Username = entity.Username,
                Password = entity.Password,
                Account = new AccountDto()
                {
                    Id = entity.Account.Id,
                    DisplayName = entity.Account.DisplayName,
                },
            };

            dtoList.Add(dto);
        }

        return dtoList;
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        var entity = await _context.Users
            .Include(u => u.Account)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (entity == null) return null;

        var dto = new UserDto()
        {
            Id = entity.Id,
            Username = entity.Username,
            Password = entity.Password,
            Account = new AccountDto()
            {
                Id = entity.Account.Id,
                DisplayName = entity.Account.DisplayName,
            },
        };

        return dto;
    }

    public async Task<UserDto> CreateUserAsync(CreateUserDto user)
    {
        if (user.Username.Length > 30)
        {
            _logger.LogWarning("Username length cannot be more than 30");
            throw new KeyNotFoundException("Username length cannot be more than 30");
        }

        if (user.Username.Length < 5)
        {
            _logger.LogWarning("Username length cannot be less than 5");
            throw new KeyNotFoundException("Username length cannot be less than 5");
        }

        var usernameHasSpaceCharacter = Regex.IsMatch(user.Username, @"\s");
        if (usernameHasSpaceCharacter)
        {
            _logger.LogWarning("Username cannot have any space characters");
            throw new KeyNotFoundException("Username cannot have any space characters");
        }

        if (user.Password.Length < 8)
        {
            _logger.LogWarning("Password length cannot be less than 8");
            throw new KeyNotFoundException("Password length cannot be less than 8");
        }

        var hasUpperCase = Regex.IsMatch(user.Password, "[A-Z]");
        if (!hasUpperCase)
        {
            _logger.LogWarning("Password must have at least one upper case character");
            throw new KeyNotFoundException("Password must have at least one upper case character");
        }

        var hasLowerCase = Regex.IsMatch(user.Password, "[a-z]");
        if (!hasLowerCase)
        {
            _logger.LogWarning("Password must have at least one lower case character");
            throw new KeyNotFoundException("Password must have at least one lower case character");
        }

        // Inserting special characters currently makes the API exit with code 'Access Violation'
        // Therefore, the validation below is suspended

        //var hasSpecialCharacter = Regex.IsMatch(user.Password, @"[!@#$%^&*(),.?""{}|<>_]");
        //if (!hasSpecialCharacter)
        //{
        //    _logger.LogWarning("Password must have at least one special character");
        //    throw new KeyNotFoundException("Password must have at least one special character");
        //}

        var passwordHasSpaceCharacter = Regex.IsMatch(user.Password, @"\s");
        if (passwordHasSpaceCharacter)
        {
            _logger.LogWarning("Password cannot have any space characters");
            throw new KeyNotFoundException("Password cannot have any space characters");
        }

        if (user.AccountDisplayName.Length > 20)
        {
            _logger.LogWarning("AccountDisplayName length cannot be more than 20");
            throw new KeyNotFoundException("AccountDisplayName length cannot be more than 20");
        }

        var entity = new User()
        {
            Username = user.Username,
            Password = user.Password,
            Account = new Account()
            {
                DisplayName = user.AccountDisplayName,
            },
        };

        _context.Users.Add(entity);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Created a user with Id: {Id}", entity.Id);

        var returnDto = new UserDto()
        {
            Id = entity.Id,
            Username = entity.Username,
            Password = entity.Password,
            Account = new AccountDto()
            {
                Id = entity.Account.Id,
                DisplayName = entity.Account.DisplayName,
            },
        };

        return returnDto;
    }

    public async Task<UserDto> UpdateUserAsync(UpdateUserDto user)
    {
        if (user.Username.Length > 30)
        {
            _logger.LogWarning("Username length cannot be more than 30");
            throw new KeyNotFoundException("Username length cannot be more than 30");
        }

        if (user.Username.Length < 5)
        {
            _logger.LogWarning("Username length cannot be less than 5");
            throw new KeyNotFoundException("Username length cannot be less than 5");
        }

        var usernameHasSpaceCharacter = Regex.IsMatch(user.Username, @"\s");
        if (usernameHasSpaceCharacter)
        {
            _logger.LogWarning("Username cannot have any space characters");
            throw new KeyNotFoundException("Username cannot have any space characters");
        }

        if (user.Password.Length < 8)
        {
            _logger.LogWarning("Username length cannot be less than 8");
            throw new KeyNotFoundException("Username length cannot be less than 8");
        }

        var hasUpperCase = Regex.IsMatch(user.Password, "[A-Z]");
        if (!hasUpperCase)
        {
            _logger.LogWarning("Password must have at least one upper case character");
            throw new KeyNotFoundException("Password must have at least one upper case character");
        }

        var hasLowerCase = Regex.IsMatch(user.Password, "[a-z]");
        if (!hasLowerCase)
        {
            _logger.LogWarning("Password must have at least one lower case character");
            throw new KeyNotFoundException("Password must have at least one lower case character");
        }

        // Inserting special characters currently makes the API exit with code 'Access Violation'
        // Therefore, the validation below is suspended

        //var hasSpecialCharacter = Regex.IsMatch(user.Password, @"[!@#$%^&*(),.?""{}|<>_]");
        //if (!hasSpecialCharacter)
        //{
        //    _logger.LogWarning("Password must have at least one special character");
        //    throw new KeyNotFoundException("Password must have at least one special character");
        //}

        var passwordHasSpaceCharacter = Regex.IsMatch(user.Password, @"\s");
        if (passwordHasSpaceCharacter)
        {
            _logger.LogWarning("Password cannot have any space characters");
            throw new KeyNotFoundException("Password cannot have any space characters");
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

        var returnDto = new UserDto()
        {
            Id = entity.Id,
            Username = entity.Username,
            Password = entity.Password,
            Account = new AccountDto()
            {
                Id = entity.Account.Id,
                DisplayName = entity.Account.DisplayName,
            },
        };

        return returnDto;
    }

    public async Task<UserDto> DeleteUserAsync(int id)
    {
        var entity = await _context.Users.Include(u => u.Account).FirstOrDefaultAsync(u => u.Id == id);
        if (entity is null)
        {
            _logger.LogWarning("User not found with Id: {Id}", id);
            throw new KeyNotFoundException($"User not found with Id: {id}");
        }

        var returnDto = new UserDto()
        {
            Id = entity.Id,
            Username = entity.Username,
            Password = entity.Password,
            Account = new AccountDto()
            {
                Id = entity.Account.Id,
                DisplayName = entity.Account.DisplayName,
            },
        };

        if (entity.Account is not null)
        {
            var account = entity.Account;
            _context.Accounts.Remove(account);
            _logger.LogInformation("Deleted an account with Id: {Id}", account.Id);
        }

        _context.Users.Remove(entity);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Deleted a user with Id: {Id}", id);

        return returnDto;
    }
}
