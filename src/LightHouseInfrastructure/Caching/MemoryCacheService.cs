using System;
using LightHouseApplication.Common;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace LightHouseInfrastructure.Caching;

public class MemoryCacheService(IMemoryCache memoryCache, ILogger<MemoryCacheService> logger) : ICacheService
{
    public Task<Result<T?>> GetAsync<T>(string key)
    {
        try
        {
            if (memoryCache.TryGetValue(key, out var value))
            {
                logger.LogDebug("Cache hit for key: {Key}", key);
                return Task.FromResult(Result<T?>.Ok((T?)value));
            }

            logger.LogDebug("Cache miss for key: {Key}", key);
            return Task.FromResult(Result<T?>.Ok(default(T?)));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting cache item with key '{Key}'", key);
            return Task.FromResult(Result<T?>.Fail(ex.Message));
        }
    }

    public Task<Result> RemoveAsync<T>(string key)
    {
        try
        {
            memoryCache.Remove(key);
            logger.LogDebug("Cache key removed: {Key}", key);
            return Task.FromResult(Result.Ok());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception occurred while removing value from memory cache. Key: {Key}", key);
            return Task.FromResult(Result.Fail($"Failed to remove cache value: {ex.Message}"));
        }

    }

    public Task<Result> SetAsync<T>(string key, T value, TimeSpan? absoluteExpireTime = null, TimeSpan? slidingExpireTime = null)
    {
        try
        {
            if (absoluteExpireTime.HasValue)
            {
                memoryCache.Set(key, value, absoluteExpireTime.Value);
            }
            else
            {
                memoryCache.Set(key, value);
            }

            logger.LogDebug("Cache value set for key: {Key}, Expiration: {Expiration}", key, absoluteExpireTime);
            return Task.FromResult(Result.Ok());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception occurred while setting value to memory cache. Key: {Key}", key);
            return Task.FromResult(Result.Fail($"Failed to set cache value: {ex.Message}"));
        }
    }
}
