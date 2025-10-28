using FluentValidation;
using LightHouseApplication.Common;
using LightHouseApplication.Common.Pipeline;
using LightHouseApplication.Common.Pipeline.Behavior;
using LightHouseApplication.Contracts;
using LightHouseApplication.Dtos;
using LightHouseApplication.Features.LightHouse;
using LightHouseApplication.Features.Models;
using LightHouseApplication.Features.Photo.Saga;
using LightHouseApplication.Features.Photo.Saga.Steps;
using LightHouseApplication.Services;
using LightHouseApplication.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace LightHouseApplication;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddScoped<ILightHouseService, LightHouseService>();
        services.AddScoped<IPhotoService, PhotoService>();
        services.AddScoped<IPhotoUploadService, PhotoUploadService>();
        services.AddScoped<PhotoUploadSaga>();
        services.AddScoped<FileUploadStep>();
        services.AddScoped<MetadataSaveStep>();

        services.AddScoped<PipelineDispatcher>();


        //Todo: Register all handlers 
        services.AddScoped<IHandler<CreateLightHouseRequest, Result<Guid>>, CreateLightHouseHandler>(); 
        services.AddScoped<IHandler<GetTopLightHousesRequest, Result<IEnumerable<LightHouseTopDto>>>, GetTopLightHousesHandler>();    

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ExceptionHandlingBehavior<,>));

        services.AddScoped<IValidator<LightHouseDto>, LightHouseDtoValidator>();
        //services.AddScoped<IValidator<PhotoDto>, PhotoDtoValidator>();
        return services;
    }
}
