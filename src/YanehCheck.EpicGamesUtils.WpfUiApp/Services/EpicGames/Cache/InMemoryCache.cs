using System.Collections.Concurrent;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames.Cache;

public class InMemoryCache : ICache
{
    private readonly ConcurrentDictionary<string, object> cache = new();

    public T GetOrAdd<T>(string key, Func<T> valueFactory)
    {
        return (T)cache.GetOrAdd(key, _ => valueFactory());
    }

    public void Remove(string key)
    {
        cache.TryRemove(key, out _);
    }

    public void Clear()
    {
        cache.Clear();
    }
}