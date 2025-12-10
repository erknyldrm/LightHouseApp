using System;
using LightHouseApplication.Common;
using LightHouseApplication.Common.Pipeline;
using LightHouseApplication.Contracts.Repositories;
using LightHouseApplication.Dtos;

namespace LightHouseApplication.Features.Photo;

internal record GetPhotosByLightHouseRequest(Guid LighthouseId);

internal class GetPhotosByLightHouseHandler(IPhotoRepository photoRepository)
    : IHandler<GetPhotosByLightHouseRequest, Result<IEnumerable<PhotoDto>>>
{
    public async Task<Result<IEnumerable<PhotoDto>>> HandleAsync(GetPhotosByLightHouseRequest request, CancellationToken cancellationToken)
    {
        var photosResult = await photoRepository.GetPhotosByLightHouseIdAsync(request.LighthouseId);
        if (!photosResult.IsSuccess)
        {
            return Result<IEnumerable<PhotoDto>>.Fail(photosResult.ErrorMessage!);
        }

        var photos = photosResult.Data!;
        if (!photos.Any())
        {
            return Result<IEnumerable<PhotoDto>>.Fail("NoPhotosFoundForLighthouse");
        }

        var photoDtos = photos.Select(
            p => new PhotoDto(
                p.Id,
                p.Filename,
                p.UploadDate,
                "",
                p.UserId,
                p.LighthouseId)
        ).ToList();

        return Result<IEnumerable<PhotoDto>>.Ok(photoDtos);
    }
}
