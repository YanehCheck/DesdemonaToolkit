using YanehCheck.EpicGamesUtils.Db.Bl.Models;
using YanehCheck.EpicGamesUtils.WpfUiApp.Types.Enums;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Types.Models;

/// <summary>
/// Represents item, extended with user and app specific information.
/// </summary>
public class ItemOwnedModel : ItemModel
{
    public IEnumerable<ItemStyleRaw> OwnedStylesRaw { get; set; } = [];
    public string? Remark { get; set; } = null;

    public ItemOwnedModel() { }
    public ItemOwnedModel(ItemModel item)
    {
        Id = item.Id;
        FortniteId = item.FortniteId;
        FortniteGgId = item.FortniteGgId;
        Name = item.Name;
        Description = item.Description;
        PriceVbucks = item.PriceVbucks;
        PriceUsd = item.PriceUsd;
        Season = item.Season;
        Source = item.Source;
        SourceDescription = item.SourceDescription;
        Rarity = item.Rarity;
        Type = item.Type;
        Set = item.Set;
        Release = item.Release;
        LastSeen = item.LastSeen;
        Styles = item.Styles ?? [];
        Tags = item.Tags ?? [];
    }
}