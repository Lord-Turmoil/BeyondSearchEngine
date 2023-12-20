using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Beyond.SearchEngine.Extensions.Cache;

/// <summary>
///     Provide cache with Redis.
/// </summary>
public class RedisCacheAdapter : ICacheAdapter
{
    private readonly IDistributedCache _cache;
    private readonly TimeSpan _timeout;

    public RedisCacheAdapter(IDistributedCache cache, IConfiguration configuration)
    {
        _cache = cache;

        var option = new CacheOptions();
        configuration.GetRequiredSection(CacheOptions.CacheSection).Bind(option);
        _timeout = TimeSpan.FromMinutes(option.TimeoutInMinutes);
    }

    public void Set(string key, object value, TimeSpan? timeSpan = null)
    {
        _cache.SetString(key, JsonConvert.SerializeObject(value), new DistributedCacheEntryOptions {
            AbsoluteExpirationRelativeToNow = timeSpan ?? _timeout
        });
    }

    public Task SetAsync(string key, object value, TimeSpan? timeSpan = null)
    {
        return _cache.SetStringAsync(key, JsonConvert.SerializeObject(value), new DistributedCacheEntryOptions {
            AbsoluteExpirationRelativeToNow = timeSpan ?? _timeout
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