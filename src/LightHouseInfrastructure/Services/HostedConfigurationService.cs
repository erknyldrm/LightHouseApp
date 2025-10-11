using System;
using LightHouseInfrastructure.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LightHouseInfrastructure.Services;

public class HostedConfigurationService(IServiceProvider serviceProvider, ILogger<HostedConfigurationService> logger) : IHostedService
{


    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("HostedConfigurationService is starting.");

        try
        {
            using var scope = serviceProvider.CreateScope();
            var cachedConfigService = scope.ServiceProvider.GetRequiredService<CachedConfigurationService>();

            if (cachedConfigService == null)
            {
                logger.LogError("CachedConfigurationService is not available.");
                throw new InvalidOperationException("CachedConfigurationService is not available.");
            }
            else
            {
                //Todo: CachedConfigurationService implementasyonu tamamlandıktan sonra burası açılacak
                //await cachedConfigService.WarmUpCacheAsync(cancellationToken);
               
            }
        }
        catch (System.Exception ex)
        {
            logger.LogError(ex, "An error occurred while starting HostedConfigurationService.");

        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("HostedConfigurationService is stopping.");
        return Task.CompletedTask;  
    }
}
