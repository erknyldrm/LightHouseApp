using System;
using Microsoft.Extensions.Logging;

namespace LightHouseApplication.Common.Pipeline.Behavior;

public class ExceptionHandlingBehavior<TRequest, TResponse>(ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> _logger = logger;

    public async Task<TResponse> HandleAsync(TRequest request, Func<Task<TResponse>> next, CancellationToken cancellationToken = default)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;
            var requestType = request?.GetType().Name ?? "Unknown";

            _logger.LogError(ex, "Unhandled exception for request {RequestName} of type {RequestType}: {ExceptionMessage}", requestName, requestType, ex.Message);
           
            throw;
        }
    }
}
