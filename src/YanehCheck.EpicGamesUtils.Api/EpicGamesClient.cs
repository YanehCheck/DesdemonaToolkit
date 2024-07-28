using System.Text;
using Newtonsoft.Json.Linq;
using RestSharp;
using YanehCheck.EpicGamesUtils.Api.Auth;
using YanehCheck.EpicGamesUtils.Api.Results;
using YanehCheck.EpicGamesUtils.Api.Stw;

namespace YanehCheck.EpicGamesUtils.Api;

public class EpicGamesClient(IRestClient client) : IEpicGamesClient {
    public async Task<ApiResult> Accounts_AuthenticateAsClient(AuthClientType clientType) {
        
        var authClient = AuthClient.GetClient(clientType);
        var credentialsBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{authClient.Id}:{authClient.Secret}"));

        var request = new RestRequest("https://account-public-service-prod.ol.epicgames.com/account/api/oauth/token", Method.Post);
        
        request.AddHeader("Authorization", $"Basic {credentialsBase64}");
        request.AddParameter("grant_type", "client_credentials");

        var response = await client.ExecuteAsync(request);

        var jsonDom = response.Content is not null ?
            JObject.Parse(response.Content!) :
            null;
        return new ApiResult(response.StatusCode, jsonDom);
    }

    public async Task<ApiResult> Accounts_AuthenticateAsAccount(AuthClientType clientType, string authCode) {
        var authClient = AuthClient.GetClient(clientType);
        var credentialsBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{authClient.Id}:{authClient.Secret}"));

        var request = new RestRequest("https://account-public-service-prod.ol.epicgames.com/account/api/oauth/token", Method.Post);

        request.AddHeader("Authorization", $"Basic {credentialsBase64}");
        request.AddParameter("grant_type", "authorization_code");
        request.AddParameter("code", authCode);

        var response = await client.ExecuteAsync(request);
        var jsonDom = response.Content is not null ?
            JObject.Parse(response.Content!) : 
            null;
        return new ApiResult(response.StatusCode, jsonDom);
    }

    public async Task<ApiResult> Fortnite_GetFounderCodes(FounderCodePlatform platform, string accountId, string accessToken) {
        var request =
            new RestRequest(
                $"https://fngw-mcp-gc-livefn.ol.epicgames.com/fortnite/api/game/v2/friendcodes/{accountId}/{Enum.GetName(platform)!.ToLower()}");
        request.AddHeader("Authorization", $"Bearer {accessToken}");

        var response = await client.ExecuteAsync(request);
        var jsonDom = response.Content is not null ?
            JObject.Parse(response.Content!) :
            null;
        return new ApiResult(response.StatusCode, jsonDom);
    }

    public async Task<ApiResult> Accounts_LookupAccountId(string accountId, string accessToken) {
        var request =
            new RestRequest(
                $"https://account-public-service-prod.ol.epicgames.com/account/api/public/account/{accountId}");
        request.AddHeader("Authorization", $"Bearer {accessToken}");

        var response = await client.ExecuteAsync(request);
        var jsonDom = response.Content is not null ?
            JObject.Parse(response.Content!) :
            null;
        return new ApiResult(response.StatusCode, jsonDom);
    }

    public async Task<ApiResult> Fortnite_QueryProfile(string accountId, string accessToken) {
        var request =
            new RestRequest(
                $"https://fngw-mcp-gc-livefn.ol.epicgames.com/fortnite/api/game/v2/profile/{accountId}/client/QueryProfile", Method.Post); 
        request.AddHeader("Authorization", $"Bearer {accessToken}");
        request.AddQueryParameter("profileId", "athena");
        request.AddBody("{}");

        var response = await client.ExecuteAsync(request);
        var jsonDom = response.Content is not null ?
            JObject.Parse(response.Content!) :
            null;
        return new ApiResult(response.StatusCode, jsonDom);
    }
}