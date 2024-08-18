using YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames.Results;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames.Interfaces;

public interface IEpicGamesService
{
    public Task<EpicGamesAuthResult> AuthenticateAccount(string authCode);
    public Task<EpicGamesDisplayNameResult> GetAccountInformation(string accountId, string accessToken);
    public Task<EpicGamesItemsResult> GetFortniteBrProfile(string accountId, string accessToken);
}