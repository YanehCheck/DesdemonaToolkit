using System.Text.Json.Serialization;
using YanehCheck.EpicGamesUtils.Common.Enums.Items;
using YanehCheck.EpicGamesUtils.Db.Bl.Models.Interfaces;

namespace YanehCheck.EpicGamesUtils.Db.Bl.Models;

public class ItemModel : IModel {
    [JsonIgnore]
    public Guid Id { get; set; }
    public string FortniteId { get; set; }
    public string FortniteGgId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? PriceVbucks { get; set; }
    public decimal? PriceUsd { get; set; }
    public string? Season { get; set; }
    public ItemSource Source { get; set; }
    public string? SourceDescription { get; set; }
    public ItemRarity Rarity { get; set; }
    public ItemType Type { get; set; }
    public string? Set { get; set; }
    public DateTime? Release { get; set; }
    public DateTime? LastSeen { get; set; }
    public IEnumerable<string> FortniteGgStyles { get; set; }
    public IEnumerable<ItemTag> Tags { get; set; }

    public ItemModel GetEmpty() => new() {
        Id = Guid.Empty,
        FortniteId = default!,
        FortniteGgId = default!,
        Name = default,
        Description = default,
        PriceVbucks = default,
        PriceUsd = default,
        Season = default,
        Source = default,
        SourceDescription = default,
        Rarity = default,
        Type = default,
        Set = default,
        Release = default,
        LastSeen = default,
        FortniteGgStyles = default!,
        Tags = default!
    };
}