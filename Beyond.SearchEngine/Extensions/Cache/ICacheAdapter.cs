// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

namespace Beyond.SearchEngine.Extensions.Cache;

/// <summary>
///     Provide a high level interface for cache. With this, we can
///     easily switch between different cache implementations, and
///     enable us the ability of running even when cache is not available.
/// </summary>
public interface ICacheAdapter
{
    void Set(string key, object value, TimeSpan? timeSpan = null);
    Task SetAsync(string key, object value, TimeSpan? timeSpan = null);

    TObject? Get<TObject>(string key) where TObject : class;
    Task<TObject?> GetAsync<TObject>(string key) where TObject : class;

    void Remove(string key);
    Task RemoveAsync(string key);

    bool Exists(string key);
    Task<bool> ExistsAsync(string key);

    void SetInt(string key, long value, TimeSpan? timeSpan = null);
    Task SetIntAsync(string key, long value, TimeSpan? timeSpan = null);

    long GetInt(string key, long defaultValue);
    Task<long> GetIntAsync(string key, long defaultValue);
}