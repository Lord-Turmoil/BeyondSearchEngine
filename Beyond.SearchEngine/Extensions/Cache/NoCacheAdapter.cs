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
}