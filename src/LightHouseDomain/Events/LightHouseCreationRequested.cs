using System;
using LightHouseDomain.Common;

namespace LightHouseDomain.Events;

public class LightHouseCreationRequested(Guid lightHouseId, string name, int countryId, string location) : EventBase
{
    public Guid LightHouseId { get; } = lightHouseId;
    public string Name { get; set; } = name ?? throw new ArgumentNullException(nameof(name));
    public int CountryId { get; set; } = countryId;
    public string Location { get; } = location;
}
