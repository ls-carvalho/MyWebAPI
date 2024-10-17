using MyWebAPI.DataTransferObject;
using MyWebAPI.DataTransferObject.ReturnDtos;

namespace MyWebAPI.Services.Interfaces;
public interface IAccountService
{
    Task<IEnumerable<AccountDto>> GetAllAccountsAsync();
    Task<AccountDto?> GetAccountByIdAsync(int id);
    Task<AccountDto> UpdateAccountAsync(UpdateAccountDto account);
    Task<AccountDto> AddProductToAccountAsync(AddProductToAccountDto accountProduct);
}
