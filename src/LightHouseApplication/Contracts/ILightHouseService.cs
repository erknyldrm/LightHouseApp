
using LightHouseApplication.Dtos;

namespace LightHouseApplication.Contracts;


public interface ILightHouseService
{
    Task<IEnumerable<LightHouseDto>> GetLightHousesAsync();
    Task<LightHouseDto?> GetLightHouseByIdAsync(Guid id);
    Task<Guid> CreateLightHouseAsync(LightHouseDto lightHouseDto);
    Task<LightHouseDto> UpdateLightHouseAsync(Guid id, LightHouseDto lightHouseDto);
    Task<Guid> DeleteLightHouseAsync(Guid id);
}

