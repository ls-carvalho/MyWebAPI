using MyWebAPI.DataTransferObject;
using MyWebAPI.Models;

namespace MyWebAPI.Services.Interfaces;
public interface IAccountService
{
    Task<IEnumerable<Account>> GetAllAccountsAsync();
    Task<Account?> GetAccountByIdAsync(int id);
    Task<Account> UpdateAccountAsync(UpdateAccountDto account);
}
