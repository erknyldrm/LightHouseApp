using System;
using LightHouseApplication.Common;
using LightHouseApplication.Common.Pipeline;
using LightHouseApplication.Contracts.Repositories;

namespace LightHouseApplication.Features.LightHouse;

internal record DeleteLightHouseRequest(Guid LightHouseId);

internal class DeleteLightHouseHandler(ILightHouseRepository lightHouseRepository) : IHandler<DeleteLightHouseRequest, Result>
{
    public async Task<Result> HandleAsync(DeleteLightHouseRequest request, CancellationToken cancellationToken)
    {
        var lightHouseResult =  await lightHouseRepository.GetByIdAsync(request.LightHouseId);
        if (!lightHouseResult.IsSuccess)
        {
            return Result.Fail(lightHouseResult.ErrorMessage!); 
        }

        var deleteResult = await lightHouseRepository.DeleteAsync(request.LightHouseId);

        if (!deleteResult.IsSuccess)
        {
            return Result.Fail(deleteResult.ErrorMessage!);
        }

        return Result.Ok(); 
    }
}
