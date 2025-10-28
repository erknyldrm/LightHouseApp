using LightHouseApplication.Common;
using LightHouseApplication.Contracts;
using LightHouseApplication.Dtos;
using LightHouseApplication.Features.Photo.Saga;
using Microsoft.Extensions.Logging;

namespace LightHouseApplication.Services;

public class PhotoUploadService(PhotoUploadSaga photoUploadSaga, ILogger<PhotoUploadService> logger) : IPhotoUploadService
{
    public async Task<Result<PhotoDto>> UploadPhotoAsync(PhotoDto photo, Stream fileContent)
    {
        try
        {
            logger.LogInformation("Starting photo upload for PhotoId: {PhotoId}", photo.Id);

            var result = await photoUploadSaga.ExecuteAsync(new Features.Photo.Models.UploadPhotoRequest(photo, fileContent), CancellationToken.None);
            
            if (result.IsSuccess)
            {
                logger.LogInformation("Photo upload succeeded for PhotoId: {PhotoId}", photo.Id);
            }
            else
            {
                logger.LogWarning("Photo upload failed for PhotoId: {PhotoId} with error: {ErrorMessage}", photo.Id, result.ErrorMessage);
            }
            return result;              
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while uploading the photo for PhotoId: {PhotoId}", photo.Id);

            return Result<PhotoDto>.Fail($"An error occurred while uploading the photo: {ex.Message}");
        }
    }
}
