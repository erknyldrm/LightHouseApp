using System;
using LightHouseApplication.Common;
using LightHouseDomain.Entities;

namespace LightHouseApplication.Contracts.Repositories;

public interface IPhotoRepository
{
    Task<Result<Photo>> GetByIdAsync(Guid id);
    Task<Result<IEnumerable<Photo>>> GetAllAsync();
    Task<Result> AddAsync(Photo photo);
    Task<Result> UpdateAsync(Photo photo);
    Task<Result> DeleteAsync(Guid id);

    Task<Result<IEnumerable<Photo>>> GetPhotosByLightHouseIdAsync(Guid lightHouseId);

    Task<Result<IEnumerable<Photo>>> GetByUserIdAsync(Guid userId);
}
