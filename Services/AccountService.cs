using Microsoft.EntityFrameworkCore;
using MyWebAPI.Context;
using MyWebAPI.DataTransferObject;
using MyWebAPI.Models;
using MyWebAPI.Services.Interfaces;

namespace MyWebAPI.Services;

public class AccountService : IAccountService
{
    private readonly ILogger<AccountService> _logger;
    public readonly AppDbContext _context;

    public AccountService(ILogger<AccountService> logger, AppDbContext context)
    {
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

    public async Task<Account> UpdateAccountAsync(UpdateAccountDto account)
    {
        var entity = await _context.Accounts.FindAsync(account.Id);
        if (entity is null)
        {
            _logger.LogWarning("Account not found with Id: {Id}", account.Id);
            throw new KeyNotFoundException($"Account not found with Id: {account.Id}");
        }

        if (account.DisplayName.Length > 20)
        {
            _logger.LogWarning("DisplayName length cannot be more than 20");
            throw new KeyNotFoundException("DisplayName length cannot be more than 20");
        }

        entity.DisplayName = account.DisplayName;

        await _context.SaveChangesAsync();
        _logger.LogInformation("Updated an account with Id: {Id}", account.Id);
        return entity;
    }
}
