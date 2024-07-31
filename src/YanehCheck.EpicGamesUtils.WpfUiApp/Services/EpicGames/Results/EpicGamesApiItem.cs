namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames.Results;

/// <summary>
/// Represents item obtained return by QueryProfile by EG API
/// </summary>
public class EpicGamesApiItem(string fortniteId)
{
    public string FortniteId { get; init; } = fortniteId;
}