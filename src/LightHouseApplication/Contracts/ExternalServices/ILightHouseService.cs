
using LightHouseApplication.Common;
using LightHouseApplication.Dtos;

namespace LightHouseApplication.Contracts.ExternalServices;

public interface ILightHouseService
{
    Task<Result<IEnumerable<LightHouseDto>>> GetLightHousesAsync();
    Task<Result<LightHouseDto?>> GetLightHouseByIdAsync(Guid id);
    Task<Result<Guid>> CreateLightHouseAsync(LightHouseDto lightHouseDto);
    Task<Result<LightHouseDto>> UpdateLightHouseAsync(Guid id, LightHouseDto lightHouseDto);
    Task<Result<Guid>> DeleteLightHouseAsync(Guid id);
    Task<Result<IEnumerable<LightHouseTopDto>>> GetTopAsync(TopDto topDto);
}
