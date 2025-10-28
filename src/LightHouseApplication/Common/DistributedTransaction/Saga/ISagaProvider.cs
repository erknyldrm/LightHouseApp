
namespace LightHouseApplication.Common.DistributedTransaction.Saga;

public interface ISagaProvider<TRequest, TResponse>
{
    Task<TResponse> ExecuteAsync(TRequest request, CancellationToken cancellationToken = default);


}

public interface ISagaProviderStep<TStepData>
{
    Task<Result<TStepData>> ExecuteStepAsync(TStepData request, CancellationToken cancellationToken = default);
    Task CompensateAsync(TStepData request, CancellationToken cancellationToken = default);
}

