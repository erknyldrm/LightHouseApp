using System;
using LightHouseDomain.Interfaces;
using LightHouseInfrastructure.Configuration;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace LightHouseInfrastructure.Storage;

public class PhotoStorageService : IPhotoStorageService
{
    private readonly IMinioClient _minioClient;
    private readonly string _bucketName;

    public PhotoStorageService(IOptions<MinioSettings> minioSettings, VaultConfigurationService vaultConfigurationService)
    {
        var (accessKey, secretKey) = vaultConfigurationService.GetMinioCredentialAsync().GetAwaiter().GetResult();

        var settings = minioSettings.Value;
        _bucketName = settings.BucketName;

        _minioClient = new MinioClient()
            .WithEndpoint(settings.EndPoint)
            .WithCredentials(accessKey, secretKey)
            .WithSSL(settings.UseSSL)
            .Build();
    }


    public async Task DeletePhotoAsync(string filePath, CancellationToken cancellationToken = default)
    {
        await _minioClient.RemoveObjectAsync(new RemoveObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(filePath), cancellationToken);
    }

    public async Task<Stream> GetAsync(string filePath, CancellationToken cancellationToken = default)
    {
        var memoryStream = new MemoryStream();
        await _minioClient.GetObjectAsync(new GetObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(filePath)
            .WithCallbackStream(stream =>
            {
                stream.CopyTo(memoryStream);
            }), cancellationToken);

        memoryStream.Position = 0;
        return memoryStream;
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
