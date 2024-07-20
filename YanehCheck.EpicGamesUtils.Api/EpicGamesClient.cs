using System.Text;
using System.Text.Json;
using RestSharp;
using YanehCheck.EpicGamesUtils.Api.Auth;
using YanehCheck.EpicGamesUtils.Api.Stw;

namespace YanehCheck.EpicGamesUtils.Api;

public class EpicGamesClient(IRestClient client) : IEpicGamesClient {
    private string accessToken = null!;
    private string accountId = null!;

    public async Task<ApiResult> AuthenticateAsClient(AuthClientType clientType) {
        
        var authClient = AuthClient.GetClient(clientType);
        var credentialsBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{authClient.Id}:{authClient.Secret}"));

        var request = new RestRequest("https://account-public-service-prod.ol.epicgames.com/account/api/oauth/token", Method.Post);
        
        request.AddHeader("Authorization", $"Basic {credentialsBase64}");
        request.AddParameter("grant_type", "client_credentials");

        var response = await client.ExecuteAsync(request);

        var jsonDom = JsonDocument.Parse(response.Content!);
        if(response.IsSuccessful) {
            return ApiResult.Ok();
        }
        else {
            var errorCode = jsonDom.RootElement.GetProperty("errorCode");
            var errorMessage = jsonDom.RootElement.GetProperty("errorMessage");
            return ApiResult.Error($"{errorCode} - {errorMessage}");
        }
    }

    public async Task<ApiResult> AuthenticateAsAccount(AuthClientType clientType, string authCode) {
        var authClient = AuthClient.GetClient(clientType);
        var credentialsBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{authClient.Id}:{authClient.Secret}"));

        var request = new RestRequest("https://account-public-service-prod.ol.epicgames.com/account/api/oauth/token", Method.Post);

        request.AddHeader("Authorization", $"Basic {credentialsBase64}");
        request.AddParameter("grant_type", "authorization_code");
        request.AddParameter("code", authCode);

        var response = await client.ExecuteAsync(request);

        var jsonDom = JsonDocument.Parse(response.Content!);
        if (response.IsSuccessful) {
            accessToken = jsonDom.RootElement.GetProperty("access_token").ToString();
            accountId = jsonDom.RootElement.GetProperty("account_id").ToString();
            return ApiResult.Ok();
        }
        else {
            var errorCode = jsonDom.RootElement.GetProperty("errorCode").ToString() ?? "ERROR CODE MISSING";
            var errorMessage = jsonDom.RootElement.GetProperty("errorMessage").ToString() ?? "ERROR MESSAGE MISSING";
            return ApiResult.Error($"{errorCode} - {errorMessage}");
        }
    }

    public async Task<ApiResult> GetFounderCodes(FounderCodePlatform platform) {
        var request =
            new RestRequest(
                $"https://fngw-mcp-gc-livefn.ol.epicgames.com/fortnite/api/game/v2/friendcodes/{accountId}/{Enum.GetName(platform)!.ToLower()}");
        request.AddHeader("Authorization", $"Bearer {accessToken}");

        var response = await client.ExecuteAsync(request);
        var jsonDom = JsonDocument.Parse(response.Content!);
        var codeIds = jsonDom.RootElement.EnumerateArray()
            .Where(element => element.GetProperty("codeType").GetString() == "CodeToken:founderfriendinvite")
            .Select(element => element.GetProperty("codeId").GetString());
        var codes = string.Join(Environment.NewLine, codeIds);
        return new ApiResult(response.IsSuccessful, codes);
    }

    public async Task<ApiResult> GetInventory() {
        var request =
            new RestRequest(
                $"https://fngw-mcp-gc-livefn.ol.epicgames.com/fortnite/api/game/v2/profile/{accountId}/client/QueryProfile", Method.Post); 
        request.AddHeader("Authorization", $"Bearer {accessToken}");
        request.AddQueryParameter("profileId", "athena");
        request.AddBody("{}");

        var response = await client.ExecuteAsync(request);
        return new ApiResult(response.IsSuccessful, response.Content);
    }
}