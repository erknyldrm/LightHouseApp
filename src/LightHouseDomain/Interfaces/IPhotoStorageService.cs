using System;

namespace LightHouseDomain.Interfaces;

public interface IPhotoStorageService
{
    Task<string> GetFilePathAsync(Stream fileStream, CancellationToken cancellationToken = default);
    Task<string> SavePhotoAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default);
    Task<bool> DeletePhotoAsync(string filePath, CancellationToken cancellationToken = default);

}

