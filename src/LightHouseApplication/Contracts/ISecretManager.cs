using System;
using LightHouseApplication.Common;

namespace LightHouseApplication.Contracts;

public interface ISecretManager
{
    Task<Result<string?>> GetSecretAsync(string secretPath, string secretKey, CancellationToken cancellationToken = default);
    Task<Result<Dictionary<string, string>?>> GetSecretsAsync(string secretPath, CancellationToken cancellationToken = default);
}
