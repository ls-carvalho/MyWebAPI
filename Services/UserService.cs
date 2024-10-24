using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    private readonly IMapper _mapper;

    public UserService(ILogger<UserService> logger, AppDbContext context, IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        return await _context.Users
            .Include(u => u.Account)
            .OrderBy(user => user.Id)
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        return await _context.Users
            .Include(u => u.Account)
            .OrderBy(user => user.Id)
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .FirstAsync(u => u.Id == id);
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

        var hasSpecialCharacter = Regex.IsMatch(user.Password, @"[!@#$%^&*(),.?""{}|<>_]");
        if (!hasSpecialCharacter)
        {
            _logger.LogWarning("Password must have at least one special character");
            throw new KeyNotFoundException("Password must have at least one special character");
        }

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

        var entity = _mapper.Map<User>(user);

        _context.Users.Add(entity);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Created a user with Id: {Id}", entity.Id);

        return _mapper.Map<UserDto>(entity);
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

        var hasSpecialCharacter = Regex.IsMatch(user.Password, @"[!@#$%^&*(),.?""{}|<>_]");
        if (!hasSpecialCharacter)
        {
            _logger.LogWarning("Password must have at least one special character");
            throw new KeyNotFoundException("Password must have at least one special character");
        }

        var passwordHasSpaceCharacter = Regex.IsMatch(user.Password, @"\s");
        if (passwordHasSpaceCharacter)
        {
            _logger.LogWarning("Password cannot have any space characters");
            throw new KeyNotFoundException("Password cannot have any space characters");
        }

        var entity = await _context.Users
            .Include(u => u.Account)
            .ThenInclude(a => a.Products)
            .ThenInclude(ap => ap.Product)
            .ThenInclude(p => p.Addons)
            .FirstOrDefaultAsync(p => p.Id == user.Id);

        if (entity is null)
        {
            _logger.LogWarning("User not found with Id: {Id}", user.Id);
            throw new KeyNotFoundException($"User not found with Id: {user.Id}");
        }

        _mapper.Map(user, entity);

        await _context.SaveChangesAsync();
        _logger.LogInformation("Updated a user with Id: {Id}", user.Id);

        return _mapper.Map<UserDto>(entity);
    }

    public async Task<UserDto> DeleteUserAsync(int id)
    {
        var entity = await _context.Users
            .Include(u => u.Account)
            .ThenInclude(a => a.Products)
            .ThenInclude(ap => ap.Product)
            .ThenInclude(p => p.Addons)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (entity is null)
        {
            _logger.LogWarning("User not found with Id: {Id}", id);
            throw new KeyNotFoundException($"User not found with Id: {id}");
        }

        var returnDto = _mapper.Map<UserDto>(entity);

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
