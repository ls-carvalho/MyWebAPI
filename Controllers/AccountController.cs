using Microsoft.AspNetCore.Mvc;
using MyWebAPI.DataTransferObject;
using MyWebAPI.DataTransferObject.ReturnDtos;
using MyWebAPI.Services.Interfaces;

namespace MyWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet]
    [Route("all")]
    public async Task<ActionResult<IEnumerable<AccountDto>>> GetAllAccountsAsync()
    {
        var result = await _accountService.GetAllAccountsAsync();
        return Ok(result);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<AccountDto>> GetAccountByIdAsync(int id)
    {
        var result = await _accountService.GetAccountByIdAsync(id);
        if (result is null)
        {
            return BadRequest($"Account not found with Id: {id}");
        }

        return Ok(result);
    }

    [HttpPost]
    [Route("update")]
    public async Task<ActionResult<AccountDto>> UpdateAccountAsync(UpdateAccountDto account)
    {
        try
        {
            var entity = await _accountService.UpdateAccountAsync(account);
            return Ok(entity);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
