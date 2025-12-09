using System;
using Microsoft.Extensions.Logging;

namespace LightHouseApplication.Common.Pipeline.Behavior;

public class ExceptionHandlingBehavior<TRequest, TResponse>(ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
{
  
    public async Task<TResponse> HandleAsync(TRequest request, Func<Task<TResponse>> next, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Handling request of type {RequestType}", typeof(TRequest).Name);    
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;
            var requestType = request?.GetType().Name ?? "Unknown";

            logger.LogError(ex, "Unhandled exception for request {RequestName} of type {RequestType}: {ExceptionMessage}", requestName, requestType, ex.Message);
           
            if (typeof(TResponse) == typeof(Result))
            {
                return (TResponse)(object)Result.Fail("An unexpected error occurred."); 
            }

            if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
            {
                var resultType = typeof(TResponse).GetGenericArguments()[0];
                var failMethod = typeof(Result<>).MakeGenericType(resultType).GetMethod("Fail", new Type[] { typeof(string) });

                if (failMethod != null)
                {
                    var failedResult = failMethod.Invoke(null, new object[] { "An unexpected error occurred." });
                    if (failedResult is TResponse typedResult)
                    {
                        return typedResult;
                    }
                } 
            }
        }

        return default!;
    }
}
