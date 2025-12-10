using System;
using LightHouseApplication.Common;
using LightHouseApplication.Common.Pipeline;
using LightHouseApplication.Contracts.Repositories;
using LightHouseApplication.Dtos;

namespace LightHouseApplication.Features.Photo;

internal record GetPhotoByIdRequest(Guid PhotoId);

internal class GetPhotoByIdHandler(IPhotoRepository photoRepository)
    : IHandler<GetPhotoByIdRequest, Result<PhotoDto>>
{
    public async Task<Result<PhotoDto>> HandleAsync(GetPhotoByIdRequest request, CancellationToken cancellationToken)
    {
        var photoResult = await photoRepository.GetByIdAsync(request.PhotoId);

        if (!photoResult.IsSuccess)
            return Result<PhotoDto>.Fail(photoResult.ErrorMessage!);

        var photo = photoResult.Data!;
        var dto = new PhotoDto
        (
            photo.Id,
            photo.Filename,
            photo.UploadDate,
            "",
            photo.UserId,
            photo.LighthouseId
        );

        return Result<PhotoDto>.Ok(dto);
    }
}