using LightHouseApplication.Common;
using LightHouseApplication.Common.Pipeline;
using LightHouseApplication.Contracts;
using LightHouseApplication.Dtos;
using LightHouseApplication.Features.LightHouse;
using LightHouseApplication.Features;
using LightHouseApplication.Contracts.ExternalServices;

namespace LightHouseApplication.Services;

internal class LightHouseService(PipelineDispatcher pipelineDispatcher) : ILightHouseService
{
    private readonly PipelineDispatcher _pipelineDispatcher = pipelineDispatcher;

    public async Task<Result<Guid>> CreateLightHouseAsync(LightHouseDto lightHouseDto)
    {
        return await _pipelineDispatcher.SendAsync<CreateLightHouseRequest, Result<Guid>>(new CreateLightHouseRequest(lightHouseDto));
    }

    public async Task<Result<Guid>> DeleteLightHouseAsync(Guid id)
    {
        return await _pipelineDispatcher.SendAsync<DeleteLightHouseRequest, Result<Guid>>(new DeleteLightHouseRequest(id));
    }

    public async Task<Result<LightHouseDto?>> GetLightHouseByIdAsync(Guid id)
    {
        return await _pipelineDispatcher.SendAsync<GetLightHouseByIdRequest, Result<LightHouseDto?>>(new GetLightHouseByIdRequest(id));

    }

    public async Task<Result<IEnumerable<LightHouseDto>>> GetLightHousesAsync()
    {
        return await _pipelineDispatcher.SendAsync<GetAllLightHousesRequest, Result<IEnumerable<LightHouseDto>>>(new GetAllLightHousesRequest());

    }

    public async Task<Result<IEnumerable<LightHouseTopDto>>> GetTopAsync(TopDto topDto)
    {
        return await _pipelineDispatcher.SendAsync<GetTopLightHousesRequest, Result<IEnumerable<LightHouseTopDto>>>(new GetTopLightHousesRequest(topDto.Count));
    }

    public Task<LightHouseDto> UpdateLightHouseAsync(Guid id, LightHouseDto lightHouseDto)
    {
        throw new NotImplementedException();
    }

    Task<Result<LightHouseDto>> ILightHouseService.UpdateLightHouseAsync(Guid id, LightHouseDto lightHouseDto)
    {
        throw new NotImplementedException();
    }
}
