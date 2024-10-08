﻿using Newtonsoft.Json;
using YanehCheck.EpicGamesUtils.Db.Bl.Models.Interfaces;

namespace YanehCheck.EpicGamesUtils.Db.Bl.Models;

public class ItemStyleModel : IModel {
    [JsonIgnore]
    public Guid Id { get; set; }
    public string FortniteId { get; set; }
    public string Channel { get; set; }
    public string Property { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public required string ItemFortniteId { get; set; }
    [JsonIgnore]
    public ItemModel? Item { get; set; }
}