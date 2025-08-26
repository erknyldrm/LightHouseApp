using System;
using LightHouseDomain.Entities;
using LightHouseDomain.Interfaces;

namespace LightHouseData;

public class PhotoRepository : IPhotoRepository
{
    public Task AddAsync(Photo photo)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Photo>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Photo?> GetByIdAsync(Guid id)
    {
        return new Photo(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "test.jpg",
            new LightHouseDomain.ValueObjects.PhotoMetadata ("40mm","1920, 1080", "Canon", DateTime.UtcNow.AddYears(-1)));   
    }

    public Task<IEnumerable<Photo>> GetByUserIdAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Photo>> GetPhotosByLightHouseIdAsync(Guid lightHouseId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Photo photo)
    {
        throw new NotImplementedException();
    }
}
