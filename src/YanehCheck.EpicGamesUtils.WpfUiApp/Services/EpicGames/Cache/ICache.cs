namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames.Cache;

public interface ICache
{
    T GetOrAdd<T>(string key, Func<T> valueFactory);
    void Remove(string key);
    void Clear();
}