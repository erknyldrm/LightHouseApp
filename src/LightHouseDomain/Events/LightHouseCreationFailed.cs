using System;
using LightHouseDomain.Common;

namespace LightHouseDomain.Events;

public class LightHouseCreationFailed(string name, int countryId, double latitude, double longitude, string failedDescription, string errorDetails, Guid requestedBy) : EventBase
{
    public string Name { get; protected set; } = name;
    public int CountryId { get; protected set; } = countryId;
    public double Latitude { get; protected set; } = latitude;
    public double Longitude { get; protected set; } = longitude;
    public string FailedDescription { get; set; } = failedDescription;
    public string ErrorDetails { get; protected set; } = errorDetails;
    public Guid RequestedBy { get; protected set; } = requestedBy;
}
