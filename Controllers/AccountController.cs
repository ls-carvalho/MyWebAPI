using Microsoft.AspNetCore.Mvc;
using MyWebAPI.Context;
using MyWebAPI.DataTransferObject;
using MyWebAPI.Models;
using MyWebAPI.Services.Interfaces;

namespace MyWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    private readonly AppDbContext _context;
    private readonly IAccountService _accountService;

    public AccountController(ILogger<AccountController> logger, AppDbContext context, IAccountService accountService)
    {
        _logger = logger;
        _context = context;
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
            return NotFound("Account not found");
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
            return NotFound("Request body invalid");
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
            return BadRequest("Request body invalid");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound("Account not found for update");
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
            return NotFound("Account not found for deletion");
        }

    }
}
