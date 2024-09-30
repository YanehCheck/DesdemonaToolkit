using YanehCheck.EpicGamesUtils.EgsApi.Api.Enums;
using YanehCheck.EpicGamesUtils.EgsApi.Api.Exceptions;
using YanehCheck.EpicGamesUtils.EgsApi.Api.Responses;

namespace YanehCheck.EpicGamesUtils.EgsApi.Api;

/// <summary>
/// <exception cref="EpicGamesApiException"></exception>
/// </summary>
public interface IEpicGamesClient {
    Task<ClientAuthResponse> Accounts_AuthenticateAsClient(AuthClientType clientType);
    Task<AccountAuthCodeAuthResponse> Accounts_AuthenticateAsAccount(AuthClientType clientType, string authCode);
    Task<AccountLookupResponse> Accounts_LookupAccountId(string accountId, string accessToken);
    Task<QueryProfileCommonCoreResponse> Fortnite_QueryCommonCoreProfile(string accountId, string accessToken);
    Task<QueryProfileAthenaResponse> Fortnite_QueryAthenaProfile(string accountId, string accessToken);
    Task<RedeemCodeAccountResponse> Fortnite_RedeemCodeAccount(string accountId, string accessToken, string code);
    Task<QueryProfileCommonCoreResponse> Accounts_SetSacCode(string accountId, string accessToken, string sacCode);
}