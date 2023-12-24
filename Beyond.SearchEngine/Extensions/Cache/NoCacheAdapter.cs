// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

namespace Beyond.SearchEngine.Extensions.Cache;

/// <summary>
///     Used locally when cache is not available.
/// </summary>
public class NoCacheAdapter : ICacheAdapter
{
    public void Set(string key, object value, TimeSpan? timeSpan = null)
    {
        // Do nothing...
    }

    public Task SetAsync(string key, object value, TimeSpan? timeSpan = null)
    {
        return Task.CompletedTask;
    }

    public TObject? Get<TObject>(string key) where TObject : class
    {
        return null;
    }

    public Task<TObject?> GetAsync<TObject>(string key) where TObject : class
    {
        return Task.FromResult<TObject?>(null);
    }

    public void Remove(string key)
    {
        // Nothing...
    }

    public Task RemoveAsync(string key)
    {
        return Task.CompletedTask;
    }

    public bool Exists(string key)
    {
        return false;
    }

    public Task<bool> ExistsAsync(string key)
    {
        return Task.FromResult(false);
    }

    public void SetInt(string key, long value, TimeSpan? timeSpan = null)
    {
        // Nothing...
    }

    public Task SetIntAsync(string key, long value, TimeSpan? timeSpan = null)
    {
        return Task.CompletedTask;
    }

    public long GetInt(string key, long defaultValue)
    {
        return defaultValue;
    }

    public Task<long> GetIntAsync(string key, long defaultValue)
    {
        return Task.FromResult(defaultValue);
    }
}