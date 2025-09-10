using FluentValidation;
using LightHouseApplication.Common;
using LightHouseApplication.Contracts;
using LightHouseApplication.Dtos;
using LightHouseDomain.Countries;
using LightHouseDomain.Interfaces;
using LightHouseDomain.ValueObjects;

namespace LightHouseInfrastructure.Features.LightHouse;

public class CreateLightHouseHandler(ILightHouseRepository lightHouseRepository, ICountryDataReader countryDataReader, IValidator<LightHouseDto> validator)
   
{
    private readonly ILightHouseRepository _lightHouseRepository = lightHouseRepository;
    private readonly ICountryDataReader _countryDataReader = countryDataReader;

    private readonly IValidator<LightHouseDto> _validator = validator;

    public async Task<Result<Guid>> HandleAsync(LightHouseDto lightHouseDto)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(lightHouseDto);
            
            if (!validationResult.IsValid)
            {
                var errorMessages = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));

                return Result<Guid>.Fail(errorMessages);
            }


            var country = await  _countryDataReader.GetCountryByIdAsync(lightHouseDto.CountryId);
            if (country is null)
                return Result<Guid>.Fail("Country not found.");

            var location = new Coordinates(lightHouseDto.Latitude, lightHouseDto.Longitude);

            var lightHouse = new LightHouseDomain.Entities.LightHouse(
                lightHouseDto.Name,
                country,
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
