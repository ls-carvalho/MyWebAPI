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

    public AccountService(ILogger<AccountService> logger, AppDbContext context)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<AccountDto>> GetAllAccountsAsync()
    {
        var entityList = await _context.Accounts
            .Include(a => a.Products)
            .ThenInclude(ap => ap.Product)
            .ThenInclude(p => p.Addons)
            .OrderBy(account => account.Id)
            .ToListAsync();

        var dtoList = new List<AccountDto>();

        foreach (var entity in entityList)
        {
            var products = new List<ProductDto>();
            foreach (var produto in entity.Products)
            {
                var addons = new List<AddonWithoutProductIdDto>();
                foreach (var addon in produto.Product.Addons)
                {
                    var addonItem = new AddonWithoutProductIdDto()
                    {
                        Id = addon.Id,
                        Name = addon.Name
                    };
                    addons.Add(addonItem);
                }

                var productItem = new ProductDto()
                {
                    Id = produto.Product.Id,
                    Name = produto.Product.Name,
                    Description = produto.Product.Description,
                    Value = produto.Product.Value,
                    Addons = addons
                };
                products.Add(productItem);
            }

            var dto = new AccountDto()
            {
                Id = entity.Id,
                DisplayName = entity.DisplayName,
                Products = products,
            };

            dtoList.Add(dto);
        }

        return dtoList;
    }

    public async Task<AccountDto?> GetAccountByIdAsync(int id)
    {
        // Retornar a Account completa em forma de DTO
        var entity = await _context.Accounts
            .Include(a => a.Products)
            .ThenInclude(ap => ap.Product)
            .ThenInclude(p => p.Addons)
            .FirstAsync(a => a.Id == id);

        if (entity == null) return null;

        var products = new List<ProductDto>();
        foreach (var produto in entity.Products)
        {
            var addons = new List<AddonWithoutProductIdDto>();
            foreach (var addon in produto.Product.Addons)
            {
                var addonItem = new AddonWithoutProductIdDto()
                {
                    Id = addon.Id,
                    Name = addon.Name,
                };
                addons.Add(addonItem);
            }

            var productItem = new ProductDto()
            {
                Id = produto.Product.Id,
                Name = produto.Product.Name,
                Description = produto.Product.Description,
                Value = produto.Product.Value,
                Addons = addons
            };
            products.Add(productItem);
        }

        var dto = new AccountDto()
        {
            Id = entity.Id,
            DisplayName = entity.DisplayName,
            Products = products
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
        var account = await _context.Accounts
            .Include(a => a.Products)
            .ThenInclude(ap => ap.Product)
            .ThenInclude(p => p.Addons)
            .FirstAsync(a => a.Id == accountProductEntity.Account.Id);

        var products = new List<ProductDto>();
        foreach (var produto in account.Products)
        {
            var addons = new List<AddonWithoutProductIdDto>();
            foreach (var addon in produto.Product.Addons)
            {
                var addonItem = new AddonWithoutProductIdDto()
                {
                    Id = addon.Id,
                    Name = addon.Name,
                };
                addons.Add(addonItem);
            }

            var productItem = new ProductDto()
            {
                Id = produto.Product.Id,
                Name = produto.Product.Name,
                Description = produto.Product.Description,
                Value = produto.Product.Value,
                Addons = addons
            };
            products.Add(productItem);
        }

        var accountDto = new AccountDto()
        {
            Id = account.Id,
            DisplayName = account.DisplayName,
            Products = products
        };

        return accountDto;
    }

    public async Task<AccountDto> RemoveProductFromAccountAsync(AccountProductIdsDto accountProduct)
    {
        // Validar que a relação exista
        var accountProductEntity = await _context.AccountProducts.FirstOrDefaultAsync(ap => ap.ProductId == accountProduct.ProductId && ap.AccountId == accountProduct.AccountId);
        if (accountProductEntity is null)
        {
            _logger.LogWarning("No relation found between account {AccountId} and product {ProductId}", accountProduct.AccountId, accountProduct.ProductId);
            throw new InvalidOperationException($"No relation found between account {accountProduct.AccountId} and product {accountProduct.ProductId}");
        }

        // Retornar a Account completa em forma de DTO
        var account = await _context.Accounts
            .Include(a => a.Products)
            .ThenInclude(ap => ap.Product)
            .ThenInclude(p => p.Addons)
            .FirstAsync(a => a.Id == accountProductEntity.Account.Id);

        _context.AccountProducts.Remove(accountProductEntity);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Removed product {ProductId} from account {AccountId}", accountProduct.ProductId, accountProduct.AccountId);

        var produtos = new List<ProductDto>();
        foreach (var produto in account.Products)
        {
            var addons = new List<AddonWithoutProductIdDto>();
            foreach (var addon in produto.Product.Addons)
            {
                var addonItem = new AddonWithoutProductIdDto()
                {
                    Id = addon.Id,
                    Name = addon.Name,
                };
                addons.Add(addonItem);
            }

            var produtoItem = new ProductDto()
            {
                Id = produto.Product.Id,
                Name = produto.Product.Name,
                Description = produto.Product.Description,
                Value = produto.Product.Value,
                Addons = addons
            };
            produtos.Add(produtoItem);
        }

        var accountDto = new AccountDto()
        {
            Id = account.Id,
            DisplayName = account.DisplayName,
            Products = produtos
        };

        return accountDto;
    }
}
