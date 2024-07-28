using System.Net;
using YanehCheck.EpicGamesUtils.WpfUiApp.Models;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames.Results;

public class EpicGamesItemsResult(HttpStatusCode statusCode, IEnumerable<EpicGamesItem>? items = null, string? errorMessage = null)
    : EpicGamesResult(statusCode, errorMessage) {
    public IEnumerable<EpicGamesItem>? Items { get; set; } = items;
}