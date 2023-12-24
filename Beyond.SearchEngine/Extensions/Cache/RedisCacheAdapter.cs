// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

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
        SetString(key, JsonConvert.SerializeObject(value), timeSpan);
    }

    public Task SetAsync(string key, object value, TimeSpan? timeSpan = null)
    {
        return SetStringAsync(key, JsonConvert.SerializeObject(value), timeSpan);
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

    public void SetString(string key, string value, TimeSpan? timeSpan = null)
    {
        _cache.SetString(key, value, new DistributedCacheEntryOptions {
            AbsoluteExpirationRelativeToNow = timeSpan ?? _timeout
        });
    }

    public Task SetStringAsync(string key, string value, TimeSpan? timeSpan = null)
    {
        return _cache.SetStringAsync(key, value, new DistributedCacheEntryOptions {
            AbsoluteExpirationRelativeToNow = timeSpan ?? _timeout
        });
    }

    public string? GetString(string key)
    {
        return _cache.GetString(key);
    }

    public Task<string?> GetStringAsync(string key)
    {
        return _cache.GetStringAsync(key);
    }

    public void SetInt(string key, long value, TimeSpan? timeSpan = null)
    {
        _cache.Set(key, BitConverter.GetBytes(value), new DistributedCacheEntryOptions {
            AbsoluteExpirationRelativeToNow = timeSpan ?? _timeout
        });
    }

    public Task SetIntAsync(string key, long value, TimeSpan? timeSpan = null)
    {
        return _cache.SetAsync(key, BitConverter.GetBytes(value), new DistributedCacheEntryOptions {
            AbsoluteExpirationRelativeToNow = timeSpan ?? _timeout
        });
    }

    public long GetInt(string key, long defaultValue)
    {
        byte[]? bytes = _cache.Get(key);
        return bytes == null ? defaultValue : BitConverter.ToInt64(bytes);
    }

    public async Task<long> GetIntAsync(string key, long defaultValue)
    {
        byte[]? bytes = await _cache.GetAsync(key);
        return bytes == null ? defaultValue : BitConverter.ToInt64(bytes);
    }

    public void Remove(string key)
    {
        _cache.Remove(key);
    }

    public Task RemoveAsync(string key)
    {
        return _cache.RemoveAsync(key);
    }

    public bool Exists(string key)
    {
        return _cache.GetString(key) != null;
    }

    public async Task<bool> ExistsAsync(string key)
    {
        return await _cache.GetStringAsync(key) != null;
    }
}