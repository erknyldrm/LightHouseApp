using System;

namespace LightHouseDomain.Interfaces;

public interface ISecretManager
{
    Task<string?> GetSecretAsync(string secretPath, string secretKey, CancellationToken cancellationToken = default);
    Task<Dictionary<string, string>?> GetSecretsAsync(string secretPath, CancellationToken cancellationToken = default);
}
