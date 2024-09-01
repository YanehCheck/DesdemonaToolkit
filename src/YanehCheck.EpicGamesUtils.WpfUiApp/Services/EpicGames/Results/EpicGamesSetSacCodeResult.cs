using System.Net;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames.Results;

public class EpicGamesSetSacCodeResult(HttpStatusCode statusCode, string? sacCode = null, string? errorMessage = null)
    : EpicGamesResult(statusCode, errorMessage)
{
    public string? NewSacCode { get; init; } = sacCode;
}