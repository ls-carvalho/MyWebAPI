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

    // TODO - ARTHUR: Alterar o GetAccountByIdAsync e GetAllAccountsAsync para retornar a lista de produtos associados a conta.
    // DICA: Combine a lógica de mapeamento do metodo de adicionar relação com a lógica que você já usou em outros GETs.
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

    public async Task<AccountDto> AddProductToAccountAsync(AddProductToAccountDto accountProduct)
    {
        // Recuperar a Account
        var accountEntity = await _context.Accounts.FindAsync(accountProduct.AccountId);
        
        // Validar a existencia da Account
        if (accountEntity is null) throw new Exception("Account com ID " + accountProduct.AccountId + " não encontrada!");
        
        // Recuperar o Product
        var productEntity = await _context.Products.FindAsync(accountProduct.ProductId);
        
        // Validar a existencia do Product
        if (productEntity is null) throw new Exception("Product com ID " + accountProduct.ProductId + " não encontrado!");
        
        // Validar a existencia da futura relação
        var accountProductEntity = await _context.AccountProducts.FirstOrDefaultAsync(ap => ap.ProductId == accountProduct.ProductId && ap.AccountId == accountProduct.AccountId);
        if (accountProductEntity is not null) throw new Exception("A account " + accountProduct.AccountId + " já possui o product " + accountProduct.ProductId + "!");
        
        // Criar a relação
        accountProductEntity = new AccountProduct()
        {
            AccountId = accountProduct.AccountId,
            ProductId = accountProduct.ProductId,
        };
        
        // Alternativamente:
        //  accountProductEntity = new AccountProduct()
        //  {
        //        Account = accountEntity,
        //        Product = productEntity,
        //  };

        await _context.AddAsync(accountProductEntity);
        await _context.SaveChangesAsync();

        // Retornar a Account completa em forma de DTO
        var account = await _context.Accounts
            .Include(a => a.Products)
            .ThenInclude(ap => ap.Product)
            .ThenInclude(p => p.Addons)
            .FirstAsync(a => a.Id == accountProductEntity.AccountId)
            ;

        var produtos = new List<ProductDto>();
        foreach(var produto in account.Products)
        {
            var addons = new List<AddonDto>();
            foreach(var addon in produto.Product.Addons)
            {
                var addonItem = new AddonDto()
                {
                    Id = addon.Id,
                    Name = addon.Name,
                    // Pode ser melhor criar uma outra DTO sem esse ID
                    // E então lembrar de remover essa property desnecessária.
                    ProductId = addon.ProductId,
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

    // TODO - ARTHUR: Criar um método que remova uma relação AccountProduct dado os Ids
    // DICA: Basta recuperar ela e dar um "_context.RemoveAsync(accountProductEntity);"
}
