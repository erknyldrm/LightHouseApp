using System;
using LightHouseApplication.Contracts;
using LightHouseApplication.Dtos;
using LightHouseInfrastructure.Features.LightHouse;

namespace LightHouseApplication.Services;

internal class LightHouseService(CreateLightHouseHandler createLightHouseHandler, GetLightHousesHandler getLightHousesHandler) : ILightHouseService
{
    private readonly CreateLightHouseHandler _createLightHouseHandler = createLightHouseHandler;

    private readonly GetLightHousesHandler _getLightHousesHandler = getLightHousesHandler;

    public async Task<Guid> CreateLightHouseAsync(LightHouseDto lightHouseDto)
    {
        var result = await _createLightHouseHandler.HandleAsync(new Features.Models.CreateLightHouseRequest(lightHouseDto));
        if (!result.IsSuccess)
        {
            throw new Exception(result.ErrorMessage);
        }
        return result.Data;
    }

    public Task DeleteLightHouseAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<LightHouseDto?> GetLightHouseByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<LightHouseDto>> GetLightHousesAsync()
    {
        var result = await _getLightHousesHandler.HandleAsync();

        return result.IsSuccess ? result.Data : throw new Exception(result.ErrorMessage);   
    }

    public Task<LightHouseDto> UpdateLightHouseAsync(Guid id, LightHouseDto lightHouseDto)
    {
        throw new NotImplementedException();
    }
}
