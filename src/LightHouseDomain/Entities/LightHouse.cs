using LightHouseDomain.Common;
using LightHouseDomain.Countries;
using LightHouseDomain.ValueObjects;


namespace LightHouseDomain.Entities;

public class LightHouse : EntityBase
{
    public string Name { get; private set; }
    public int CountryId { get; private set; }
    public Country Country { get; private set; }
    public Coordinates Location { get; private set; }
    public List<Photo> Photos { get; } = [];

    protected LightHouse() { }

    public LightHouse(string name, Country country, Coordinates location)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Country = country ?? throw new ArgumentNullException(nameof(country));
        Location = location;
    }
}
