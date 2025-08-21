using System;
using LightHouseApplication.Common;
using LightHouseApplication.Dtos;
using LightHouseDomain.Entities;
using LightHouseDomain.Interfaces;
using LightHouseDomain.ValueObjects;

namespace LightHouseApplication.Features.LightHouse;

public class UploadPhotoHandler(IPhotoStorageService photoStorageService, IPhotoRepository photoRepository)
{
    private readonly IPhotoStorageService _photoStorageService = photoStorageService;

    private readonly IPhotoRepository _photoRepository = photoRepository;

    public async Task<Result<Guid>> HandleAsync(PhotoDto photoDto, Stream content)
    {
        try
        {
            if(content == null || content.Length == 0)
                return Result<Guid>.Fail("Photo content cannot be empty.");

            var savedPath = await _photoStorageService.SavePhotoAsync(content, photoDto.FileName);

            var metadata = new PhotoMetadata("N/A", "Unknown", photoDto.CameraModel, photoDto.UploadedAt);

            var photo = new Photo(photoDto.UserId, photoDto.LightHouseId, savedPath, metadata);

            await _photoRepository.AddAsync(photo);

            return Result<Guid>.Ok(photo.Id);
        }
        catch (Exception ex)
        {
            return Result<Guid>.Fail($"An error occurred while uploading the photo: {ex.Message}");
        }
    }
}
