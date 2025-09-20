using LightHouseApplication.Common;
using LightHouseApplication.Common.Pipeline;
using LightHouseApplication.Contracts;
using LightHouseApplication.Dtos;
using LightHouseApplication.Features.Models;

namespace LightHouseApplication.Services;

internal class LightHouseService(PipelineDispatcher pipelineDispatcher) : ILightHouseService
{
    private readonly PipelineDispatcher _pipelineDispatcher = pipelineDispatcher;

    public async Task<Guid> CreateLightHouseAsync(LightHouseDto lightHouseDto)
    {
        var result = await _pipelineDispatcher.SendAsync<CreateLightHouseRequest, Result<Guid>>(new CreateLightHouseRequest(lightHouseDto));
        if (!result.IsSuccess)
        {
            throw new Exception(result.ErrorMessage);
        }
        return result.Data;
    }

    public async Task<Guid> DeleteLightHouseAsync(Guid id)
    {
        var result = await _pipelineDispatcher.SendAsync<DeleteLightHouseRequest, Result<Guid>>(new DeleteLightHouseRequest(id));

        if (!result.IsSuccess)
        {
            return Guid.Empty; // todo: add not found vs error distinction
        }

        return result.Data;
    }

    public async Task<LightHouseDto?> GetLightHouseByIdAsync(Guid id)
    {
        var result = await _pipelineDispatcher.SendAsync<GetLightHouseByIdRequest, Result<LightHouseDto?>>(new GetLightHouseByIdRequest(id));
        if (!result.IsSuccess)
        {
            return null; // todo: add not found vs error distinction
        }

        return result.Data;
    }

    public async Task<IEnumerable<LightHouseDto>> GetLightHousesAsync()
    {
        var result = await _pipelineDispatcher.SendAsync<GetAllLightHousesRequest, Result<IEnumerable<LightHouseDto>>>(new GetAllLightHousesRequest());

        return result.IsSuccess ? result.Data : throw new Exception(result.ErrorMessage);   
    }

    public Task<LightHouseDto> UpdateLightHouseAsync(Guid id, LightHouseDto lightHouseDto)
    {
        throw new NotImplementedException();
    }
}
