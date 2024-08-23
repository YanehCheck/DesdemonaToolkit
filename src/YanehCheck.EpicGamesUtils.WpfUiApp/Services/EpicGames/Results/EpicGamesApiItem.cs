using YanehCheck.EpicGamesUtils.WpfUiApp.Models;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames.Results;

/// <summary>
/// Represents item obtained return by QueryProfile by EG API
/// </summary>
public class EpicGamesApiItem(string fortniteId, IEnumerable<ItemStyleRaw>? ownedStylesRaw = null!)
{
    public string FortniteId { get; init; } = fortniteId;
    public List<ItemStyleRaw> OwnedStylesRaw { get; init; } = ownedStylesRaw?.ToList() ?? [];
}