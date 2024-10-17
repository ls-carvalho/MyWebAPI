using Microsoft.EntityFrameworkCore;
using MyWebAPI.Context;
using MyWebAPI.DataTransferObject;
using MyWebAPI.DataTransferObject.ReturnDtos;
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

    public async Task<IEnumerable<AccountDto>> GetAllAccountsAsync()
    {
        var entityList = await _context.Accounts
            .OrderBy(account => account.Id)
            .ToListAsync();

        var dtoList = new List<AccountDto>();

        foreach (var entity in entityList)
        {
            var dto = new AccountDto()
            {
                Id = entity.Id,
                DisplayName = entity.DisplayName,
            };

            dtoList.Add(dto);
        }

        return dtoList;
    }

    public async Task<AccountDto?> GetAccountByIdAsync(int id)
    {
        var entity = await _context.Accounts
            .FirstOrDefaultAsync(a => a.Id == id);

        if (entity == null) return null;

        var dto = new AccountDto()
        {
            Id = entity.Id,
            DisplayName = entity.DisplayName,
        };

        return dto;
    }

    public async Task<AccountDto> UpdateAccountAsync(UpdateAccountDto account)
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

        var returnDto = new AccountDto()
        {
            Id = entity.Id,
            DisplayName = entity.DisplayName,
        };

        return returnDto;
    }
}
