using LightHouseApplication.Common;
using LightHouseApplication.Common.DistributedTransaction.Saga;
using LightHouseApplication.Contracts;
using Microsoft.Extensions.Logging;

namespace LightHouseApplication.Features.Photo.Saga.Steps;

public class FileUploadStep(IPhotoStorageService photoStorageService, ILogger<FileUploadStep> logger) : ISagaProviderStep<PhotoUploadSagaData>
{
    public async Task<Result<PhotoUploadSagaData>> ExecuteStepAsync(PhotoUploadSagaData request, CancellationToken cancellationToken = default)
    {

        try
        {
            logger.LogInformation("Starting file upload for PhotoId: {PhotoId}", request.PhotoId);
            request.FileStream.Position = 0;

            var fileNameResult = await photoStorageService.SaveAsync(request.FileStream!, request.FileName, cancellationToken);

            if (!fileNameResult.IsSuccess)
            {
                logger.LogError("File upload failed for PhotoId: {PhotoId}, Error: {ErrorMessage}", request.PhotoId, fileNameResult.ErrorMessage);
                return Result<PhotoUploadSagaData>.Fail(fileNameResult.ErrorMessage!);
            }
            
            request.FileName = fileNameResult.Data!;

            request.IsFileUploaded = true;

            logger.LogInformation("File upload completed for PhotoId: {PhotoId}, FileName: {FileName}", request.PhotoId, fileNameResult);

            return Result<PhotoUploadSagaData>.Ok(request);

        }
        catch (System.Exception ex)
        {
            logger.LogError(ex, "File upload failed for PhotoId: {PhotoId}", request.PhotoId);

            return Result<PhotoUploadSagaData>.Fail($"File upload failed: {ex.Message}");
        }
    }

    public async Task CompensateAsync(PhotoUploadSagaData request, CancellationToken cancellationToken = default)
    {
        if (request.IsFileUploaded || !string.IsNullOrEmpty(request.FileName))
        {
            logger.LogInformation("No compensation needed for file upload of PhotoId: {PhotoId}", request.PhotoId);
            return;
        }
        else
        {
            try
            {
                logger.LogInformation("Starting file deletion for photoId:{PhotoId}", request.PhotoId);

                await photoStorageService.DeleteAsync(request.FileName, cancellationToken);

                request.IsFileUploaded = false;

                logger.LogInformation("File deletion completed for PhotoId: {PhotoId}, FileName: {FileName}", request.PhotoId, request.FileName);
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex, "File deletion failed during compensation for PhotoId: {PhotoId}", request.PhotoId);
            }
        }
    }
}
