using FluentValidation;
using LightHouseApplication.Common;
using LightHouseApplication.Common.Pipeline;
using LightHouseApplication.Contracts;
using LightHouseApplication.Contracts.Repositories;
using LightHouseApplication.Dtos;
using LightHouseApplication.Features.Models;
using LightHouseDomain.Common;
using LightHouseDomain.Events;
using LightHouseDomain.ValueObjects;

namespace LightHouseApplication.Features.LightHouse;

internal class CreateLightHouseHandler(ILightHouseRepository lightHouseRepository, ICountryDataReader countryDataReader,
                                       IValidator<LightHouseDto> validator, IEventPublisher eventPublisher)
    : IHandler<CreateLightHouseRequest, Result<Guid>>
{
    private readonly ILightHouseRepository _lightHouseRepository = lightHouseRepository;
    private readonly ICountryDataReader _countryDataReader = countryDataReader;

    private readonly IValidator<LightHouseDto> _validator = validator;

    public async Task<Result<Guid>> HandleAsync(CreateLightHouseRequest request, CancellationToken cancellationToken = default)
    {
        var creationRequestedEvent = new LightHouseCreationRequested(
            request.LightHouse.Id,
            request.LightHouse.Name,
            request.LightHouse.CountryId,
            request.LightHouse.Latitude,
            request.LightHouse.Longitude
        );

        await eventPublisher.PublishAsync(creationRequestedEvent, cancellationToken);

        var validationResult = await _validator.ValidateAsync(request.LightHouse);

        if (!validationResult.IsValid)
        {
            var errorMessages = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));

            var failureEvent = new LightHouseCreationFailed(
                request.LightHouse.Name,
                request.LightHouse.CountryId,
                request.LightHouse.Latitude,
                request.LightHouse.Longitude,
                "Validation failed",
                errorMessages,
                Guid.Empty
            );

            await eventPublisher.PublishAsync(failureEvent, cancellationToken);

            return Result<Guid>.Fail(errorMessages);
        }


        var result = await _countryDataReader.GetCountryByIdAsync(request.LightHouse.CountryId);

        if (!result.IsSuccess)
        {

            var failureEvent = new LightHouseCreationFailed(
                request.LightHouse.Name,
                request.LightHouse.CountryId,
                request.LightHouse.Latitude,
                request.LightHouse.Longitude,
                "Country retrieval failed",
                result.ErrorMessage!,
                Guid.Empty
            );

            await eventPublisher.PublishAsync(failureEvent, cancellationToken);

            return Result<Guid>.Fail(result.ErrorMessage!);
        }

        var location = new Coordinates(request.LightHouse.Latitude, request.LightHouse.Longitude);

        var lightHouse = new LightHouseDomain.Entities.LightHouse(
            request.LightHouse.Name,
            result.Data,
            location);

        var addedResult = await _lightHouseRepository.AddAsync(lightHouse);

        if (!addedResult.IsSuccess)
        {
            var failureEvent = new LightHouseCreationFailed(
                request.LightHouse.Name,
                request.LightHouse.CountryId,
                request.LightHouse.Latitude,
                request.LightHouse.Longitude,
                "LightHouse persistence failed",
                addedResult.ErrorMessage!,
                Guid.Empty
            );

            await eventPublisher.PublishAsync(failureEvent, cancellationToken);

            return Result<Guid>.Fail(addedResult.ErrorMessage!);
        }

        var creationSucceededEvent = new LightHouseCreated(
            request.LightHouse.Id,
            request.LightHouse.Name,
            request.LightHouse.CountryId,
            request.LightHouse.Latitude,
            request.LightHouse.Longitude);

        await eventPublisher.PublishAsync(creationSucceededEvent, cancellationToken);

        return Result<Guid>.Ok(lightHouse.Id);
    }
}
