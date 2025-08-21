using System;
using LightHouseDomain.Entities;

namespace LightHouseDomain.Interfaces;

public interface IPhotoRepository
{
    Task<Photo?> GetByIdAsync(Guid id);
    Task<IEnumerable<Photo>> GetAllAsync();
    Task AddAsync(Photo photo);
    Task UpdateAsync(Photo photo);
    Task DeleteAsync(Guid id);

    Task<IEnumerable<Photo>> GetPhotosByLightHouseIdAsync(Guid lightHouseId);

    Task<IEnumerable<Photo>> GetByUserIdAsync(Guid userId);
}
