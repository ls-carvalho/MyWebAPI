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
    private readonly ILogger<AddonController> _logger;
    private readonly AppDbContext _context;
    private readonly IAddonService _addonService;

    public AddonController(ILogger<AddonController> logger, AppDbContext context, IAddonService addonService)
    {
        _logger = logger;
        _context = context;
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
            return NotFound("Addon not found");
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
            return NotFound("Request body invalid");
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
            return BadRequest("Request body invalid");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound("Addon not found for update");
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
            return NotFound("Addon not found for deletion");
        }

    }
}
