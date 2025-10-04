using LightHouseApplication.Dtos;

namespace LightHouseApplication.Contracts.Repositories;

public interface ILightHouseODataRepository
{
    IQueryable<QueryableLightHouseDto> GetLightHouses();   
}
