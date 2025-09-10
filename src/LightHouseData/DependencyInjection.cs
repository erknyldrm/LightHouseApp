using System;
using LightHouseApplication.Contracts;
using LightHouseData.Repositories;
using LightHouseDomain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.Replication;

namespace LightHouseData;

public class DependencyInjection
{

    public static IServiceCollection AddLightHouseDataServices(IServiceCollection services, string connectionString)
    {
        ArgumentNullException.ThrowIfNull(services);

        if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentNullException(nameof(connectionString));

        services.AddSingleton<IDbConnectionFactory>(new NpgsqlConnectionFactory(connectionString));

        AddLightHouseDataServices(services);

        return services;
    }

    public static IServiceCollection AddLightHouseDataServices(IServiceCollection services, Func<IServiceProvider, string> connectionStringFactory)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(connectionStringFactory);

        services.AddSingleton<IDbConnectionFactory>(sp =>
        {
            var connectionString = connectionStringFactory(sp);
            if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentNullException(nameof(connectionString));

            return new NpgsqlConnectionFactory(connectionString);
        });

        AddLightHouseDataServices(services);

        return services;
    }

    private static void AddLightHouseDataServices(IServiceCollection services)
    {
        services.AddScoped<ILightHouseRepository, LightHouseRepository>();
        services.AddScoped<IPhotoRepository, PhotoRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<CountryDataReader>();

        services.AddScoped<ICountryDataReader>(sp =>
        {
            var repo = sp.GetRequiredService<CountryDataReader>();
            var cache = sp.GetService<LightHouseInfrastructure.Caching.ICacheService>();
            return cache is null ? repo : new CachedCountryDataReader(repo, cache);
        });
    }
}