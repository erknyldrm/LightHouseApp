using System;
using LightHouseApplication.Common;
using LightHouseDomain.Entities;
using LightHouseDomain.ValueObjects;

namespace LightHouseApplication.Contracts.Repositories;

public interface ILightHouseRepository
{
    
    Task<Result<LightHouse>> GetByIdAsync(Guid id);
    Task<Result<IEnumerable<LightHouse>>> GetAllAsync();
    Task<Result> AddAsync(LightHouse entity);
    Task<Result> UpdateAsync(LightHouse entity);
    Task<Result> DeleteAsync(Guid id);

    Task<Result<IEnumerable<LightHouseWithStats>>> GetTopAsync(int count);  

}
