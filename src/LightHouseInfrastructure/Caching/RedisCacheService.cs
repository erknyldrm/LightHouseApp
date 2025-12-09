using System;
using System.Text.Json;
using LightHouseApplication.Common;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace LightHouseInfrastructure.Caching;

public class RedisCacheService(IConnectionMultiplexer connectionMultiplexer, ILogger<RedisCacheService> logger)
    : ICacheService
{
    private readonly IDatabase _database = connectionMultiplexer.GetDatabase();
    private readonly ILogger<RedisCacheService> logger = logger;

    public async Task<Result<T?>> GetAsync<T>(string key)
    {
        try
        {
            var json = await _database.StringGetAsync(key);
            if (string.IsNullOrEmpty(json))
            {
                logger.LogDebug("Cache miss for key: {Key}", key);
                return Result<T?>.Ok(default);
            }

            var value = JsonSerializer.Deserialize<T>(json!);
            logger.LogDebug("Cache hit for key: {Key}", key);
            return Result<T?>.Ok(value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception occurred while getting value from Redis cache. Key: {Key}", key);
            return Result<T?>.Fail($"Failed to get cache value: {ex.Message}");
        }
    }

    public async Task<Result> RemoveAsync<T>(string key)
    {
        try
        {
            await _database.KeyDeleteAsync(key);
            logger.LogDebug("Cache key removed: {Key}", key);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception occurred while removing value from Redis cache. Key: {Key}", key);
            return Result.Fail($"Failed to remove cache value: {ex.Message}");
        }
    }

    public async Task<Result> SetAsync<T>(string key, T value, TimeSpan? absoluteExpireTime = null, TimeSpan? slidingExpireTime = null)
    {
        try
        {
            var json = JsonSerializer.Serialize(value);
            await _database.StringSetAsync(key, json, absoluteExpireTime);
            logger.LogDebug("Cache value set for key: {Key}, Expiration: {Expiration}", key, absoluteExpireTime);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception occurred while setting value to Redis cache. Key: {Key}", key);
            return Result.Fail($"Failed to set cache value: {ex.Message}");
        }
    }
}
