using Newtonsoft.Json;
using YanehCheck.EpicGamesUtils.EgsApi.Api.Responses.Interfaces;

namespace YanehCheck.EpicGamesUtils.EgsApi.Api.Responses;

public class AccountAuthCodeAuthResponse : IJsonParseableResponse {
    [JsonProperty("access_token")]
    public string AccessToken { get; set; }

    [JsonProperty("expires_in")]
    public int ExpiresIn { get; set; }

    [JsonProperty("expires_at")]
    public DateTime ExpiresAt { get; set; }

    [JsonProperty("token_type")]
    public string TokenType { get; set; }

    [JsonProperty("refresh_token")]
    public string RefreshToken { get; set; }

    [JsonProperty("refresh_expires")]
    public int RefreshExpires { get; set; }

    [JsonProperty("refresh_expires_at")]
    public DateTime RefreshExpiresAt { get; set; }

    [JsonProperty("account_id")]
    public string AccountId { get; set; }

    [JsonProperty("client_id")]
    public string ClientId { get; set; }

    [JsonProperty("internal_client")]
    public bool InternalClient { get; set; }

    [JsonProperty("client_service")]
    public string ClientService { get; set; }

    [JsonProperty("scope")]
    public List<string> Scope { get; set; }

    [JsonProperty("displayName")]
    public string DisplayName { get; set; }

    [JsonProperty("app")]
    public string App { get; set; }

    [JsonProperty("in_app_id")]
    public string InAppId { get; set; }

    [JsonProperty("device_id")]
    public string DeviceId { get; set; }

    [JsonProperty("product_id")]
    public string ProductId { get; set; }

    [JsonProperty("application_id")]
    public string ApplicationId { get; set; }

    [JsonProperty("acr")]
    public string Acr { get; set; }

    [JsonProperty("auth_time")]
    public DateTime AuthTime { get; set; }
}