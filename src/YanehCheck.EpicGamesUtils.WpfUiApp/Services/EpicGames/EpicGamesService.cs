using System.Net;
using System.Text.RegularExpressions;
using YanehCheck.EpicGamesUtils.Utils.EgApiWrapper;
using YanehCheck.EpicGamesUtils.Utils.EgApiWrapper.Auth;
using YanehCheck.EpicGamesUtils.Utils.EgApiWrapper.Enums;
using YanehCheck.EpicGamesUtils.Utils.EgApiWrapper.Results;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames.Results;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames;

public class EpicGamesService(IEpicGamesClient epicGamesClient) : IEpicGamesService
{
    public async Task<EpicGamesAuthResult> AuthenticateAccount(string authCode)
    {
        var result = await epicGamesClient.Accounts_AuthenticateAsAccount(AuthClientType.FortnitePcGameClient, authCode);
        if (!result.Success)
        {
            var errorParams = HandleError(result);
            return new EpicGamesAuthResult(errorParams.Item1, errorMessage: errorParams.Item2);
        }

        return new EpicGamesAuthResult(
                result.StatusCode,
                (string)result.Content!["account_id"]!,
                (string)result.Content!["access_token"]!,
                // Newtonsoft throws out timezone info on default, so let's do this for now
                (DateTime)result.Content!["expires_at"]! + DateTimeOffset.Now.Offset,
                (string)result.Content!["displayName"]!
                );
    }

    public async Task<EpicGamesDisplayNameResult> GetAccountInformation(string accountId, string accessToken)
    {
        var result = await epicGamesClient.Accounts_LookupAccountId(accountId, accessToken);
        if (!result.Success)
        {
            var errorParams = HandleError(result);
            return new EpicGamesDisplayNameResult(errorParams.Item1, errorMessage: errorParams.Item2);
        }

        return new EpicGamesDisplayNameResult(
            result.StatusCode,
            (string)result.Content!["displayName"]!
        );
    }

    public async Task<EpicGamesItemsResult> GetFortniteBrProfile(string accountId, string accessToken)
    {
        var result = await epicGamesClient.Fortnite_QueryProfile(accountId, accessToken, FortniteProfile.Athena);
        if (!result.Success)
        {
            var errorParams = HandleError(result);
            return new EpicGamesItemsResult(errorParams.Item1, errorMessage: errorParams.Item2);
        }

        // Able to only write JSON path that does include unobtained things from current Festival pass
        // (and probably other things in future). The applied regex should result in actually owned things
        // (be it items, or some other clutter like quests and passes)
        var itemTokens = result.Content!.SelectTokens("$..templateId");
        var filteredItemTokens = itemTokens.Where(t => Regex.IsMatch(t.Path, @"profileChanges\[0]\.profile\.items\..{36}\.templateId"));

        var items = filteredItemTokens.Select(x => x.ToObject<string>());
        var removeFilter = EpicGamesServiceHelpers.GetQueryProfileRemoveFilter();
        var filteredItems = items.Where(i => !removeFilter.Any(i.StartsWith));

        var builtinEmotesTokens = result.Content!.SelectTokens("$..access_item");
        var filteredBuiltinEmotes = builtinEmotesTokens.Select(x => x.ToObject<string>());

        // Now banners from common_core
        var bannerResult = await epicGamesClient.Fortnite_QueryProfile(accountId, accessToken, FortniteProfile.CommonCore);
        var bannerTokens = bannerResult.Content!.SelectTokens("$..templateId");
        var filteredBannerTokens = bannerTokens.Where(t => Regex.IsMatch(t.Path, @"profileChanges\[0]\.profile\.items\..{36}\.templateId"));
        var banners = filteredBannerTokens.Select(x => x.ToObject<string>());
        var filteredBanners = banners.Where(i => !removeFilter.Any(i.StartsWith));

        var allItems = filteredBanners.Concat(filteredItems).Concat(filteredBuiltinEmotes);

        return new EpicGamesItemsResult(
            result.StatusCode,
            allItems.Select(i => new EpicGamesApiItem(i!))
        );
    }

    private (HttpStatusCode, string) HandleError(ApiResult result)
    {
        return result.StatusCode != 0 ?
            (result.StatusCode, (string)result.Content!["errorMessage"]!) :
            (result.StatusCode, "Can not contact the Epic Games API.");
    }
}