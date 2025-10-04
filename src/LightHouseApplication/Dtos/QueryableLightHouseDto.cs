
namespace LightHouseApplication.Dtos;

public record QueryableLightHouseDto
(
    Guid Id,
    string Name,
    int CountryId,
    string CountryName,
    double Latitude,
    double Longitude,
    int PhotoCount,
    int CommentCount,
    double AverageRating,
    DateTime? LastPhotoAddedAt
);
