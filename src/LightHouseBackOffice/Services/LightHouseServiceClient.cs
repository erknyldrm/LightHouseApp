using System;
using System.Runtime.InteropServices.Marshalling;
using System.Text.Json;
using LightHouseBackOffice.Models;

namespace LightHouseBackOffice.Services;

public interface ILightHouseServiceClient
{
    Task<ApiResponse<Guid>> CreateAsync(CreateLightHouseRequest request);
    Task<ApiResponse<IEnumerable<LightHouseDto>>> GetPagedAsync(int pageNumber, int pageSize);
}


public class LightHouseServiceClient : ILightHouseServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<LightHouseServiceClient> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public LightHouseServiceClient(IHttpClientFactory httpClientFactory, ILogger<LightHouseServiceClient> logger)
    {
        _httpClient = httpClientFactory.CreateClient("LightHouseServiceClient");
        _logger = logger;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<ApiResponse<Guid>> CreateAsync(CreateLightHouseRequest request)
    {
        try
        {
            var body = new StringContent(JsonSerializer.Serialize(request, _jsonOptions), System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("lighthouse", body);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                if (Guid.TryParse(responseContent.Trim('"'), out var lightHouseId))
                {
                    return new ApiResponse<Guid>
                    {
                        Success = true,
                        Data = lightHouseId
                    };
                }
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            return new ApiResponse<Guid>
            {
                Success = false,
                ErrorMessage = $"Invalid response format. Content: {errorContent} StatusCode: {response.StatusCode}"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating lighthouse.");
            return new ApiResponse<Guid>
            {
                Success = false,
                ErrorMessage = "Internal server error. Creation failed."
            };
        }
    }

    public Task<ApiResponse<IEnumerable<LightHouseDto>>> GetPagedAsync(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }
}
