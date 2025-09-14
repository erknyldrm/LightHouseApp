using System;
using LightHouseDomain.Interfaces;
using LightHouseInfrastructure.Auditors;
using LightHouseInfrastructure.Caching;
using LightHouseInfrastructure.Configuration;
using LightHouseInfrastructure.SecretManager;
using LightHouseInfrastructure.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Minio;

namespace LightHouseInfrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
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
                .WithSSL(config.UseSSL)
                .Build();
        });

        var useRedis = configuration.GetValue<bool>("Caching:UseRedis");
        if (useRedis)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetValue<string>("Redis:Configuration");
                options.InstanceName = configuration.GetValue<string>("Redis:InstanceName") ?? "LightHouseApp:";
            });
            services.AddSingleton<ICacheService, RedisCacheService>();
        }
        else
        {
            services.AddMemoryCache();
            services.AddSingleton<ICacheService, MemoryCacheService>();
        }

        return services;
    }

}
