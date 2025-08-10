
using LightHouseApplication.Dtos;

namespace LightHouseApplication.Contracts;


public interface ILightHouseService
{
    Task<IEnumerable<LightHouseDto>> GetAllLightHouseNamesAsync();
    Task<LightHouseDto?> GetLightHouseByIdAsync(Guid id);
    Task<LightHouseDto> CreateLightHouseAsync(LightHouseDto lightHouseDto);
    Task<LightHouseDto> UpdateLightHouseAsync(Guid id, LightHouseDto lightHouseDto);
    Task DeleteLightHouseAsync(Guid id);
}

