using System;

namespace LightHouseDomain.Events;

public class LightHouseCreated(Guid lightHouseId, string name, int countryId, string location)
{
    public Guid LightHouseId { get; protected set; } = lightHouseId;
    public string Name { get; protected set; } = name ?? throw new ArgumentNullException(nameof(name));
    public int CountryId { get; protected set; } = countryId;
    public string Location { get; protected set; } = location;
}
