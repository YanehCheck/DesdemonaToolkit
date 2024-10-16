﻿using YanehCheck.EpicGamesUtils.Db.Bl.Models;
using YanehCheck.EpicGamesUtils.WpfUiApp.Types.Enums;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Types.Classes;

/// <summary>
/// Represents item, extended with user and app specific information.
/// </summary>
public class ItemOwnedModel : ItemModel
{
    public IEnumerable<ItemStyleRaw> OwnedStylesRaw { get; set; } = [];
    public IEnumerable<ItemStyleModel> OwnedStyles { get; set; } = [];
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
        FortniteGgStyles = item.FortniteGgStyles ?? [];
        Tags = item.Tags ?? [];
    }
}