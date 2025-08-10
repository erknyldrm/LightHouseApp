namespace LightHouseApplication.Dtos;

public record LightHouseDto(
    Guid Id,
    string Name,
    int CountryId,
    double Latitude,
    double Longitude
);

