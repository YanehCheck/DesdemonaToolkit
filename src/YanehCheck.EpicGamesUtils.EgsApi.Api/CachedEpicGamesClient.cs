using RestSharp;
using YanehCheck.EpicGamesUtils.EgsApi.Api.Caching;
using YanehCheck.EpicGamesUtils.EgsApi.Api.Enums;
using YanehCheck.EpicGamesUtils.EgsApi.Api.Responses;
using YanehCheck.EpicGamesUtils.EgsApi.Api.Responses.Interfaces;

namespace YanehCheck.EpicGamesUtils.EgsApi.Api;

public class CachedEpicGamesClient(IRestClient client) : EpicGamesClient(client), ICachedEpicGamesClient {
    private readonly InMemoryCache<string, IResponse> cache = new();

    public async Task PrecacheAsync(Func<Task<IResponse>> methodCall, string methodName) {
        var response = await methodCall();
        cache.Precache(methodName, response);
    }

    public override async Task<ClientAuthResponse> Accounts_AuthenticateAsClient(AuthClientType clientType) {
        string cacheKey = nameof(Accounts_AuthenticateAsClient);
        if(cache.TryGet(cacheKey, out var cachedResponse)) {
            return (ClientAuthResponse) cachedResponse;
        }

        var response = await base.Accounts_AuthenticateAsClient(clientType);
        return response;
    }

    public override async Task<AccountAuthCodeAuthResponse> Accounts_AuthenticateAsAccount(AuthClientType clientType, string authCode) {
        string cacheKey = nameof(Accounts_AuthenticateAsAccount);
        if(cache.TryGet(cacheKey, out var cachedResponse)) {
            return (AccountAuthCodeAuthResponse) cachedResponse;
        }

        var response = await base.Accounts_AuthenticateAsAccount(clientType, authCode);
        return response;
    }

    public override async Task<AccountLookupResponse> Accounts_LookupAccountId(string accountId, string accessToken) {
        string cacheKey = nameof(Accounts_LookupAccountId);
        if(cache.TryGet(cacheKey, out var cachedResponse)) {
            return (AccountLookupResponse) cachedResponse;
        }

        var response = await base.Accounts_LookupAccountId(accountId, accessToken);
        return response;
    }

    public override async Task<QueryProfileCommonCoreResponse> Fortnite_QueryCommonCoreProfile(string accountId, string accessToken) {
        string cacheKey = nameof(Fortnite_QueryCommonCoreProfile);
        if(cache.TryGet(cacheKey, out var cachedResponse)) {
            return (QueryProfileCommonCoreResponse) cachedResponse;
        }

        var response = await base.Fortnite_QueryCommonCoreProfile(accountId, accessToken);
        return response;
    }

    public override async Task<QueryProfileAthenaResponse> Fortnite_QueryAthenaProfile(string accountId, string accessToken) {
        string cacheKey = nameof(Fortnite_QueryAthenaProfile);
        if(cache.TryGet(cacheKey, out var cachedResponse)) {
            return (QueryProfileAthenaResponse) cachedResponse;
        }

        var response = await base.Fortnite_QueryAthenaProfile(accountId, accessToken);
        return response;
    }
}