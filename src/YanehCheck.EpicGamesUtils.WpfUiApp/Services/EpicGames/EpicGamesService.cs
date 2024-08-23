using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using YanehCheck.EpicGamesUtils.Utils.EgApiWrapper;
using YanehCheck.EpicGamesUtils.Utils.EgApiWrapper.Auth;
using YanehCheck.EpicGamesUtils.Utils.EgApiWrapper.Enums;
using YanehCheck.EpicGamesUtils.Utils.EgApiWrapper.Results;
using YanehCheck.EpicGamesUtils.WpfUiApp.Models;
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

    public async Task<EpicGamesItemsResult> GetFortniteProfile(string accountId, string accessToken)
    {
        var athenaResult = await epicGamesClient.Fortnite_QueryProfile(accountId, accessToken, FortniteProfile.Athena);
        if (!athenaResult.Success)
        {
            var errorParams = HandleError(athenaResult);
            return new EpicGamesItemsResult(errorParams.Item1, errorMessage: errorParams.Item2);
        }
        var commonCoreResult = await epicGamesClient.Fortnite_QueryProfile(accountId, accessToken, FortniteProfile.CommonCore);
        if(!commonCoreResult.Success) {
            var errorParams = HandleError(commonCoreResult);
            return new EpicGamesItemsResult(errorParams.Item1, errorMessage: errorParams.Item2);
        }

        var items = ExtractAthenaItems(athenaResult);
        var banners = ExtractBanners(commonCoreResult);

        var allItems = banners.Concat(items);

        return new EpicGamesItemsResult(
            athenaResult.StatusCode,
            allItems
        );
    }

    private IEnumerable<EpicGamesApiItem> ExtractAthenaItems(ApiResult athenaResult) {
        var removeFilter = EpicGamesServiceHelpers.GetQueryProfileRemoveFilter();

        var itemTokens = athenaResult.Content!.SelectTokens("$..templateId");
        var filteredItemTokens = itemTokens.Where(t => Regex.IsMatch(t.Path, @"profileChanges\[0]\.profile\.items\..{36}\.templateId"));

        var items = filteredItemTokens.Select(x => {
            var itemId = x.ToObject<string>()!;
            var variants = ExtractStyles(x.Parent!.Parent!);
            return (Id: itemId, Styles: variants);
        });

        var filteredItems = items.Where(i => !removeFilter.Any(i.Id.StartsWith));

        var builtinEmotesTokens = athenaResult.Content!.SelectTokens("$..access_item");
        var filteredBuiltinEmotes = builtinEmotesTokens.Select(x => x.ToObject<string>());

        return filteredItems.Select(i => new EpicGamesApiItem(i.Id, i.Styles))
            .Concat(filteredBuiltinEmotes.Select(i => new EpicGamesApiItem(i!)));

        IEnumerable<ItemStyleRaw> ExtractStyles(JToken itemToken) {
            var variants = itemToken.SelectTokens("..variants").Children()
                .Where(v => v["owned"] != null && v["owned"]!.Any());

            var styles = variants.Select(v => 
                new ItemStyleRaw(v["channel"]!.ToObject<string>()!, v["owned"]!.ToObject<List<string>>()!)
            );
            return styles;
        }
    }

    private IEnumerable<EpicGamesApiItem> ExtractBanners(ApiResult commonCoreResult) {
        IEnumerable<string> removeFilter = EpicGamesServiceHelpers.GetQueryProfileRemoveFilter();

        var bannerTokens = commonCoreResult.Content!.SelectTokens("$..templateId");
        var filteredBannerTokens = bannerTokens.Where(t => Regex.IsMatch(t.Path, @"profileChanges\[0]\.profile\.items\..{36}\.templateId"));

        var banners = filteredBannerTokens.Select(x => x.ToObject<string>());
        var filteredBanners = banners.Where(i => !removeFilter.Any(i.StartsWith));
        return filteredBanners.Select(i => new EpicGamesApiItem(i!));
    }

    private (HttpStatusCode, string) HandleError(ApiResult result)
    {
        return result.StatusCode != 0 ?
            (result.StatusCode, (string)result.Content!["errorMessage"]!) :
            (result.StatusCode, "Can not contact the Epic Games API.");
    }
}