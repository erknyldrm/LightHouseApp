using Microsoft.AspNetCore.Mvc;
using LightHouseApplication.Contracts;
using LightHouseApplication.Dtos;

namespace LightHouseWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LightHouseController : ControllerBase
{
    private readonly ILightHouseService _lightHouseService;

    public LightHouseController(ILightHouseService lightHouseService)
    {
        _lightHouseService = lightHouseService;
    }

    [HttpGet]
    public async Task<IActionResult> GetLightHouses()
    {
        try
        {
            var result = await _lightHouseService.GetLightHousesAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateLightHouse([FromBody] LightHouseDto lightHouseDto)
    {
        try
        {
            var result = await _lightHouseService.CreateLightHouseAsync(lightHouseDto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
