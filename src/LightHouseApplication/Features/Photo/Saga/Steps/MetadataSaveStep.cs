using LightHouseApplication.Common;
using LightHouseApplication.Common.DistributedTransaction.Saga;
using LightHouseDomain.Interfaces;
using Microsoft.Extensions.Logging;

namespace LightHouseApplication.Features.Photo.Saga.Steps;

public class MetadataSaveStep(IPhotoRepository photoRepository, ILogger<MetadataSaveStep> logger) : ISagaProviderStep<PhotoUploadSagaData>
{
    public async Task<Result<PhotoUploadSagaData>> ExecuteStepAsync(PhotoUploadSagaData request, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Starting metadata save for PhotoId: {PhotoId}", request.PhotoId);

            if (request.PhotoEntity == null)
            {
                return Result<PhotoUploadSagaData>.Fail("Photo entity is null. Cannot save metadata.");
            }
            request.PhotoEntity.SetFileName(request.FileName);

            await photoRepository.AddAsync(request.PhotoEntity);

            request.IsMetadataSaved = true;

            logger.LogInformation("Metadata save completed for PhotoId: {PhotoId}", request.PhotoId);

            return Result<PhotoUploadSagaData>.Ok(request);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Metadata save failed for PhotoId: {PhotoId}", request.PhotoId);

            request.LastException = ex;

            return Result<PhotoUploadSagaData>.Fail($"Metadata save failed: {ex.Message}");
        }
    }
    public async Task CompensateAsync(PhotoUploadSagaData request, CancellationToken cancellationToken = default)
    {
       if (!request.IsMetadataSaved)
        {
            logger.LogInformation("No compensation needed for metadata save of PhotoId: {PhotoId}", request.PhotoId);
            return;
        }
        else
        {
            try
            {
                logger.LogInformation("Starting metadata deletion for PhotoId: {PhotoId}", request.PhotoId);

                await photoRepository.DeleteAsync(request.PhotoId);

                request.IsMetadataSaved = false;

                logger.LogInformation("Metadata deletion completed for PhotoId: {PhotoId}", request.PhotoId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Metadata deletion failed during compensation for PhotoId: {PhotoId}", request.PhotoId);
            }

            return;
        }
    }
}
