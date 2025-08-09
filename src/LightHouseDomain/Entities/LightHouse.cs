using LightHouseDomain.Common;
using LightHouseDomain.ValueObjects;


namespace LightHouseDomain.Entities;

public class LightHouse : EntityBase
{
    public string Name { get; private set; }
    public string Country { get; private set; }
    public Coordinates Locations { get; private set; }
    public List<Photo> Photos { get; } = [];

    protected LightHouse() { }

    public LightHouse(string name, string country, Coordinates locations)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Country = country ?? throw new ArgumentNullException(nameof(country));
        Locations = locations;
    }
}
