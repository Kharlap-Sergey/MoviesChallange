using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace ApiApplication.Infrastructure;

public static class IDistributedCacheExtensions
{
    public static async Task<T> GetOrSet<T>(
        this IDistributedCache cache, 
        string key, 
        Func<Task<T>> getItem, 
        int expirationInMinutes)
    {
        var cachedItem = await cache.GetStringAsync(key);
        if (cachedItem != null)
        {
            return JsonSerializer.Deserialize<T>(cachedItem);
        }

        var item = await getItem();
        var serializedItem = JsonSerializer.Serialize(item);
        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expirationInMinutes)
        };
        await cache.SetStringAsync(key, serializedItem, cacheOptions);

        return item;
    }
}
