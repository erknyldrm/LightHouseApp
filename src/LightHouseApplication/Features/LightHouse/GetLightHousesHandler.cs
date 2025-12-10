using System;
using LightHouseApplication.Common;
using LightHouseApplication.Contracts.Repositories;
using LightHouseApplication.Dtos;

namespace LightHouseApplication.Features.LightHouse;

public class GetLightHousesHandler(ILightHouseRepository lightHouseRepository)
{
    private readonly ILightHouseRepository _lightHouseRepository = lightHouseRepository;

    public async Task<Result<IEnumerable<LightHouseDto>>> HandleAsync()
    {
        try
        {
            var lightHousesResult = await _lightHouseRepository.GetAllAsync();

            if (!lightHousesResult.IsSuccess)
            {
                return Result<IEnumerable<LightHouseDto>>.Fail(lightHousesResult.ErrorMessage!);
            }

            var lightHouses = lightHousesResult.Data;

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
