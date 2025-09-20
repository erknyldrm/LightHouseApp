
using LightHouseApplication.Dtos;
using LightHouseApplication.Features.Models;

namespace LightHouseApplication.Contracts;


internal interface ILightHouseService
{
    Task<IEnumerable<GetAllLightHousesRequest>> GetLightHousesAsync();
    Task<LightHouseDto?> GetLightHouseByIdAsync(Guid id);
    Task<Guid> CreateLightHouseAsync(LightHouseDto lightHouseDto);
    Task<LightHouseDto> UpdateLightHouseAsync(Guid id, LightHouseDto lightHouseDto);
    Task<Guid> DeleteLightHouseAsync(Guid id);
}

