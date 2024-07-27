using YanehCheck.EpicGamesUtils.Api;
using YanehCheck.EpicGamesUtils.Api.Auth;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames.Results;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames;

public class EpicGamesService(IEpicGamesClient epicGamesClient) : IEpicGamesService {
    public async Task<EpicGamesAuthResult> AuthenticateAccount(string authCode) {
        var result = await epicGamesClient.Accounts_AuthenticateAsAccount(AuthClientType.FortnitePcGameClient, authCode);
        if (result.Success) {
            // TODO: Validate JSON structure just to be sure
            return new EpicGamesAuthResult(
                result.StatusCode,
                result.Content!.RootElement.GetProperty("account_id").ToString(),
                result.Content!.RootElement.GetProperty("access_token").ToString(),
                DateTime.Parse(result.Content!.RootElement.GetProperty("expires_at").ToString())
                );
        }

        if (result.StatusCode != 0) {
            return new EpicGamesAuthResult(result.StatusCode,
                errorMessage: result.Content!.RootElement.GetProperty("errorMessage").ToString());
        }

        return new EpicGamesAuthResult(result.StatusCode,
            errorMessage: "Can not contact the Epic Games API.");
    }

    public async Task<EpicGamesGetByAccountIdResult> LookupAccountId(string accountId, string accessToken) {
        var result = await epicGamesClient.Accounts_LookupAccountId(accountId, accessToken);
        if (result.Success) {
            return new EpicGamesGetByAccountIdResult(
                result.StatusCode,
                result.Content!.RootElement.GetProperty("displayName").ToString()
            );
        }

        if(result.StatusCode != 0) {
            return new EpicGamesGetByAccountIdResult(result.StatusCode,
                errorMessage: result.Content!.RootElement.GetProperty("errorMessage").ToString());
        }

        return new EpicGamesGetByAccountIdResult(result.StatusCode,
            errorMessage: "Can not contact the Epic Games API.");
    }
}