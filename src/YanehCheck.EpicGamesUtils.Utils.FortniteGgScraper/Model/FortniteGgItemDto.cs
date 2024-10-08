﻿using Newtonsoft.Json;
using YanehCheck.EpicGamesUtils.Common.Enums.Items;

namespace YanehCheck.EpicGamesUtils.Utils.FortniteGgScraper.Model;

public class FortniteGgItemDto
{
    [JsonProperty(PropertyName = "FortniteId")]
    public string Id { get; set; }
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