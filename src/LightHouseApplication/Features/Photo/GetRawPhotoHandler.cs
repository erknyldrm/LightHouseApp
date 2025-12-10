using System;
using LightHouseApplication.Common;
using LightHouseApplication.Common.Pipeline;
using LightHouseApplication.Contracts;

namespace LightHouseApplication.Features.Photo;

internal record GetRawPhotoRequest(string FileName);
internal class GetRawPhotoHandler(IPhotoStorageService storageService)
    : IHandler<GetRawPhotoRequest, Result<Stream>>
{
    public async Task<Result<Stream>> HandleAsync(GetRawPhotoRequest request, CancellationToken cancellationToken)
    {
        var streamResult = await storageService.GetAsync(request.FileName, cancellationToken);
        if (!streamResult.IsSuccess)
        {
            return Result<Stream>.Fail(streamResult.ErrorMessage!);
        }

        var stream = streamResult.Data;
        if (stream is null || stream.Length == 0)
        {
            return Result<Stream>.Fail("PhotoNotFoundInStorage");
        }
        return Result<Stream>.Ok(stream);
    }
}