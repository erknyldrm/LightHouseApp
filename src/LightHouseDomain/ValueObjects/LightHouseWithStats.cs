
namespace LightHouseDomain.ValueObjects;

public class LightHouseWithStats
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public double AverageScore { get; set; }
    public int PhotoCount { get; set; }

}
