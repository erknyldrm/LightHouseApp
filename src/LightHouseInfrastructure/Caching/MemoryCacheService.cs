using System;
using Microsoft.Extensions.Caching.Memory;

namespace LightHouseInfrastructure.Caching;

public class MemoryCacheService(IMemoryCache memoryCache) : ICacheService
{


    public Task<T?> GetAsync<T>(string key)
    {
        memoryCache.TryGetValue(key, out T? value);
       return Task.FromResult(value); 
    }

    public Task RemoveAsync<T>(string key)
    {
        memoryCache.Remove(key);
        return Task.CompletedTask;
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpireTime = null, TimeSpan? slidingExpireTime = null)
    {
        var options = new MemoryCacheEntryOptions();
        if (absoluteExpireTime.HasValue)
        {
            options.SetAbsoluteExpiration(absoluteExpireTime.Value);
        }
        if (slidingExpireTime.HasValue)
        {
            options.SetSlidingExpiration(slidingExpireTime.Value);
        }
        memoryCache.Set(key, value, options);
        return Task.CompletedTask;
    }
}
