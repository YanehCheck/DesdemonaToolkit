using Microsoft.EntityFrameworkCore;
using YanehCheck.EpicGamesUtils.FortniteGGScraper;
using YanehCheck.EpicGamesUtils.FortniteGGScraper.Model;

namespace YanehCheck.EpicGamesUtils.DAL.Entities;

[Index(nameof(FortniteGgId))] // Mapping results from EGS API to the items
public class ItemEntity : IEntity
{
    public required Guid Id { get; set; }
    public required string FortniteId { get; set; }
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
    public IEnumerable<string> Styles { get; set; }
    public IEnumerable<ItemTag> Tags { get; set; }
}