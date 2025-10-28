using LightHouseApplication.Dtos;

namespace LightHouseApplication.Contracts;


public interface IPhotoService
{
    Task<IEnumerable<PhotoDto>> GetPhotosByLightHouseIdAsync(Guid lightHouseId);
    Task<PhotoDto?> GetPhotoByIdAsync(Guid id);
    Task DeletePhotoAsync(Guid id);
}
