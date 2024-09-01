using YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames.Results;
using YanehCheck.EpicGamesUtils.WpfUiApp.Utilities.Cache;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames;

public class CachedEpicGamesService(EpicGamesService epicGamesService, ICache cache) : ICached, ICachedEpicGamesService {
    public Task<EpicGamesAuthResult> AuthenticateAccount(string authCode) =>
        epicGamesService.AuthenticateAccount(authCode);

    public Task<EpicGamesDisplayNameResult> GetAccountInformation(string accountId, string accessToken) {
        return cache.GetOrAdd(nameof(GetAccountInformation),
            () => epicGamesService.GetAccountInformation(accountId, accessToken));
    }

    public Task<EpicGamesItemsResult> GetFortniteProfile(string accountId, string accessToken) {
        return cache.GetOrAdd(nameof(GetFortniteProfile),
            () => epicGamesService.GetFortniteProfile(accountId, accessToken));
    }

    public Task<EpicGamesSetSacCodeResult> SetSacCode(string accountId, string accessToken, string sacCode) =>
        epicGamesService.SetSacCode(accountId, accessToken, sacCode);

    public void InvalidateAll() => cache.Clear();

    public void Invalidate(string method) => cache.Remove(method);

    public async Task PreCacheAll(string accountId, string accessToken) {
        await GetFortniteProfile(accountId, accessToken);
    }
}