using YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames.Cache;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames.Results;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames;

public class CachedEpicGamesService(EpicGamesService epicGamesService, ICache cache) : IEpicGamesService, ICached {
    public Task<EpicGamesAuthResult> AuthenticateAccount(string authCode) {
        return cache.GetOrAdd(nameof(AuthenticateAccount), 
            () => epicGamesService.AuthenticateAccount(authCode));
    }

    public Task<EpicGamesDisplayNameResult> GetAccountInformation(string accountId, string accessToken) {
        return cache.GetOrAdd(nameof(GetAccountInformation), 
            () => epicGamesService.GetAccountInformation(accountId, accessToken));
    }

    public Task<EpicGamesItemsResult> GetFortniteBrProfile(string accountId, string accessToken) {
        return cache.GetOrAdd(nameof(GetFortniteBrProfile), 
            () => epicGamesService.GetFortniteBrProfile(accountId, accessToken));
    }

    public void InvalidateCache() => cache.Clear();

    public void Invalidate(string methodName) => cache.Remove(methodName);
}