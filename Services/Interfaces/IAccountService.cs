using MyWebAPI.DataTransferObject;
using MyWebAPI.Models;

namespace MyWebAPI.Services.Interfaces;
public interface IAccountService
{
    Task<IEnumerable<Account>> GetAllAccountsAsync();
    Task<Account?> GetAccountByIdAsync(int id);
    Task<Account> CreateAccountAsync(CreateAccountDto account);
    Task<Account> UpdateAccountAsync(UpdateAccountDto account);
    Task<Account> DeleteAccountAsync(int id);
}
