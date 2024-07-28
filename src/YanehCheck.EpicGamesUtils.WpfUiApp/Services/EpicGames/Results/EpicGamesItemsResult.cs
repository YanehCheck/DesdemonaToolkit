using System.Net;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames.Results;

public class EpicGamesItemsResult(HttpStatusCode statusCode, IEnumerable<string>? itemFortniteIds = null, string? errorMessage = null)
    : EpicGamesResult(statusCode, errorMessage) {
    public IEnumerable<string>? ItemFortniteIds { get; set; } = itemFortniteIds;
}