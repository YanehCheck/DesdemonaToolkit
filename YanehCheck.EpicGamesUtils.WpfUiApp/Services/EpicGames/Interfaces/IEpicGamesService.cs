using YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames.Results;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames.Interfaces;

public interface IEpicGamesService {
    public Task<EpicGamesAuthResult> AuthenticateAccount(string authCode);
}