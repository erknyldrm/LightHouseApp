using System;
using LightHouseDomain.Interfaces;
using LightHouseInfrastructure.Auditors;
using LightHouseInfrastructure.Configuration;
using LightHouseInfrastructure.SecretManager;
using LightHouseInfrastructure.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Minio;

namespace LightHouseInfrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IPhotoStorageService, PhotoStorageService>();
        services.AddScoped<ICommentAuditor, ExternalCommentAuditor>();

        services.AddScoped<ISecretManager, VaultSecretManager>();
        services.AddScoped<VaultConfigurationService>();

        services.AddScoped(provider => 
        {
            var config = provider.GetRequiredService<IOptions<MinioSettings>>().Value;
        
            return new MinioClient()
                .WithEndpoint(config.EndPoint)
                .WithCredentials(config.AccessKey, config.SecretKey)
                .Build();
        }); 
        return services;
    }

}
