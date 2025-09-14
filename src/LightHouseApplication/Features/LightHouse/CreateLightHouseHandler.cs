using FluentValidation;
using LightHouseApplication.Common;
using LightHouseApplication.Common.Pipeline;
using LightHouseApplication.Contracts;
using LightHouseApplication.Dtos;
using LightHouseApplication.Features.Models;
using LightHouseDomain.Countries;
using LightHouseDomain.Interfaces;
using LightHouseDomain.ValueObjects;

namespace LightHouseInfrastructure.Features.LightHouse;

internal class CreateLightHouseHandler(ILightHouseRepository lightHouseRepository, ICountryDataReader countryDataReader, IValidator<LightHouseDto> validator)
    : IHandler<CreateLightHouseRequest, Result<Guid>>   
{
    private readonly ILightHouseRepository _lightHouseRepository = lightHouseRepository;
    private readonly ICountryDataReader _countryDataReader = countryDataReader;

    private readonly IValidator<LightHouseDto> _validator = validator;

   public async Task<Result<Guid>> HandleAsync(CreateLightHouseRequest request, CancellationToken cancellationToken = default)
    {
       try
        {
            var validationResult = await _validator.ValidateAsync(request.LightHouse);
            
            if (!validationResult.IsValid)
            {
                var errorMessages = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));

                return Result<Guid>.Fail(errorMessages);
            }


            var country = await  _countryDataReader.GetCountryByIdAsync(request.LightHouse.CountryId);
            if (country is null)
                return Result<Guid>.Fail("Country not found.");

            var location = new Coordinates(request.LightHouse.Latitude, request.LightHouse.Longitude);

            var lightHouse = new LightHouseDomain.Entities.LightHouse(
                request.LightHouse.Name,
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
