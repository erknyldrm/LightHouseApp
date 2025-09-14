using System;
using System.Net.Http.Json;
using LightHouseDomain.Interfaces;
using Microsoft.Extensions.Logging;

namespace LightHouseInfrastructure.Auditors;

public class ExternalCommentAuditor(HttpClient httpClient) : ICommentAuditor
{
    public async Task<bool> IsTextAppropriateAsync(string text)
    {
        var response = await httpClient.PostAsJsonAsync("http://localhost:5000", new { Text = text });

        AuditResult result;
        try
        {
            result = await response.Content.ReadFromJsonAsync<AuditResult>();
            return result?.IsAppropriate ?? true;
        }
        catch (System.Exception ex)         
        {
            return false;
        }
    }
}

public class AuditResult
{
    public bool IsAppropriate { get; set; }
}
