using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace LightHouseApplication.Common.Pipeline.Behavior;

public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;

    public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<TResponse> HandleAsync(TRequest request, Func<Task<TResponse>> next, CancellationToken cancellationToken = default)
    {
        var requestName = typeof(TRequest).Name;
        var stopwatch = Stopwatch.StartNew();

        _logger.LogInformation("Handling {RequestName}", requestName);

        var response = await next();

        stopwatch.Stop();
        
        if (stopwatch.ElapsedMilliseconds > 500) // Threshold in milliseconds
        {
            _logger.LogWarning("Long Running Request: {RequestName} took {ElapsedMilliseconds}ms", requestName, stopwatch.ElapsedMilliseconds);
        }

        return response;
    }
}
