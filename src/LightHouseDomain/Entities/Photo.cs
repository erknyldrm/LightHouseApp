using LightHouseDomain.Common;
using LightHouseDomain.ValueObjects;

namespace LightHouseDomain.Entities;

public class Photo : EntityBase
{
    public Guid UserId { get; private set; }
    public Guid LighthouseId { get; private set; }
    public string Filename { get; private set; }
    public DateTime UploadDate { get; private set; }
    public PhotoMetadata Metadata { get; private set; }
    public List<Comment> Comments { get; } = [];

    protected Photo() { }

    public Photo(Guid userId, Guid lighthouseId, string filename, PhotoMetadata metadata)
    {
        UserId = userId;
        LighthouseId = lighthouseId;
        Filename = filename;
        UploadDate = DateTime.UtcNow;
        Metadata = metadata;
    }
}
