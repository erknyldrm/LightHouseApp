namespace LightHouseDomain.ValueObjects;

public record PhotoMetadata(string lens, string resolution, string cameraModel, DateTime takenAt)
{
    public static PhotoMetadata Create(string lens, string resolution, string cameraModel, DateTime takenAt)
    {
        if (string.IsNullOrWhiteSpace(lens))
        {
            throw new ArgumentException("Lens cannot be null or empty.", nameof(lens));
        }
        if (string.IsNullOrWhiteSpace(resolution))
        {
            throw new ArgumentException("Resolution cannot be null or empty.", nameof(resolution));
        }
        if (string.IsNullOrWhiteSpace(cameraModel))
        {
            throw new ArgumentException("Camera model cannot be null or empty.", nameof(cameraModel));
        }
        
        return new PhotoMetadata(lens, resolution, cameraModel, takenAt);
    }
}
