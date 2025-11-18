using System;
using LightHouseDomain.Events.Photo;

namespace LightHouseEventWorker.EventHandlers;

public class PhotoUploadedEventHandler(ILogger<PhotoUploadedEventHandler> logger) : IPhotoUploadedEventHandler
{

    public Task HandleAsync(PhotoUploaded photoUploaded, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(photoUploaded);

        logger.LogInformation("Handling PhotoUploaded event for PhotoId: {PhotoId}, FileName: {FileName}, UserId: {UserId}, LighthouseId: {LighthouseId}, UploadedAt: {UploadedAt}",
            photoUploaded.AggregateId,
            photoUploaded.FileName,
            photoUploaded.UserId,
            photoUploaded.LighthouseId,
            photoUploaded.UploadedAt);


        throw new NotImplementedException();
    }
}
