using LightHouseApplication.Common;
using LightHouseApplication.Common.DistributedTransaction.Saga;
using LightHouseApplication.Dtos;
using LightHouseApplication.Features.Photo.Models;
using LightHouseApplication.Features.Photo.Saga.Steps;
using Microsoft.Extensions.Logging;

namespace LightHouseApplication.Features.Photo.Saga;

public class PhotoUploadSaga(ILogger<PhotoUploadSaga> logger) : ISagaProvider<UploadPhotoRequest, Result<PhotoDto>>
{
    private readonly FileUploadStep _fileUploadStep;
    private readonly MetadataSaveStep _metadataSaveStep;


    async Task<Result<PhotoDto>> ISagaProvider<UploadPhotoRequest, Result<PhotoDto>>.ExecuteAsync(UploadPhotoRequest request, CancellationToken cancellationToken)
    {
        var sagaId = Guid.NewGuid();

        logger.LogInformation("Starting PhotoUploadSaga with SagaId: {SagaId} for PhotoId: {PhotoId}", sagaId, request.Photo.Id);

        var sagaData = new PhotoUploadSagaData
        {
            PhotoId = request.Photo.Id,
            FileName = request.Photo.FileName,
            PhotoEntity = new LightHouseDomain.Entities.Photo(
                request.Photo.UserId,
                request.Photo.LightHouseId,
                request.Photo.FileName,
                new LightHouseDomain.ValueObjects.PhotoMetadata(
                    "",
                    "",
                    request.Photo.CameraModel,
                    request.Photo.UploadedAt
                )
            )
        };

        var steps = new List<ISagaProviderStep<PhotoUploadSagaData>>();

        try
        {
            var fileUploadResult = await _fileUploadStep.ExecuteStepAsync(sagaData, cancellationToken);

            if (!fileUploadResult.IsSuccess)
            {
                logger.LogWarning("File upload step failed in PhotoUploadSaga with SagaId: {SagaId} for PhotoId: {PhotoId}", sagaId, request.Photo.Id);

                return Result<PhotoDto>.Fail(fileUploadResult.ErrorMessage);
            }

            steps.Add(_fileUploadStep);

            var metadataSaveResult = await _metadataSaveStep.ExecuteStepAsync(sagaData, cancellationToken);

            if (!metadataSaveResult.IsSuccess)
            {
                logger.LogWarning("Metadata save step failed in PhotoUploadSaga with SagaId: {SagaId} for PhotoId: {PhotoId}", sagaId, request.Photo.Id);

                foreach (var step in Enumerable.Reverse(steps))
                {
                    await CompensateAsync(sagaData, cancellationToken);
                }

                return Result<PhotoDto>.Fail(metadataSaveResult.ErrorMessage);
            }

            var photoDto = new PhotoDto
            (
                sagaData.PhotoEntity.Id,
                sagaData.PhotoEntity.Filename,
                sagaData.PhotoEntity.UploadDate,
                sagaData.PhotoEntity.Metadata.cameraModel,
                sagaData.PhotoEntity.Id,
                sagaData.PhotoEntity.LighthouseId
            );

            logger.LogInformation("PhotoUploadSaga with SagaId: {SagaId} completed successfully for PhotoId: {PhotoId}", sagaId, request.Photo.Id);
            
            return Result<PhotoDto>.Ok(photoDto);
        }
        catch (System.Exception ex)
        {
            logger.LogError(ex, "PhotoUploadSaga with SagaId: {SagaId} failed for PhotoId: {PhotoId}", sagaId, request.Photo.Id);

            await CompensateAsync(sagaData, cancellationToken);

            return Result<PhotoDto>.Fail($"Photo upload saga failed: {ex.Message}");
        }
    }

    private async Task CompensateAsync(PhotoUploadSagaData sagaData, CancellationToken cancellationToken)
    {

        logger.LogInformation("Starting compensation for PhotoUploadSaga for PhotoId: {PhotoId}", sagaData.PhotoId);

        var steps = new List<ISagaProviderStep<PhotoUploadSagaData>>
        {
            _metadataSaveStep,
            _fileUploadStep
        };

        foreach (var step in steps)
        {
            try
            {
                await step.CompensateAsync(sagaData, cancellationToken);

            }
            catch (System.Exception ex)
            {
                logger.LogError(ex, "Compensation step failed for PhotoId: {PhotoId}", sagaData.PhotoId);
            }
        }

        logger.LogInformation("Compensation completed for PhotoUploadSaga for PhotoId: {PhotoId}", sagaData.PhotoId);
    }
}
