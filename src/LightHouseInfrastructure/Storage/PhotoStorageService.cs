using System;
using LightHouseDomain.Interfaces;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace LightHouseInfrastructure.Storage;

public class PhotoStorageService : IPhotoStorageService
{
    private readonly IMinioClient _minioClient;
    private readonly string _bucketName;

    public PhotoStorageService(IOptions<MinioSettings> minioSettings)
    {
        var settings = minioSettings.Value;
        _bucketName = settings.BucketName;

        _minioClient = new MinioClient()
            .WithEndpoint(settings.EndPoint)
            .WithCredentials(settings.AccessKey, settings.SecretKey)
            .WithSSL(settings.UseSSL)
            .Build();
    }


    public Task<bool> DeletePhotoAsync(string filePath, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetFilePathAsync(Stream fileStream, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<string> SavePhotoAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default)
    {
        bool found = _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(_bucketName)).Result;

        if (!found)
        {
            await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(_bucketName));
        }

        fileStream.Position = 0;
        await _minioClient.PutObjectAsync(new PutObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(fileName)
            .WithStreamData(fileStream)
            .WithObjectSize(fileStream.Length)
            .WithContentType("image/jpeg"), cancellationToken);

        return $"{_bucketName}/{fileName}";
    }
}
