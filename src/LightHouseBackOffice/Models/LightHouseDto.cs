
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
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? ErrorMessage { get; set; }
};

public record PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = [];
    public int TotalCount { get; set; }
    public int PageSize { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}