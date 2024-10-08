﻿using YanehCheck.EpicGamesUtils.Db.Bl.Mappers.Interfaces;
using YanehCheck.EpicGamesUtils.Db.Bl.Models;
using YanehCheck.EpicGamesUtils.Db.Dal.Entities;

namespace YanehCheck.EpicGamesUtils.Db.Bl.Mappers;

public class ItemModelMapper(ItemStyleModelMapper itemStyleMapper) : IModelMapper<ItemEntity, ItemModel> {
    public ItemModel MapToModel(ItemEntity entity) {
        return new ItemModel() {
            Id = entity.Id,
            FortniteId = entity.FortniteId,
            FortniteGgId = entity.FortniteGgId,
            Name = entity.Name,
            Description = entity.Description,
            PriceVbucks = entity.PriceVbucks,
            PriceUsd = entity.PriceUsd,
            Season = entity.Season,
            Source = entity.Source,
            SourceDescription = entity.SourceDescription,
            Rarity = entity.Rarity,
            Type = entity.Type,
            Set = entity.Set,
            Release = entity.Release,
            LastSeen = entity.LastSeen,
            FortniteGgStyles = entity.FortniteGgStyles,
            Tags = entity.Tags
        };
    }

    public ItemEntity MapToEntity(ItemModel model) {
        return new ItemEntity() {
            Id = model.Id,
            FortniteId = model.FortniteId,
            FortniteGgId = model.FortniteGgId,
            Name = model.Name,
            Description = model.Description,
            PriceVbucks = model.PriceVbucks,
            PriceUsd = model.PriceUsd,
            Season = model.Season,
            Source = model.Source,
            SourceDescription = model.SourceDescription,
            Rarity = model.Rarity,
            Type = model.Type,
            Set = model.Set,
            Release = model.Release,
            LastSeen = model.LastSeen,
            FortniteGgStyles = model.FortniteGgStyles,
            Tags = model.Tags
        };
    }
}