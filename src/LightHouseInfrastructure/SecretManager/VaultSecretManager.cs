using System;
using LightHouseDomain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;

namespace LightHouseInfrastructure.SecretManager;

public class VaultSecretManager : ISecretManager
{
    private readonly VaultSettings _vaultSettings;
    private readonly ILogger<VaultSecretManager> _logger;
    private readonly IVaultClient _vaultClient;

    public VaultSecretManager(IConfiguration configuration, ILogger<VaultSecretManager> logger)
    {
        _logger = logger;
        _vaultSettings = new VaultSettings();
        configuration.GetSection("VaultSettings").Bind(_vaultSettings);

        _vaultClient = new VaultClient(new VaultClientSettings(_vaultSettings.Address, new TokenAuthMethodInfo(_vaultSettings.Token)));

        _logger.LogInformation("VaultSecretManager initialized with address: {Address}", _vaultSettings.Address);
    }

    public async Task<string?> GetSecretAsync(string secretPath, string secretKey, CancellationToken cancellationToken = default)
    {
        try
        {
            var secret = await _vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(path: secretPath, mountPoint: _vaultSettings.MountPoint);
            if (secret?.Data?.Data != null && secret.Data.Data.TryGetValue(secretKey, out var value))
            {
                return value?.ToString();
            }

            _logger.LogWarning("Secret key {SecretKey} not found in path {SecretPath}", secretKey, secretPath);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving secret {SecretKey} from path {SecretPath}", secretKey, secretPath);
            throw;
        }
    }

    public async Task<Dictionary<string, string>?> GetSecretsAsync(string secretPath, CancellationToken cancellationToken = default)
    {
        try
        {
            var secret = await _vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(path: secretPath, mountPoint: _vaultSettings.MountPoint);
            if (secret?.Data?.Data != null)
            {
                return secret.Data.Data.ToDictionary(kv => kv.Key, kv => kv.Value?.ToString() ?? string.Empty);
            }

            _logger.LogWarning("No secrets found in path {SecretPath}", secretPath);
            return [];
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving secrets from path {SecretPath}", secretPath);
            throw;
        }
    }
}
