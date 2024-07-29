using System.Net;
using YanehCheck.EpicGamesUtils.WpfUiApp.Models;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames.Results;

public class EpicGamesItemsResult(HttpStatusCode statusCode, IEnumerable<EpicGamesApiItem>? items = null, string? errorMessage = null)
    : EpicGamesResult(statusCode, errorMessage) {
    public IEnumerable<EpicGamesApiItem>? Items { get; set; } = items;
}