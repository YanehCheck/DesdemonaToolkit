using YanehCheck.EpicGamesUtils.Db.Bl.Models;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Models;

/// <summary>
/// Represents item, extended with user and app specific information.
/// </summary>
public class ItemOwnedModel : ItemModel {
    public IEnumerable<string> OwnedStylesRaw { get; set; } = [];
    public string? Remark { get; set; } = null;
}