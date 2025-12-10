using System;
using System.Net.Http.Json;
using LightHouseApplication.Common;
using LightHouseApplication.Contracts;

namespace LightHouseInfrastructure.Auditors;

public class ExternalCommentAuditor(HttpClient httpClient) : ICommentAuditor
{
    public async Task<Result<bool>> IsTextAppropriateAsync(string text)
    {
        var response = await httpClient.PostAsJsonAsync("http://localhost:5000", new { Text = text });

        AuditResult result;
        try
        {
            result = await response.Content.ReadFromJsonAsync<AuditResult>();
            return Result<bool>.Ok(result.IsAppropriate);   
        }
        catch (System.Exception ex)         
        {
            return Result<bool>.Fail($"Failed to parse audit response: {ex.Message}")   ;
        }
    }
}

public class AuditResult
{
    public bool IsAppropriate { get; set; }
}
