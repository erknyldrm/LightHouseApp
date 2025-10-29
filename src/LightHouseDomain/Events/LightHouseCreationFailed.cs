using System;
using LightHouseDomain.Common;

namespace LightHouseDomain.Events;
public class LightHouseCreationFailed(string name, int countryId, string location, string errorDetails, Guid requestedBy) : EventBase
{
    public string Name { get; protected set; } = name;
    public int CountryId { get; protected set; } = countryId;
    public string Location { get; protected set; } = location;
    public string ErrorDetails { get; protected set; } = errorDetails;
    public Guid RequestedBy { get; protected set; } = requestedBy;
}
