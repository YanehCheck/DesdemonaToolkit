namespace YanehCheck.EpicGamesUtils.WpfUiApp.Models;

/// <summary>
/// Represents item obtained return by QueryProfile by EG API
/// </summary>
public class EpicGamesItem(string fortniteId) {
    public string FortniteId { get; init; } = fortniteId;
}