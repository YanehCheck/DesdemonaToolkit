using System.Net;
using YanehCheck.EpicGamesUtils.Api;
using YanehCheck.EpicGamesUtils.Api.Auth;
using YanehCheck.EpicGamesUtils.Api.Results;
using YanehCheck.EpicGamesUtils.WpfUiApp.Models;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames.Results;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames;

public class EpicGamesService(IEpicGamesClient epicGamesClient) : IEpicGamesService {
    public async Task<EpicGamesAuthResult> AuthenticateAccount(string authCode) {
        var result = await epicGamesClient.Accounts_AuthenticateAsAccount(AuthClientType.FortnitePcGameClient, authCode);
        if(!result.Success) {
            var errorParams = HandleError(result);
            return new EpicGamesAuthResult(errorParams.Item1, errorMessage: errorParams.Item2);
        }

        return new EpicGamesAuthResult(
                result.StatusCode,
                (string) result.Content!["account_id"]!,
                (string) result.Content!["access_token"]!,
                // Newtonsoft throws out timezone info on default, so let's do this for now
                (DateTime) result.Content!["expires_at"]! + DateTimeOffset.Now.Offset,
                (string) result.Content!["displayName"]!
                );
    }

    public async Task<EpicGamesDisplayNameResult> GetDisplayName(string accountId, string accessToken) {
        var result = await epicGamesClient.Accounts_LookupAccountId(accountId, accessToken);
        if(!result.Success) {
            var errorParams = HandleError(result);
            return new EpicGamesDisplayNameResult(errorParams.Item1, errorMessage: errorParams.Item2);
        }

        return new EpicGamesDisplayNameResult(
            result.StatusCode,
            (string) result.Content!["displayName"]!
        );
    }

    public async Task<EpicGamesItemsResult> GetItems(string accountId, string accessToken) {
        var result = await epicGamesClient.Fortnite_QueryProfile(accountId, accessToken);
        if (!result.Success) {
            var errorParams = HandleError(result);
            return new EpicGamesItemsResult(errorParams.Item1, errorMessage: errorParams.Item2);
        }

        var items = result.Content!.SelectTokens("$..templateId").Select(x => x.ToObject<string>());
        var removeFilter = EpicGamesServiceHelpers.GetQueryProfileRemoveFilter();

        var filteredItems = items.Where(i => !removeFilter.Any(i.StartsWith));
        return new EpicGamesItemsResult(
            result.StatusCode,
            filteredItems.Select(i => new EpicGamesItem(i!))
        );
    }

    private (HttpStatusCode, string) HandleError(ApiResult result) {
        return result.StatusCode != 0 ? 
            (result.StatusCode, (string) result.Content!["errorMessage"]!) : 
            (result.StatusCode, "Can not contact the Epic Games API.");
    }
}