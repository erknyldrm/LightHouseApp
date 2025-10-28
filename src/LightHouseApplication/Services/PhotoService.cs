using LightHouseApplication.Common;
using LightHouseApplication.Common.Pipeline;
using LightHouseApplication.Contracts;
using LightHouseApplication.Dtos;
using LightHouseApplication.Features.Photo.Models;
using LightHouseInfrastructure.Features.Photo;

namespace LightHouseApplication.Services;

public class PhotoService(PipelineDispatcher pipelineDispatcher) : IPhotoService
{

    public async Task DeletePhotoAsync(Guid id)
    {
        await pipelineDispatcher.SendAsync<DeletePhotoRequest, bool>(new DeletePhotoRequest(id));
    }

    public Task<PhotoDto?> GetPhotoByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<PhotoDto>> GetPhotosByLightHouseIdAsync(Guid lightHouseId)
    {
        throw new NotImplementedException();
    }

}
