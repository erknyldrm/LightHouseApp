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
    public static InfrastructureBuilder AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        return new InfrastructureBuilder(services, configuration);
    }
}

public class InfrastructureBuilder(IServiceCollection services, IConfiguration configuration)
{
    public InfrastructureBuilder WithSecretVault()
    {
        services.AddSingleton<ISecretManager, VaultSecretManager>();
        services.AddSingleton<VaultConfigurationService>();
        return this;
    }

    public InfrastructureBuilder WithPhotoStorage()
    {
        services.AddScoped<IPhotoStorageService, PhotoStorageService>();

        services.AddSingleton(provider =>
        {
            var config = provider.GetRequiredService<IOptions<MinioSettings>>().Value;

            return new MinioClient()
                .WithEndpoint(config.EndPoint)
                .WithCredentials(config.AccessKey, config.SecretKey)
                .WithSSL(config.UseSSL)
                .Build();
        });

        return this;
    }

    public InfrastructureBuilder WithCaching()
    {
        var useRedis = configuration.GetValue<bool>("Caching:UseRedis");
        return WithCaching(useRedis);
    }

    public InfrastructureBuilder WithCaching(bool useRedis = false)
    {
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

        return this;
    }

    public InfrastructureBuilder WithExternals()
    {
        services.AddScoped<ICommentAuditor, ExternalCommentAuditor>();
        return this;
    }
}
