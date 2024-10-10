using Microsoft.EntityFrameworkCore;
using MyWebAPI.Context;
using MyWebAPI.Controllers;
using MyWebAPI.DataTransferObject.Account;
using MyWebAPI.Models;
using MyWebAPI.Services.Interfaces;

namespace MyWebAPI.Services;

public class AccountService : IAccountService
{
    private readonly IUserService _userService;
    private readonly ILogger<AccountController> _logger;
    public readonly AppDbContext _context;

    public AccountService(IUserService userService, ILogger<AccountController> logger, AppDbContext context)
    {
        _userService = userService;
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Account>> GetAllAccountsAsync()
    {
        return await _context.Accounts.OrderBy(account => account.Id).ToListAsync();
    }

    public async Task<Account?> GetAccountByIdAsync(int id)
    {
        return await _context.Accounts.FindAsync(id);
    }

    public async Task<Account> CreateAccountAsync(CreateAccountDto account)
    {
        // Não tem efeito prático, precisa mudar
        if (account is null)
        {
            _logger.LogWarning("Request body invalid");
            throw new ArgumentNullException(nameof(account));
        }

        var entity = new Account()
        {
            DisplayName = account.DisplayName,
            User = account.User,
        };

        _context.Accounts.Add(entity);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Created a account with Id: {Id}", entity.Id);

        return entity;
    }

    public async Task<Account> UpdateAccountAsync(UpdateAccountDto account)
    {
        // Não tem efeito prático, precisa mudar
        if (account is null)
        {
            _logger.LogWarning("Request body invalid");
            throw new ArgumentNullException(nameof(account));
        }

        var entity = await _context.Accounts.FindAsync(account.Id);

        if (entity is null)
        {
            _logger.LogWarning("Account not found");
            throw new KeyNotFoundException(nameof(account));
        }

        entity.DisplayName = account.DisplayName;
        entity.User = account.User;

        await _context.SaveChangesAsync();
        _logger.LogInformation("Updated a account with Id: {Id}", account.Id);
        return entity;
    }

    public async Task<Account> DeleteAccountAsync(int id)
    {
        var entity = await _context.Accounts.FindAsync(id);
        if (entity is null)
        {
            _logger.LogWarning("Account not found");
            throw new KeyNotFoundException("{id}");
        }

        _context.Accounts.Remove(entity);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Deleted a account with Id: {Id}", id);

        return entity;
    }
}
