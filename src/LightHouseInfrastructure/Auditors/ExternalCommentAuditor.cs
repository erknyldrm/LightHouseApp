using System;
using System.Net.Http.Json;
using LightHouseDomain.Interfaces;

namespace LightHouseInfrastructure.Auditors;

public class ExternalCommentAuditor(HttpClient httpClient) : ICommentAuditor
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<bool> IsTextAppropriateAsync(string text)
    {
        var response = await _httpClient.PostAsJsonAsync("http://localhost:5000", new { Text = text });

        AuditResult result;
        try
        {
            result = await response.Content.ReadFromJsonAsync<AuditResult>();
            return result?.IsAppropriate ?? true;
        }
        catch (System.Exception)
        {
            return false;
        }
    }
}

public class AuditResult
{
    public bool IsAppropriate { get; set; }
}
