using LightHouseApplication.Common;
using LightHouseApplication.Dtos;

namespace LightHouseApplication.Contracts.ExternalServices;


public interface IPhotoService
{
    Task<Result<IEnumerable<PhotoDto>>> GetPhotosByLightHouseIdAsync(Guid lightHouseId);
    Task<Result<PhotoDto>> GetPhotoByIdAsync(Guid id);
    Task<Result> DeletePhotoAsync(Guid id);
    Task<Result<Stream>> GetRawPhotoAsync(string fileName);

}
