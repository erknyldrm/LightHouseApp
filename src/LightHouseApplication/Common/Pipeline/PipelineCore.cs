using System;
using Microsoft.Extensions.DependencyInjection;

namespace LightHouseApplication.Common.Pipeline;

public interface IHandler<TRequest, TResponse>
{
    Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken);
}

public interface IPipelineBehavior<TRequest, TResponse>
{
    Task<TResponse> HandleAsync(TRequest request, Func<Task<TResponse>> next, CancellationToken cancellationToken = default);
}

public class PipelineDispatcher(IServiceProvider serviceProvider)
{
    private readonly IServiceProvider _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

    public async Task<TResponse> SendAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        var handler = _serviceProvider.GetRequiredService<IHandler<TRequest, TResponse>>();
        var behaviors = _serviceProvider.GetServices<IPipelineBehavior<TRequest, TResponse>>() ?? [];

        Func<Task<TResponse>> nextDelegate = () => handler.HandleAsync(request, cancellationToken);

        foreach (var behavior in behaviors.Reverse())
        {
            var next = nextDelegate;
            nextDelegate = () => behavior.HandleAsync(request, next, cancellationToken);
        }

        return await nextDelegate();
    }
}