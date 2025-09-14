using System;

namespace LightHouseDomain.Countries;

public class Country
{
    public int Id { get; set; }
    public string Name { get; set; }

    public override string ToString() 
    {
        return Name;
    }
    public Country()
    {

    }

    internal Country(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public static Country Create(int id, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Country name cannot be null or empty.", nameof(name));

        return new Country(id, name);
    }
}
