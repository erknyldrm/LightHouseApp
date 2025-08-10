using System;

namespace LightHouseDomain.Interfaces;

using System.Collections.Generic;
using System.Threading.Tasks;
using LightHouseDomain.Entities;

public interface ILightHouseRepository
{
    Task<LightHouse?> GetByIdAsync(int id);
    Task<IEnumerable<LightHouse>> GetAllAsync();
    Task AddAsync(LightHouse entity);
    Task UpdateAsync(LightHouse entity);
    Task DeleteAsync(int id);
}
