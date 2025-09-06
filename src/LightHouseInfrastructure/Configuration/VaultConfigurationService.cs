using System;
using LightHouseDomain.Interfaces;
using Microsoft.Extensions.Logging;

namespace LightHouseInfrastructure.Configuration;

public class VaultConfigurationService(ISecretManager secretManager, ILogger<VaultConfigurationService> logger)
{
    private readonly ISecretManager _secretManager = secretManager;
    private readonly ILogger<VaultConfigurationService> _logger = logger;
    private readonly string SecretPath = "LightHouseApp-Dev";

    public async Task<string> GetDatabaseConnectionStringAsync()
    {
        try
        {
            var connectionString = await _secretManager.GetSecretAsync(SecretPath, "DbConnStr");

            if (string.IsNullOrEmpty(connectionString))
            {
                _logger.LogWarning("Database connection string is null or empty.");
                throw new InvalidOperationException("Database connection string is not found in Vault.");
            }
            return connectionString;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving database connection string from Vault.");
            throw;
        }
    }

    public async Task<(string AccessKey, string SecretKey)> GetMinioCredentialAsync()
    {
        try
        {
            var accessKey = await _secretManager.GetSecretAsync(SecretPath, "MinIOAccessKey");
            var secretKey = await _secretManager.GetSecretAsync(SecretPath, "MinIOSecretKey");

            if (string.IsNullOrEmpty(accessKey) || string.IsNullOrEmpty(secretKey))
            {
                _logger.LogWarning("MinIO credentials are null or empty.");
                throw new InvalidOperationException("MinIO credentials are not found in Vault.");
            }
            return (accessKey, secretKey);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving MinIO credentials from Vault.");
            throw;
        }
    }

    public async Task<Dictionary<string,string>> GetAllSecretsAsync()
    {
        try
        {
            var secrets = await _secretManager.GetSecretsAsync(SecretPath);

            if (secrets == null || secrets.Count == 0)
            {
                _logger.LogWarning("No secrets found at the specified path.");
                throw new InvalidOperationException("No secrets found in Vault.");
            }
            return secrets;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all secrets from Vault.");
            throw;
        }
    }
}
