using System;
using LightHouseDomain.Common;

namespace LightHouseDomain.Events;

public class LightHouseCreated(Guid lightHouseId, string name, int countryId, double latitude, double longitude) : EventBase
{
    public Guid LightHouseId { get; protected set; } = lightHouseId;
    public string Name { get; protected set; } = name ?? throw new ArgumentNullException(nameof(name));
    public int CountryId { get; protected set; } = countryId;
    public double Latitude { get; protected set; } = latitude;
    public double Longitude { get; protected set; } = longitude;
}
