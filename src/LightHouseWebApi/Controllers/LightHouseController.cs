using Microsoft.AspNetCore.Mvc;
using LightHouseApplication.Contracts;
using LightHouseApplication.Dtos;

namespace LightHouseWebApi.Controllers;

public record CreateLightHouseRequest(LightHouseDto LightHouseDto);

[ApiController]
[Route("api/[controller]")]
public class LightHouseController : ControllerBase
{
    private readonly ILogger<LightHouseController> _logger;
    private readonly ILightHouseService _lightHouseService;

    public LightHouseController(ILogger<LightHouseController> logger, ILightHouseService lightHouseService)
    {
        _logger = logger;
        _lightHouseService = lightHouseService;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        try
        {
            var lightHouse = await _lightHouseService.GetLightHouseByIdAsync(id);
            if (lightHouse == null)
                return NotFound();

            return Ok(lightHouse);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting lighthouse by id: {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateLightHouseRequest request)
    {
        try
        {
            var newId = await _lightHouseService.CreateLightHouseAsync(request.LightHouseDto);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = newId }, newId );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating lighthouse");
            return StatusCode(500, "Internal server error");
        }
    }
}
