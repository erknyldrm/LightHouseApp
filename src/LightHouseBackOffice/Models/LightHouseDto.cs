using System;

namespace LightHouseBackOffice.Models;

public record CreateLightHouseRequest(string Name, int CountryId, double Latitude, double Longitude);

public record LightHouseDto
(
    Guid Id,
    string Name,
    int CountryId, double Latitude,
    double Longitude
);

public record ApiResponse<T>
{
    public bool Success { get; init; }
    public T? Data { get; init; }
    public string? ErrorMessage { get; init; }
};