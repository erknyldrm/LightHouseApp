using System;
using LightHouseDomain.Events.Photo;

namespace LightHouseEventWorker.EventHandlers;

public interface IPhotoUploadedEventHandler
{
    Task HandleAsync(PhotoUploaded photoUploaded, CancellationToken cancellationToken = default );
}
