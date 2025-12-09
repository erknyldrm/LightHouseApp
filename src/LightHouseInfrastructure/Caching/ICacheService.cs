using System;
using LightHouseApplication.Common;

namespace LightHouseInfrastructure.Caching;

public interface ICacheService
{
    Task<Result<T?>> GetAsync<T>(string key);
    Task<Result> SetAsync<T>(string key, T value, TimeSpan? absoluteExpireTime = null, TimeSpan? slidingExpireTime = null);
    Task<Result> RemoveAsync<T>(string key);
}
