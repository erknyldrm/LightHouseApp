using System;
using LightHouseApplication.Common;
using LightHouseDomain.Entities;
using LightHouseDomain.ValueObjects;

namespace LightHouseApplication.Contracts.Repositories;

public interface ILightHouseRepository
{
    
    Task<LightHouse?> GetByIdAsync(int id);
    Task<IEnumerable<LightHouse>> GetAllAsync();
    Task<Result> AddAsync(LightHouse entity);
    Task UpdateAsync(LightHouse entity);
    Task DeleteAsync(int id);
    Task<IEnumerable<LightHouseWithStats>> GetTopAsync(int count);

}
