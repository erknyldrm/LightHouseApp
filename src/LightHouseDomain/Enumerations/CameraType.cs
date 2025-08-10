using LightHouseDomain.Common;

namespace LightHouseDomain.Enumerations;

public sealed class CameraType : EnumerationBase
{
    private CameraType(int id, string name) : base(id, name) { }
    public static readonly CameraType SLR = new(1, "SLR");

    public static readonly CameraType DSLR = new(2, "DSLR");
    public static readonly CameraType Mirrorless = new(3, "Mirrorless");
    public static readonly CameraType PointAndShoot = new(4, "Point and Shoot");
    public static readonly CameraType ActionCamera = new(5, "Action Camera");

    public static IEnumerable<CameraType> List() => [SLR, DSLR, Mirrorless, PointAndShoot, ActionCamera];

    public static CameraType FromId(int id)
    {
        return List().FirstOrDefault(x => x.Id == id) ?? throw new ArgumentException($"No CameraType with Id {id} found.");
    }   

    public static CameraType FromName(string name)
    {
        return List().FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) 
               ?? throw new ArgumentException($"No CameraType with Name '{name}' found.");
    }   
}
