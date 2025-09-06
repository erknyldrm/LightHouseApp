using LightHouseApplication.Dtos;

namespace LightHouseApplication.Contracts;


public interface IPhotoService
{
    Task<IEnumerable<PhotoDto>> GetPhotosByLightHouseIdAsync(Guid lightHouseId);
    Task<PhotoDto?> GetPhotoByIdAsync(Guid id);
    Task<Guid> UploadPhotoAsync(PhotoDto photo, Stream fileContent);
    Task DeletePhotoAsync(Guid id);
}
