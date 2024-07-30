using YanehCheck.EpicGamesUtils.Api.Auth;
using YanehCheck.EpicGamesUtils.Api.Enums;
using YanehCheck.EpicGamesUtils.Api.Results;

namespace YanehCheck.EpicGamesUtils.Api;

public interface IEpicGamesClient {
    public Task<ApiResult> Accounts_AuthenticateAsClient(AuthClientType clientType);
    public Task<ApiResult> Accounts_AuthenticateAsAccount(AuthClientType clientType, string authCode);
    public Task<ApiResult> Accounts_LookupAccountId(string accountId, string accessToken);
    public Task<ApiResult> Fortnite_GetFounderCodes(FounderCodePlatform platform, string accountId, string accessToken);
    public Task<ApiResult> Fortnite_QueryProfile(string accountId, string accessToken, FortniteProfile profile);
}