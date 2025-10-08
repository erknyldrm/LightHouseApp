using System;
using LightHouseDomain.Interfaces;
using LightHouseInfrastructure.Identity;
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

    public async Task<Dictionary<string, string>> GetAllSecretsAsync()
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

    public async Task<KeycloakSettings> KeycloakSettings()
    {
        try
        {
            var keycloakSettings = new KeycloakSettings
            {
                Audience = await _secretManager.GetSecretAsync(SecretPath, "KeycloakAudience"),
                Authority = await _secretManager.GetSecretAsync(SecretPath, "KeycloakAuthority"),
                Realm = await _secretManager.GetSecretAsync(SecretPath, "KeycloakRealm"),
                ClientId = await _secretManager.GetSecretAsync(SecretPath, "KeycloakClientId"),
                ClientSecret = await _secretManager.GetSecretAsync(SecretPath, "KeycloakClientSecret"),
                ClockSkew = await _secretManager.GetSecretAsync(SecretPath, "KeycloakClockSkew") is string clockSkewStr && int.TryParse(clockSkewStr, out var clockSkew) ? clockSkew : 5,
                RequireHttpsMetadata = await _secretManager.GetSecretAsync(SecretPath, "KeycloakRequireHttpsMetadata") is not string requireHttpsMetadataStr || !bool.TryParse(requireHttpsMetadataStr, out var requireHttpsMetadata) || requireHttpsMetadata,
                ValidateAudience = await _secretManager.GetSecretAsync(SecretPath, "KeycloakValidateAudience") is not string validateAudienceStr || !bool.TryParse(validateAudienceStr, out var validateAudience) || validateAudience,
                ValidateIssuer = await _secretManager.GetSecretAsync(SecretPath, "KeycloakValidateIssuer") is not string validateIssuerStr || !bool.TryParse(validateIssuerStr, out var validateIssuer) || validateIssuer,
                ValidateLifetime = await _secretManager.GetSecretAsync(SecretPath, "KeycloakValidateLifetime") is not string validateLifetimeStr || !bool.TryParse(validateLifetimeStr, out var validateLifetime) || validateLifetime,
                ValidateTokenSignature = await _secretManager.GetSecretAsync(SecretPath, "KeycloakValidateTokenSignature") is not string validateTokenSignatureStr || !bool.TryParse(validateTokenSignatureStr, out var validateTokenSignature) || validateTokenSignature
            };

            if (string.IsNullOrEmpty(keycloakSettings.Audience) ||
                            string.IsNullOrEmpty(keycloakSettings.Authority) ||
                            string.IsNullOrEmpty(keycloakSettings.Realm) ||
                            string.IsNullOrEmpty(keycloakSettings.ClientId) ||
                            string.IsNullOrEmpty(keycloakSettings.ClientSecret))
            {
                _logger.LogWarning("One or more Keycloak settings are null or empty.");
                throw new InvalidOperationException("Keycloak settings are not fully configured in Vault.");
            }

            _logger.LogInformation("Successfully retrieved Keycloak settings from Vault.");
            return keycloakSettings;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all  keycloak secrets from Vault.");
            return new KeycloakSettings();
        }

    }
}
