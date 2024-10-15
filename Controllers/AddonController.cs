using Microsoft.AspNetCore.Mvc;
using MyWebAPI.DataTransferObject.ReturnDtos;
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
    [Route("{id}")]
    public async Task<ActionResult<AddonDto>> GetAddonByIdAsync(int id)
    {
        var result = await _addonService.GetAddonByIdAsync(id);
        if (result is null)
        {
            return BadRequest($"Addon not found with Id: {id}");
        }

        return Ok(result);
    }

    [HttpPost]
    [Route("create")]
    public async Task<ActionResult<AddonDto>> CreateAddonAsync(AddonDto addon)
    {
        try
        {
            var result = await _addonService.CreateAddonAsync(addon);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Route("update")]
    public async Task<ActionResult<AddonDto>> UpdateAddonAsync(AddonDto addon)
    {
        try
        {
            var entity = await _addonService.UpdateAddonAsync(addon);
            return Ok(entity);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult<AddonDto>> DeleteAddonAsync(int id)
    {
        try
        {
            var entity = await _addonService.DeleteAddonAsync(id);
            return Ok(entity);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
