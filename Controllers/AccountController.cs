using Microsoft.AspNetCore.Mvc;
using MyWebAPI.Context;
using MyWebAPI.DataTransferObject.Account;
using MyWebAPI.Models;
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
    public async Task<ActionResult<IEnumerable<Account>>> GetAllAccountsAsync()
    {
        var result = await _accountService.GetAllAccountsAsync();
        return Ok(result);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<Account>> GetAccountByIdAsync(int id)
    {
        var result = await _accountService.GetAccountByIdAsync(id);
        if (result is null)
        {
            return NotFound($"Account not found with Id: {id}");
        }

        return Ok(result);
    }

    [HttpPost]
    [Route("create")]
    public async Task<ActionResult<Account>> CreateAccountAsync(CreateAccountDto account)
    {
        try
        {
            var result = await _accountService.CreateAccountAsync(account);
            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    [Route("update")]
    public async Task<ActionResult<Account>> UpdateAccountAsync(UpdateAccountDto account)
    {
        try
        {
            var entity = await _accountService.UpdateAccountAsync(account);
            return Ok(entity);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult<Account>> DeleteAccountAsync(int id)
    {
        try
        {
            var entity = await _accountService.DeleteAccountAsync(id);
            return Ok(entity);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }

    }
}
