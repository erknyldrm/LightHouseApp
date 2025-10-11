using System;

namespace LightHouseInfrastructure.Configuration;

public class ElasticSearchSettings
{
    public string Url { get; set; } = "http://localhost:9200";
    public string IndexFormat { get; set; } = "lighthouse-logs-{0:yyyy.MM.dd}";
    private string Username { get; set; } = String.Empty;
    private string Password { get; set; } = String.Empty;

}
