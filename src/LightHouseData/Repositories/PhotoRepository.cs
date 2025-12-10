using System;
using LightHouseApplication.Common;
using LightHouseApplication.Contracts.Repositories;
using LightHouseDomain.Entities;


namespace LightHouseData.Repositories;

public class PhotoRepository : IPhotoRepository
{
    public Task<Result> AddAsync(Photo photo)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<Photo>>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Result<Photo>> GetByIdAsync(Guid id)
    {
        return Result<Photo>.Ok(new Photo(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "sample.jpg",
            new LightHouseDomain.ValueObjects.PhotoMetadata("50mm", "2048x1536", "Nikon", DateTime.UtcNow.AddMonths(-6))
        ));
    }

    public Task<Result<IEnumerable<Photo>>> GetByUserIdAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<Photo>>> GetPhotosByLightHouseIdAsync(Guid lightHouseId)
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdateAsync(Photo photo)
    {
        throw new NotImplementedException();
    }
}
