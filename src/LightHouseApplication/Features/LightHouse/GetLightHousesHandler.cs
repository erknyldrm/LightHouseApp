using System;
using LightHouseApplication.Common;
using LightHouseApplication.Dtos;
using LightHouseDomain.Interfaces;

namespace LightHouseApplication.Features.LightHouse;

public class GetLightHousesHandler(ILightHouseRepository lightHouseRepository)
{
    private readonly ILightHouseRepository _lightHouseRepository = lightHouseRepository;

    public async Task<Result<IEnumerable<LightHouseDto>>> HandleAsync()
    {
        try
        {
            var lightHouses = await _lightHouseRepository.GetAllAsync();

            if (lightHouses == null || !lightHouses.Any())
            {
                return Result<IEnumerable<LightHouseDto>>.Fail("No record found.");
            }

            var lightHouseDtos = lightHouses.Select(lh => new LightHouseDto(lh.Id, lh.Name, lh.CountryId, lh.Location.Latitude, lh.Location.Longitude));

            return Result<IEnumerable<LightHouseDto>>.Ok(lightHouseDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<LightHouseDto>>.Fail($"An error occurred: {ex.Message}");
        }
    }
}
