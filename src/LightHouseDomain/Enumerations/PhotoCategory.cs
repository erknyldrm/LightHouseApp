using LightHouseDomain.Common;

namespace LightHouseDomain.Enumerations;

public sealed class PhotoCategory : EnumerationBase
{
    private PhotoCategory(int id, string name) : base(id, name) { }
    public static readonly PhotoCategory Sunset = new(1, "Sunset");
    public static readonly PhotoCategory Historical = new(2, "Historical");
    public static readonly PhotoCategory Storm = new(3, "Storm");
    public static readonly PhotoCategory Sundown = new(4, "Sundown");

    public static IEnumerable<PhotoCategory> List()
    {
        return [Sunset, Historical, Storm, Sundown];
    }
    public static PhotoCategory FromId(int id)
    {
        return List().FirstOrDefault(x => x.Id == id) ?? throw new ArgumentException($"No PhotoCategory with Id {id} found.");
    }   

    public static PhotoCategory FromName(string name)
    {
        return List().FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) 
               ?? throw new ArgumentException($"No PhotoCategory with Name '{name}' found.");
    }   
}
