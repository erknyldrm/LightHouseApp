using System;

namespace LightHouseDomain.Countries;

public class Country
{
    public int Id { get; }
    public string Name { get; }

    public override string ToString()
    {
        return Name;
    }   

    internal Country(int id, string name)
    {
        Id = id;
        Name = name;
    }
}
