using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace LightHouseApplication.Common.Pipeline.Behavior;

public class PerformanceBehavior<TRequest, TResponse>(ILogger<PerformanceBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
{

    public async Task<TResponse> HandleAsync(TRequest request, Func<Task<TResponse>> next, CancellationToken cancellationToken = default)
    {
        var requestName = typeof(TRequest).Name;
        var stopwatch = Stopwatch.StartNew();

        logger.LogInformation("Handling {RequestName}", requestName);

        var response = await next();

        stopwatch.Stop();
        
        if (stopwatch.ElapsedMilliseconds > 500) // Threshold in milliseconds
        {
            logger.LogWarning("Long Running Request: {RequestName} took {ElapsedMilliseconds}ms", requestName, stopwatch.ElapsedMilliseconds);
        }

        return response;
    }
}
