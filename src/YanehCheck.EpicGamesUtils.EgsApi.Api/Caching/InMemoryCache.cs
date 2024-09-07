using System.Collections.Concurrent;

namespace YanehCheck.EpicGamesUtils.EgsApi.Api.Caching;

public class InMemoryCache<TKey, TValue> : ICache<TKey, TValue> where TKey : notnull {
    private readonly ConcurrentDictionary<TKey, (TValue Value, int RemainingFetches)> cache = new();
    private const int DefaultExpiration = 999999;

    public void Cache(TKey key, TValue value) {
        cache[key] = (value, DefaultExpiration);
    }

    public void Invalidate(TKey key) {
        cache.TryRemove(key, out _);
    }

    public void Clear() {
        cache.Clear();
    }

    public TValue Get(TKey key) {
        if(cache.TryGetValue(key, out var cacheEntry)) {
            if(cacheEntry.RemainingFetches > 0) {
                cache[key] = (cacheEntry.Value, cacheEntry.RemainingFetches - 1);
                return cacheEntry.Value;
            }
            else {
                cache.TryRemove(key, out _);
            }
        }

        throw new KeyNotFoundException($"The key '{key}' was not found in the cache or has expired.");
    }

    public void Precache(TKey key, TValue value) {
        cache[key] = (value, 1);
    }

    public bool TryGet(TKey key, out TValue value) {
        if(cache.TryGetValue(key, out var cacheEntry) && cacheEntry.RemainingFetches > 0) {
            cache[key] = (cacheEntry.Value, cacheEntry.RemainingFetches - 1);
            value = cacheEntry.Value;
            return true;
        }

        if(cacheEntry.RemainingFetches == 0) {
            cache.TryRemove(key, out _);
        }

        value = default;
        return false;
    }

    public void Cache(TKey key, TValue value, int expiration) {
        cache[key] = (value, expiration);
    }
}