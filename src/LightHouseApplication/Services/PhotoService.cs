using LightHouseApplication.Common;
using LightHouseApplication.Common.Pipeline;
using LightHouseApplication.Contracts;
using LightHouseApplication.Contracts.ExternalServices;
using LightHouseApplication.Dtos;
using LightHouseApplication.Features.Photo;
using LightHouseApplication.Features.Photo.Models;


namespace LightHouseApplication.Services;

public class PhotoService(PipelineDispatcher pipelineDispatcher) : IPhotoService
{

    public async Task<Result> DeletePhotoAsync(Guid id)
    {
       var request = new DeletePhotoRequest(id);
        return await pipelineDispatcher.SendAsync<DeletePhotoRequest, Result>(request);
    }

    public Task<Result<PhotoDto>> GetPhotoByIdAsync(Guid id)
    {
        var request = new GetPhotoByIdRequest(id);
        return pipelineDispatcher.SendAsync<GetPhotoByIdRequest, Result<PhotoDto>>(request);    
    }

    public Task<Result<IEnumerable<PhotoDto>>> GetPhotosByLightHouseIdAsync(Guid lightHouseId)
    {
        var request = new GetPhotosByLightHouseRequest(lightHouseId);
        return pipelineDispatcher.SendAsync<GetPhotosByLightHouseRequest, Result<IEnumerable<PhotoDto>>>(request);  
    }

    public async Task<Result<Stream>> GetRawPhotoAsync(string fileName)
    {
      var request = new GetRawPhotoRequest(fileName);
        return await pipelineDispatcher.SendAsync<GetRawPhotoRequest, Result<Stream>>(request);
    }
}
