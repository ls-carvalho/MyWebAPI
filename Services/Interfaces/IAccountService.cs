using MyWebAPI.DataTransferObject;
using MyWebAPI.DataTransferObject.ReturnDtos;
using MyWebAPI.Models;

namespace MyWebAPI.Services.Interfaces;
public interface IAccountService
{
    Task<IEnumerable<Account>> GetAllAccountsAsync();
    Task<Account?> GetAccountByIdAsync(int id);
    Task<Account> UpdateAccountAsync(UpdateAccountDto account);
    Task<AccountDto> AddProductToAccountAsync(AddProductToAccountDto accountProduct);
}
