using System.Net;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames.Results;

public class EpicGamesDisplayNameResult(HttpStatusCode statusCode, string? displayName = null, string? errorMessage = null)
    : EpicGamesResult(statusCode, errorMessage)
{
    public string? DisplayName { get; init; } = displayName;
}