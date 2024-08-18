namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames.Interfaces;

public interface ICachedEpicGamesService : IEpicGamesService
{
    void InvalidateAll();
    void Invalidate(string method);
    Task PreCacheAll(string accountId, string accessToken);
}