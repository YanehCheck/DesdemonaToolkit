using YanehCheck.EpicGamesUtils.Utils.EgApiWrapper.Auth;
using YanehCheck.EpicGamesUtils.Utils.EgApiWrapper.Enums;
using YanehCheck.EpicGamesUtils.Utils.EgApiWrapper.Results;

namespace YanehCheck.EpicGamesUtils.Utils.EgApiWrapper;

public interface IEpicGamesClient {
    public Task<ApiResult> Accounts_AuthenticateAsClient(AuthClientType clientType);
    public Task<ApiResult> Accounts_AuthenticateAsAccount(AuthClientType clientType, string authCode);
    public Task<ApiResult> Accounts_LookupAccountId(string accountId, string accessToken);
    public Task<ApiResult> Fortnite_GetFounderCodes(FounderCodePlatform platform, string accountId, string accessToken);
    public Task<ApiResult> Fortnite_QueryProfile(string accountId, string accessToken, FortniteProfile profile);
    public Task<ApiResult> Accounts_SetSacCode(string accountId, string accessToken, string sacCode);
}