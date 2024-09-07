using YanehCheck.EpicGamesUtils.EgsApi.Api;
using YanehCheck.EpicGamesUtils.EgsApi.Api.Enums;
using YanehCheck.EpicGamesUtils.EgsApi.Api.Responses.DataObjects;
using YanehCheck.EpicGamesUtils.EgsApi.Api.Responses.Interfaces;
using YanehCheck.EpicGamesUtils.EgsApi.Service.Dtos;
using YanehCheck.EpicGamesUtils.EgsApi.Service.Helpers;
using YanehCheck.EpicGamesUtils.EgsApi.Service.Mappers;
using Attribute = YanehCheck.EpicGamesUtils.EgsApi.Api.Responses.DataObjects.Attribute;

namespace YanehCheck.EpicGamesUtils.EgsApi.Service;

public class EpicGamesService(ICachedEpicGamesClient client) : IEpicGamesService {
    private readonly ItemMapper itemMapper = new();

    public async Task PrecacheResults(string accountId, string accessToken) {
        await client.PrecacheAsync(() => Task.FromResult(client.Fortnite_QueryAthenaProfile(accountId, accessToken).Result as IResponse), nameof(client.Fortnite_QueryAthenaProfile));
        await client.PrecacheAsync(() => Task.FromResult(client.Fortnite_QueryCommonCoreProfile(accountId, accessToken).Result as IResponse), nameof(client.Fortnite_QueryCommonCoreProfile));
    }

    public async Task<AccountAuthTokenResult> AuthenticateAccountUsingAuthCode(string authcode) {
        var response = await client.Accounts_AuthenticateAsAccount(AuthClientType.FortnitePcGameClient, authcode);
        return new AccountAuthTokenResult(
            response.AccountId,
            response.AccessToken,
            DateTime.Now + TimeSpan.FromSeconds(response.ExpiresIn),
            response.DisplayName);
    }

    public async Task UseSacCode(string accountId, string accessToken, string sacCode) {
        var response = await client.Accounts_SetSacCode(accountId, accessToken, sacCode);
    }

    public async Task<FortniteItemsResult> GetFortniteItems(string accountId, string accessToken) {
        var commonCoreResponse = await client.Fortnite_QueryCommonCoreProfile(accountId, accessToken);
        var athenaResponse = await client.Fortnite_QueryAthenaProfile(accountId, accessToken);

        var removeFilter = FortniteItemTypeFilter.GetQueryProfileRemoveFilter();

        var banners = commonCoreResponse.Items.Where(i => !removeFilter.Any(i.FortniteId.StartsWith));
        var builtinEmotes = athenaResponse.Items.Where(i => i.Attributes.GrantAccessToItem != null)
            .Select(i => new Item {
                FortniteId = i.Attributes.GrantAccessToItem!,
                Attributes = new Attribute(),
                Quantity = 1
            });
        var otherItems = athenaResponse.Items.Where(i => !removeFilter.Any(i.FortniteId.StartsWith));
        var allItems = banners.Concat(builtinEmotes).Concat(otherItems);
        return new FortniteItemsResult(allItems.Select(itemMapper.MapFromItem).ToList());
    }

}