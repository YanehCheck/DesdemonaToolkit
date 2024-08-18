using System.Net;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames.Results;

public class EpicGamesAuthResult(
    HttpStatusCode statusCode,
    string? accountId = null,
    string? accessToken = null,
    DateTime? accessTokenExpiry = null,
    string? displayName = null,
    string? errorMessage = null) : EpicGamesResult(statusCode, errorMessage)
{
    public string? AccountId { get; init; } = accountId;
    public string? AccessToken { get; init; } = accessToken;
    public DateTime? AccessTokenExpiry { get; init; } = accessTokenExpiry;
    public string? DisplayName { get; init; } = displayName;
}