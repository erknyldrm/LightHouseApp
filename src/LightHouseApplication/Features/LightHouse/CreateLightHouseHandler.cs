using LightHouseApplication.Common;
using LightHouseApplication.Dtos;
using LightHouseDomain.Countries;
using LightHouseDomain.Interfaces;
using LightHouseDomain.ValueObjects;

namespace LightHouseApplication.Features.LightHouse;

public class CreateLightHouseHandler(ILightHouseRepository lightHouseRepository, ICountryRegistry countryRegistry)
{
    private readonly ILightHouseRepository _lightHouseRepository = lightHouseRepository;
    private readonly ICountryRegistry _countryRegistry = countryRegistry;

    public async Task<Result<Guid>> HandleAsync(LightHouseDto lightHouseDto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(lightHouseDto.Name))
                return Result<Guid>.Fail("LightHouse name cannot be empty.");

            var country = _countryRegistry.GetCountryById(lightHouseDto.CountryId);
            if (country is null)
                return Result<Guid>.Fail("Country not found.");

            var location = new Coordinates(lightHouseDto.Latitude, lightHouseDto.Longitude);

            var lightHouse = new LightHouseDomain.Entities.LightHouse(
                lightHouseDto.Name,
                country.Name,
                location);

            await _lightHouseRepository.AddAsync(lightHouse);

            return Result<Guid>.Ok(lightHouse.Id);
        }
        catch (Exception ex)
        {
            return Result<Guid>.Fail($"An error occurred: {ex.Message}");
        }
    }
}
