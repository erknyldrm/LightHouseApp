using FluentValidation;
using LightHouseApplication.Common;
using LightHouseApplication.Common.Pipeline;
using LightHouseApplication.Contracts;
using LightHouseApplication.Contracts.Repositories;
using LightHouseApplication.Dtos;
using LightHouseApplication.Features.Models;
using LightHouseDomain.ValueObjects;

namespace LightHouseApplication.Features.LightHouse;

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


            var result = await _countryDataReader.GetCountryByIdAsync(request.LightHouse.CountryId);

            if (!result.IsSuccess)
            {
                return Result<Guid>.Fail(result.ErrorMessage);
            }

            var location = new Coordinates(request.LightHouse.Latitude, request.LightHouse.Longitude);

            var lightHouse = new LightHouseDomain.Entities.LightHouse(
                request.LightHouse.Name,
                result.Data,
                location);

            var added = await _lightHouseRepository.AddAsync(lightHouse);
            
            if (!added.IsSuccess)
            {
                return Result<Guid>.Fail(added.ErrorMessage);
            }

            return Result<Guid>.Ok(lightHouse.Id);
        }
        catch (Exception ex)
        {
            return Result<Guid>.Fail($"An error occurred: {ex.Message}");
        }
    }
}
