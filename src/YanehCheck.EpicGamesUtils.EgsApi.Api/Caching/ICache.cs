namespace YanehCheck.EpicGamesUtils.EgsApi.Api.Caching;

public interface ICache<in TKey, TValue>
{
    void Cache(TKey key, TValue value);
    void Invalidate(TKey key);
    void Clear();
    TValue Get(TKey key);
    void Precache(TKey key, TValue value);
}