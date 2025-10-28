
namespace LightHouseApplication.Features.Photo.Saga;

public class PhotoUploadSagaData
{
    public Guid PhotoId { get; set; }
    public string FileName { get; set; } = String.Empty;
    public Stream? FileStream { get; set; }

    public bool IsFileUploaded { get; set; }
    public bool IsMetadataSaved { get; set; }

    public LightHouseDomain.Entities.Photo? PhotoEntity { get; set; }
    public Exception? LastException { get; set; }
}
