using LightHouseApplication.Common;
using LightHouseApplication.Dtos;

namespace LightHouseApplication.Contracts.Repositories;

public interface ILightHouseODataRepository
{
    IQueryable<Result<QueryableLightHouseDto>> GetLightHouses();   
}
