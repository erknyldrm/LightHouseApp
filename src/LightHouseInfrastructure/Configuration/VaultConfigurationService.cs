
using LightHouseApplication.Contracts;
using LightHouseInfrastructure.Identity;
using Microsoft.Extensions.Logging;

namespace LightHouseInfrastructure.Configuration;

//Todo: Review usage and remove if not needed   
[Obsolete("This class is deprecated and will be removed in future versions. Please use CachedConfigurationService instead.")]
public class VaultConfigurationService(ISecretManager secretManager, ILogger<VaultConfigurationService> logger)
{
    private readonly ISecretManager _secretManager = secretManager;
    private readonly ILogger<VaultConfigurationService> _logger = logger;
    private readonly string SecretPath = "LightHouseApp-Dev";

    public async Task<string> GetDatabaseConnectionStringAsync()
    {
        try
        {
            var result = await _secretManager.GetSecretAsync(SecretPath, "DbConnStr");

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Failed to retrieve database connection string: {ErrorMessage}", result.ErrorMessage);
                throw new InvalidOperationException("Database connection string is not found in Vault.");
            }
            if (string.IsNullOrEmpty(result.Data))
            {
                _logger.LogWarning("Database connection string is null or empty.");
                throw new InvalidOperationException("Database connection string is not found in Vault.");
            }
            return result.Data;
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
            var accessKeyResult = await _secretManager.GetSecretAsync(SecretPath, "MinIOAccessKey");
            var secretKeyResult = await _secretManager.GetSecretAsync(SecretPath, "MinIOSecretKey");

            if (!accessKeyResult.IsSuccess || !secretKeyResult.IsSuccess)
            {
                _logger.LogWarning("Failed to retrieve MinIO credentials: {AccessKeyError}, {SecretKeyError}", accessKeyResult.ErrorMessage, secretKeyResult.ErrorMessage);
                throw new InvalidOperationException("MinIO credentials are not found in Vault.");
            }

            if (string.IsNullOrEmpty(accessKeyResult.Data) || string.IsNullOrEmpty(secretKeyResult.Data))
            {
                _logger.LogWarning("MinIO credentials are null or empty.");
                throw new InvalidOperationException("MinIO credentials are not found in Vault.");
            }
            return (accessKeyResult.Data, secretKeyResult.Data);
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
            var secretsResult = await _secretManager.GetSecretsAsync(SecretPath);

            if (!secretsResult.IsSuccess)
            {
                _logger.LogWarning("Failed to retrieve secrets: {ErrorMessage}", secretsResult.ErrorMessage);
                throw new InvalidOperationException("Failed to retrieve secrets from Vault.");
            }

            if (secretsResult.Data == null || secretsResult.Data.Count == 0)
            {
                _logger.LogWarning("No secrets found at the specified path.");
                throw new InvalidOperationException("No secrets found in Vault.");
            }
            return secretsResult.Data;
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
            var audience = await _secretManager.GetSecretAsync(SecretPath, "KeycloakAudience");
            var authirty = await _secretManager.GetSecretAsync(SecretPath, "KeycloakAuthority");
            var realm = await _secretManager.GetSecretAsync(SecretPath, "KeycloakRealm");
            var clientId = await _secretManager.GetSecretAsync(SecretPath, "KeycloakClientId");
            var clientSecret = await _secretManager.GetSecretAsync(SecretPath, "KeycloakClientSecret"); 

            var clockSkew = await _secretManager.GetSecretAsync(SecretPath, "KeycloakClockSkew");
            var requireHttpsMetadata = await _secretManager.GetSecretAsync(SecretPath, "KeycloakRequireHttpsMetadata");
            var validateAudience = await _secretManager.GetSecretAsync(SecretPath, "KeycloakValidateAudience");
            var validateIssuer = await _secretManager.GetSecretAsync(SecretPath, "KeycloakValidateIssuer");
            var validateLifetime = await _secretManager.GetSecretAsync(SecretPath, "KeycloakValidateLifetime");
            var validateTokenSignature = await _secretManager.GetSecretAsync(SecretPath, "KeycloakValidateTokenSignature");
            

            var keycloakSettings = new KeycloakSettings
            {
                Audience = audience.Data!,
                Authority = authirty.Data!,
                Realm = realm.Data! ,
                ClientId = clientId.Data!,
                ClientSecret = clientSecret.Data!   ,
                ClockSkew = clockSkew.Data is string clockSkewStr && int.TryParse(clockSkewStr, out var clockSkewOut) ? clockSkewOut : 5,
                RequireHttpsMetadata = requireHttpsMetadata.Data is not string requireHttpsMetadataStr || !bool.TryParse(requireHttpsMetadataStr, out var requireHttpsMetadataOut) || requireHttpsMetadataOut,
                ValidateAudience =  validateAudience.Data is not string validateAudienceStr || !bool.TryParse(validateAudienceStr, out var validateAudienceOut) || validateAudienceOut,
                ValidateIssuer = validateIssuer.Data is not string validateIssuerStr || !bool.TryParse(validateIssuerStr, out var validateIssuerOut) || validateIssuerOut,
                ValidateLifetime = validateLifetime.Data is not string validateLifetimeStr || !bool.TryParse(validateLifetimeStr, out var validateLifetimeOut) || validateLifetimeOut,
                ValidateTokenSignature = validateTokenSignature.Data is not string validateTokenSignatureStr || !bool.TryParse(validateTokenSignatureStr, out var validateTokenSignatureOut) || validateTokenSignatureOut
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
