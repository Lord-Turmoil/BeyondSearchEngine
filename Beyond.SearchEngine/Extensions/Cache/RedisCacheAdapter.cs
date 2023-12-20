using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Beyond.SearchEngine.Extensions.Cache;

/// <summary>
///     Provide cache with Redis.
/// </summary>
public class RedisCacheAdapter : ICacheAdapter
{
    private readonly IDistributedCache _cache;

    public RedisCacheAdapter(IDistributedCache cache)
    {
        _cache = cache;
    }

    public void Set(string key, object value, TimeSpan? timeSpan = null)
    {
        if (timeSpan == null)
        {
            _cache.SetString(key, JsonConvert.SerializeObject(value));
        }
        else
        {
            _cache.SetString(key, JsonConvert.SerializeObject(value), new DistributedCacheEntryOptions {
                AbsoluteExpirationRelativeToNow = timeSpan
            });
        }
    }

    public Task SetAsync(string key, object value, TimeSpan? timeSpan = null)
    {
        if (timeSpan == null)
        {
            return _cache.SetStringAsync(key, JsonConvert.SerializeObject(value));
        }

        return _cache.SetStringAsync(key, JsonConvert.SerializeObject(value), new DistributedCacheEntryOptions {
            AbsoluteExpirationRelativeToNow = timeSpan
        });
    }

    public TObject? Get<TObject>(string key) where TObject : class
    {
        string? value = _cache.GetString(key);
        return value == null ? null : JsonConvert.DeserializeObject<TObject>(value);
    }

    public async Task<TObject?> GetAsync<TObject>(string key) where TObject : class
    {
        string? value = await _cache.GetStringAsync(key);
        return value == null ? null : JsonConvert.DeserializeObject<TObject>(value);
    }
}