using System;
using Microsoft.Extensions.Caching.Distributed;

namespace LightHouseInfrastructure.Caching;

public class RedisCacheService(IDistributedCache distributedCache) : ICacheService
{
    private readonly IDistributedCache _distributedCache = distributedCache;

    public async Task<T?> GetAsync<T>(string key)
    {
        var json = await _distributedCache.GetStringAsync(key);
        if (json is null)
            return default;

        return System.Text.Json.JsonSerializer.Deserialize<T>(json);
    }

    public async Task RemoveAsync<T>(string key)
    {
        await _distributedCache.RemoveAsync(key);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpireTime = null, TimeSpan? slidingExpireTime = null)
    {
        var options = new DistributedCacheEntryOptions();
        if (absoluteExpireTime.HasValue)
            options.SetAbsoluteExpiration(absoluteExpireTime.Value);

        if (slidingExpireTime.HasValue)
            options.SetSlidingExpiration(slidingExpireTime.Value);

        var json = System.Text.Json.JsonSerializer.Serialize(value);
        await _distributedCache.SetStringAsync(key, json, options);
    }
}
