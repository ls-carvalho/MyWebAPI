using Microsoft.AspNetCore.Mvc;
using MyWebAPI.Context;
using MyWebAPI.DataTransferObject;
using MyWebAPI.Models;
using MyWebAPI.Services.Interfaces;

namespace MyWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [Route("all")]
    public async Task<ActionResult<IEnumerable<User>>> GetAllUsersAsync()
    {
        var result = await _userService.GetAllUsersAsync();
        return Ok(result);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<User>> GetUserByIdAsync(int id)
    {
        var result = await _userService.GetUserByIdAsync(id);
        if (result is null)
        {
            return NotFound($"User not found with Id: {id}");
        }

        return Ok(result);
    }

    [HttpPost]
    [Route("create")]
    public async Task<ActionResult<User>> CreateUserAsync(CreateUserDto user)
    {
        try
        {
            var result = await _userService.CreateUserAsync(user);
            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    [Route("update")]
    public async Task<ActionResult<User>> UpdateUserAsync(UpdateUserDto user)
    {
        try
        {
            var entity = await _userService.UpdateUserAsync(user);
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
    public async Task<ActionResult<User>> DeleteUserAsync(int id)
    {
        try
        {
            var entity = await _userService.DeleteUserAsync(id);
            return Ok(entity);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }

    }
}
