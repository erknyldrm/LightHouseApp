using System;
using Microsoft.Extensions.Logging;

namespace LightHouseApplication.Common.Pipeline.Behavior;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger) 
: IPipelineBehavior<TRequest, TResponse>
{
    public async Task<TResponse> HandleAsync(TRequest request, Func<Task<TResponse>> next, CancellationToken cancellationToken = default)
    {
        var requestName = typeof(TRequest).Name;

        logger.LogInformation("Handling request {RequestName} of type {RequestType}", requestName, typeof(TRequest).Name);  

        var response = await next();

        logger.LogInformation("Handled request {RequestName} of type {RequestType}", requestName, typeof(TRequest).Name);   

        return response;    
    }
}
