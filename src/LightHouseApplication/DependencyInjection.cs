using System;
using FluentValidation;
using LightHouseApplication.Contracts;
using LightHouseApplication.Dtos;
using LightHouseApplication.Services;
using LightHouseApplication.Validators;
using LightHouseDomain.Countries;
using LightHouseInfrastructure.Features.LightHouse;
using LightHouseInfrastructure.Features.Photo;
using Microsoft.Extensions.DependencyInjection;

namespace LightHouseApplication;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(IServiceCollection services)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddScoped<ILightHouseService, LightHouseService>();
        services.AddScoped<UploadPhotoHandler>();
        services.AddScoped<IPhotoService, PhotoService>();
        services.AddScoped<CreateLightHouseHandler>();
        services.AddScoped<GetLightHousesHandler>();
        services.AddScoped<ICountryRegistry, CountryRegisty>();
        services.AddScoped<IValidator<LightHouseDto>, LightHouseDtoValidator>();

        return services;
    }
}
