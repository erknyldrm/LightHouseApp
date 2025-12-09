using System;
using Microsoft.Extensions.Logging;

namespace LightHouseApplication.Common.Pipeline.Behavior;

public class CancellationBehavior<TRequest, TResponse>(ILogger<CancellationBehavior<TRequest, TResponse>> logger)
: IPipelineBehavior<TRequest, TResponse>
{
    public async Task<TResponse> HandleAsync(TRequest request, Func<Task<TResponse>> next, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            var requestName = typeof(TRequest).Name;    

            logger.LogWarning("Request {RequestName} was cancelled.", requestName); 

            return HandleCancellation();
            
        }

        return await next();
    }

    public static TResponse HandleCancellation()
    {
        if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
        {
            var resultType = typeof(TResponse).GetGenericArguments()[0];
            var failMethod = typeof(Result<>).MakeGenericType(resultType).GetMethod("Fail", new Type[] { typeof(string) });
            return (TResponse)failMethod.Invoke(null, new object[] { "Operation was cancelled." }); 
        }

        if (typeof(TResponse) == typeof(Result))
        {
            return (TResponse)(object)Result.Fail("Operation was cancelled.");  
        }

        throw new InvalidOperationException("Cannot handle cancellation for the given response type."); 
    }   
}
