using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MyWebAPI.Context;
using MyWebAPI.DataTransferObject;
using MyWebAPI.DataTransferObject.ReturnDtos;
using MyWebAPI.Models;
using MyWebAPI.Services.Interfaces;

namespace MyWebAPI.Services;

public class AccountService : IAccountService
{
    private readonly ILogger<AccountService> _logger;
    public readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public AccountService(ILogger<AccountService> logger, AppDbContext context, IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AccountDto>> GetAllAccountsAsync()
    {
        var dtoList = await _context.Accounts
            .Include(a => a.Products)
            .ThenInclude(ap => ap.Product)
            .ThenInclude(p => p.Addons)
            .OrderBy(account => account.Id)
            .ProjectTo<AccountDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return dtoList;
    }

    public async Task<AccountDto?> GetAccountByIdAsync(int id)
    {
        // Retornar a Account completa em forma de DTO
        var dto = await _context.Accounts
            .Include(a => a.Products)
            .ThenInclude(ap => ap.Product)
            .ThenInclude(p => p.Addons)
            .ProjectTo<AccountDto>(_mapper.ConfigurationProvider)
            .FirstAsync(a => a.Id == id);

        return dto;
    }

    public async Task<AccountDto> UpdateAccountAsync(UpdateAccountDto account)
    {
        var entity = await _context.Accounts
            .Include(a => a.Products)
            .ThenInclude(ap => ap.Product)
            .ThenInclude(p => p.Addons)
            .FirstOrDefaultAsync(a => a.Id == account.Id);

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

        return _mapper.Map<AccountDto>(entity);

    }

    public async Task<AccountDto> AddProductToAccountAsync(AccountProductIdsDto accountProduct)
    {
        // Recuperar e validar a Account
        var accountEntity = await _context.Accounts.FindAsync(accountProduct.AccountId);
        if (accountEntity is null)
        {
            _logger.LogWarning("Account not found with Id: {Id}", accountProduct.AccountId);
            throw new InvalidOperationException($"Account with Id {accountProduct.AccountId} not found");
        }

        // Recuperar e validar o Product
        var productEntity = await _context.Products.FindAsync(accountProduct.ProductId);
        if (productEntity is null)
        {
            _logger.LogWarning("Product not found with Id: {Id}", accountProduct.ProductId);
            throw new InvalidOperationException($"Product with Id {accountProduct.ProductId} not found");
        }

        // Validar que a futura relação não exista previamente
        var accountProductEntity = await _context.AccountProducts.FirstOrDefaultAsync(ap => ap.ProductId == accountProduct.ProductId && ap.AccountId == accountProduct.AccountId);
        if (accountProductEntity is not null)
        {
            _logger.LogWarning("Account {AccountId} already has product {ProductId}", accountProduct.AccountId, accountProduct.ProductId);
            throw new InvalidOperationException($"Account {accountProduct.AccountId} already has product {accountProduct.ProductId}");
        }

        // Criar a relação
        //accountProductEntity = new AccountProduct()
        //{
        //    AccountId = accountProduct.AccountId,
        //    ProductId = accountProduct.ProductId,
        //};

        // Alternativamente:
        accountProductEntity = new AccountProduct()
        {
            Account = accountEntity,
            Product = productEntity,
        };

        await _context.AddAsync(accountProductEntity);
        await _context.SaveChangesAsync();

        // Retornar a Account completa em forma de DTO
        var accountDto = await _context.Accounts
            .Include(a => a.Products)
            .ThenInclude(ap => ap.Product)
            .ThenInclude(p => p.Addons)
            .ProjectTo<AccountDto>(_mapper.ConfigurationProvider)
            .FirstAsync(a => a.Id == accountProductEntity.Account.Id);

        return accountDto;
    }

    public async Task<AccountDto> RemoveProductFromAccountAsync(AccountProductIdsDto accountProduct)
    {
        // Validar que a relação exista
        var accountProductEntity = await _context.AccountProducts
            .Include(a => a.Account)
            .Include(a => a.Product)
            .FirstOrDefaultAsync(ap => ap.ProductId == accountProduct.ProductId && ap.AccountId == accountProduct.AccountId);

        if (accountProductEntity is null)
        {
            _logger.LogWarning("No relation found between account {AccountId} and product {ProductId}", accountProduct.AccountId, accountProduct.ProductId);
            throw new InvalidOperationException($"No relation found between account {accountProduct.AccountId} and product {accountProduct.ProductId}");
        }

        _context.AccountProducts.Remove(accountProductEntity);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Removed product {ProductId} from account {AccountId}", accountProduct.ProductId, accountProduct.AccountId);

        // Retornar a Account completa em forma de DTO
        var accountDto = await _context.Accounts
            .Include(a => a.Products)
            .ThenInclude(ap => ap.Product)
            .ThenInclude(p => p.Addons)
            .ProjectTo<AccountDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(a => a.Id == accountProductEntity.Account.Id);

        return accountDto;
    }
}
