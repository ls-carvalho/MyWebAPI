using Microsoft.AspNetCore.Mvc;
using MyWebAPI.Context;
using MyWebAPI.DataTransferObject;
using MyWebAPI.Models;
using MyWebAPI.Services.Interfaces;

namespace MyWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AddonController : ControllerBase
{
    private readonly IAddonService _addonService;

    public AddonController(IAddonService addonService)
    {
        _addonService = addonService;
    }

    [HttpGet]
    [Route("all")]
    public async Task<ActionResult<IEnumerable<Addon>>> GetAllAddonsAsync()
    {
        var result = await _addonService.GetAllAddonsAsync();
        return Ok(result);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<Addon>> GetAddonByIdAsync(int id)
    {
        var result = await _addonService.GetAddonByIdAsync(id);
        if (result is null)
        {
            return NotFound($"Addon not found with Id: {id}");
        }

        return Ok(result);
    }

    [HttpPost]
    [Route("create")]
    public async Task<ActionResult<Addon>> CreateAddonAsync(CreateAddonDto addon)
    {
        try
        {
            var result = await _addonService.CreateAddonAsync(addon);
            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    [Route("update")]
    public async Task<ActionResult<Addon>> UpdateAddonAsync(UpdateAddonDto addon)
    {
        try
        {
            var entity = await _addonService.UpdateAddonAsync(addon);
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
    public async Task<ActionResult<Addon>> DeleteAddonAsync(int id)
    {
        try
        {
            var entity = await _addonService.DeleteAddonAsync(id);
            return Ok(entity);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }

    }
}
