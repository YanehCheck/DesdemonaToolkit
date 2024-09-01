using YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames.Results;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames.Interfaces;

public interface IEpicGamesService
{
    Task<EpicGamesAuthResult> AuthenticateAccount(string authCode);
    Task<EpicGamesDisplayNameResult> GetAccountInformation(string accountId, string accessToken);
    Task<EpicGamesItemsResult> GetFortniteProfile(string accountId, string accessToken);
    Task<EpicGamesSetSacCodeResult> SetSacCode(string accountId, string accessToken, string sacCode);
}