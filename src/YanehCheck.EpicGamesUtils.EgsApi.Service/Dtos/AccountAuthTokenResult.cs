namespace YanehCheck.EpicGamesUtils.EgsApi.Service.Dtos;

public class AccountAuthTokenResult (
    string accountId,
    string accessToken,
    DateTime accessTokenExpiry,
    string displayName) : IResult {
    public string AccountId { get; } = accountId;
    public string AccessToken { get; } = accessToken;
    public DateTime AccessTokenExpiry { get; } = accessTokenExpiry;
    public string DisplayName { get; } = displayName;
}