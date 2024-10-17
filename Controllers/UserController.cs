using Microsoft.AspNetCore.Mvc;
using MyWebAPI.DataTransferObject;
using MyWebAPI.DataTransferObject.ReturnDtos;
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
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsersAsync()
    {
        var result = await _userService.GetAllUsersAsync();
        return Ok(result);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<UserDto>> GetUserByIdAsync(int id)
    {
        var result = await _userService.GetUserByIdAsync(id);
        if (result is null)
        {
            return BadRequest($"User not found with Id: {id}");
        }

        return Ok(result);
    }

    [HttpPost]
    [Route("create")]
    public async Task<ActionResult<UserDto>> CreateUserAsync(CreateUserDto user)
    {
        try
        {
            var result = await _userService.CreateUserAsync(user);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Route("update")]
    public async Task<ActionResult<UserDto>> UpdateUserAsync(UpdateUserDto user)
    {
        try
        {
            var entity = await _userService.UpdateUserAsync(user);
            return Ok(entity);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult<UserDto>> DeleteUserAsync(int id)
    {
        try
        {
            var entity = await _userService.DeleteUserAsync(id);
            return Ok(entity);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }
}
