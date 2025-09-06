using LightHouseApplication.Contracts;
using LightHouseApplication.Dtos;
using LightHouseInfrastructure.Features.Photo;

namespace LightHouseApplication.Services;

public class PhotoService(UploadPhotoHandler uploadPhotoHandler) : IPhotoService
{
    private readonly UploadPhotoHandler _uploadPhotoHandler = uploadPhotoHandler;

    public Task DeletePhotoAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<PhotoDto?> GetPhotoByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<PhotoDto>> GetPhotosByLightHouseIdAsync(Guid lightHouseId)
    {
        throw new NotImplementedException();
    }

    public async Task<Guid> UploadPhotoAsync(PhotoDto photo, Stream fileContent)
    {
        var result = await _uploadPhotoHandler.HandleAsync(photo, fileContent);

        return result.IsSuccess 
            ? result.Data 
            : throw new Exception(result.ErrorMessage); 
    }
}
