using System;
using System.Net.Http.Json;
using LightHouseDomain.Interfaces;

namespace LightHouseInfrastructure.Auditors;

public class ExternalCommentAuditor(HttpClient httpClient) : ICommentAuditor
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<bool> IsTextAppropriateAsync(string text)
    {
        var response = await _httpClient.PostAsJsonAsync("https://external-auditor-api.com/audit", new { Text = text });

        var result = await response.Content.ReadFromJsonAsync<AuditResult>();
        return result?.IsAppropriate ?? true;
    }
}

public class AuditResult
{
    public bool IsAppropriate { get; set; }
}
