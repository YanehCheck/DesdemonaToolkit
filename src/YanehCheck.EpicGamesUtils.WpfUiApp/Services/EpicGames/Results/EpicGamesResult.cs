using System.Net;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames.Results;

public class EpicGamesResult(HttpStatusCode statusCode, string? errorMessage = null)
{
    public HttpStatusCode StatusCode { get; init; } = statusCode;
    public bool Success => (int)StatusCode is > 199 and < 300;
    public string? ErrorMessage { get; init; } = errorMessage;

    public static implicit operator bool(EpicGamesResult result) => result.Success;
}