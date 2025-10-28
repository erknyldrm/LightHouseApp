using LightHouseApplication.Common;
using LightHouseApplication.Dtos;
using LightHouseApplication.Features.Photo.Models;
using LightHouseDomain.Interfaces;
using LightHouseDomain.ValueObjects;

namespace LightHouseInfrastructure.Features.Photo;

internal class UploadPhotoHandler(IPhotoStorageService photoStorageService, IPhotoRepository photoRepository)
{
    private readonly IPhotoStorageService _photoStorageService = photoStorageService;

    private readonly IPhotoRepository _photoRepository = photoRepository;

    public async Task<Result<Guid>> HandleAsync(UploadPhotoRequest request, Stream content)
    {
        try
        {
            if (content == null || content.Length == 0)
                return Result<Guid>.Fail("Photo content cannot be empty.");

            var savedPath = await _photoStorageService.SavePhotoAsync(content, request.Photo.FileName);

            var metadata = new PhotoMetadata("N/A", "Unknown", request.Photo.CameraModel, request.Photo.UploadedAt);

            var photo = new LightHouseDomain.Entities.Photo(request.Photo.UserId, request.Photo.LightHouseId, savedPath, metadata);

            await _photoRepository.AddAsync(photo);

            return Result<Guid>.Ok(photo.Id);
        }
        catch (Exception ex)
        {
            return Result<Guid>.Fail($"An error occurred while uploading the photo: {ex.Message}");
        }
    }
}
