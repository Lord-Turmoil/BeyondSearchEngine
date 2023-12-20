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
}