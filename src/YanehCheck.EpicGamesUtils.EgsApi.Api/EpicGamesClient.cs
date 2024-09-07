using System.Text;
using Newtonsoft.Json.Linq;
using RestSharp;
using YanehCheck.EpicGamesUtils.EgsApi.Api.Enums;
using YanehCheck.EpicGamesUtils.EgsApi.Api.Helpers;
using YanehCheck.EpicGamesUtils.EgsApi.Api.Mappers;
using YanehCheck.EpicGamesUtils.EgsApi.Api.Responses;

namespace YanehCheck.EpicGamesUtils.EgsApi.Api;

/// <inheritdoc cref="IEpicGamesClient"/>
public class EpicGamesClient(IRestClient client) : IEpicGamesClient {
    private readonly ErrorResponseMapper errorMapper = new();
    private readonly ClientAuthResponseMapper clientAuthMapper = new();
    private readonly AccountAuthCodeAuthResponseMapper accAuthCodeMapper = new();
    private readonly AccountLookupResponseMapper accountLookupMapper = new();
    private readonly QueryProfileCommonCoreResponseMapper queryCoreMapper = new();
    private readonly QueryProfileAthenaResponseMapper queryAthenaMapper = new();

    public virtual async Task<ClientAuthResponse> Accounts_AuthenticateAsClient(AuthClientType clientType) {
        var authClient = AuthClient.GetClient(clientType);
        var credentialsBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{authClient.Id}:{authClient.Secret}"));

        var request = new RestRequest("https://account-public-service-prod.ol.epicgames.com/account/api/oauth/token", Method.Post);

        request.AddHeader("Authorization", $"Basic {credentialsBase64}");
        request.AddParameter("grant_type", "client_credentials");

        var response = await client.ExecuteAsync(request);
        if(response.Content is null) {
            throw new HttpRequestException($"Could not contact the Epic Games API (response code {response.StatusCode}).");
        }
        var json = JToken.Parse(response.Content!);

        if (response.IsSuccessStatusCode) {
            return clientAuthMapper.MapFromJson(json)!;
        }
        else {
            throw errorMapper.MapFromJson(json)!;
        }
    }
    
    public virtual async Task<AccountAuthCodeAuthResponse> Accounts_AuthenticateAsAccount(AuthClientType clientType, string authCode) {
        var authClient = AuthClient.GetClient(clientType);
        var credentialsBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{authClient.Id}:{authClient.Secret}"));

        var request = new RestRequest("https://account-public-service-prod.ol.epicgames.com/account/api/oauth/token", Method.Post);

        request.AddHeader("Authorization", $"Basic {credentialsBase64}");
        request.AddParameter("grant_type", "authorization_code");
        request.AddParameter("code", authCode);

        var response = await client.ExecuteAsync(request);
        if (response.Content is null) {
            throw new HttpRequestException($"Could not contact the Epic Games API (response code {response.StatusCode}).");
        }

        var json = JToken.Parse(response.Content!);

        if(response.IsSuccessStatusCode) {
            return accAuthCodeMapper.MapFromJson(json)!;
        }
        else {
            throw errorMapper.MapFromJson(json)!;
        }
    }

    public virtual async Task<AccountLookupResponse> Accounts_LookupAccountId(string accountId, string accessToken) {
        var request =
            new RestRequest(
                $"https://account-public-service-prod.ol.epicgames.com/account/api/public/account/{accountId}");
        request.AddHeader("Authorization", $"Bearer {accessToken}");

        var response = await client.ExecuteAsync(request);
        if(response.Content is null) {
            throw new HttpRequestException($"Could not contact the Epic Games API (response code {response.StatusCode}).");
        }
        var json = JToken.Parse(response.Content!);

        if(response.IsSuccessStatusCode) {
            return accountLookupMapper.MapFromJson(json)!;
        }
        else {
            throw errorMapper.MapFromJson(json)!;
        }
    }

    public virtual async Task<QueryProfileCommonCoreResponse> Fortnite_QueryCommonCoreProfile(string accountId, string accessToken) {
        var request =
            new RestRequest(
                $"https://fngw-mcp-gc-livefn.ol.epicgames.com/fortnite/api/game/v2/profile/{accountId}/client/QueryProfile", Method.Post);
        request.AddHeader("Authorization", $"Bearer {accessToken}");
        request.AddQueryParameter("profileId", FortniteProfile.CommonCore.ToParameterString());
        request.AddBody("{}");

        var response = await client.ExecuteAsync(request);
        if(response.Content is null) {
            throw new HttpRequestException($"Could not contact the Epic Games API (response code {response.StatusCode}).");
        }
        var json = JToken.Parse(response.Content!);

        if(response.IsSuccessStatusCode) {
            return queryCoreMapper.MapFromJson(json)!;
        }
        else {
            throw errorMapper.MapFromJson(json)!;
        }
    }

    public virtual async Task<QueryProfileAthenaResponse> Fortnite_QueryAthenaProfile(string accountId, string accessToken) {
        var request =
            new RestRequest(
                $"https://fngw-mcp-gc-livefn.ol.epicgames.com/fortnite/api/game/v2/profile/{accountId}/client/QueryProfile", Method.Post);
        request.AddHeader("Authorization", $"Bearer {accessToken}");
        request.AddQueryParameter("profileId", FortniteProfile.Athena.ToParameterString());
        request.AddBody("{}");

        var response = await client.ExecuteAsync(request);
        if(response.Content is null) {
            throw new HttpRequestException($"Could not contact the Epic Games API (response code {response.StatusCode}).");
        }
        var json = JToken.Parse(response.Content!);

        if(response.IsSuccessStatusCode) {
            return queryAthenaMapper.MapFromJson(json)!;
        }
        else {
            throw errorMapper.MapFromJson(json)!;
        }
    }

    public virtual async Task<QueryProfileCommonCoreResponse> Accounts_SetSacCode(string accountId, string accessToken, string sacCode) {
        var request =
            new RestRequest(
                $"https://fngw-mcp-gc-livefn.ol.epicgames.com/fortnite/api/game/v2/profile/{accountId}/client/SetAffiliateName", Method.Post);
        request.AddHeader("Authorization", $"Bearer {accessToken}");
        request.AddQueryParameter("profileId", FortniteProfile.CommonCore.ToParameterString());
        request.AddBody($"{{\"affiliateName\":\"{sacCode}\"}}");

        var response = await client.ExecuteAsync(request);
        var json = JToken.Parse(response.Content!);

        if(response.IsSuccessStatusCode) {
            return queryCoreMapper.MapFromJson(json)!;
        }
        else {
            throw errorMapper.MapFromJson(json)!;
        }
    }
}