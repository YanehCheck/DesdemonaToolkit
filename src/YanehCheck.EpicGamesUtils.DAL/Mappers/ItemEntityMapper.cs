using YanehCheck.EpicGamesUtils.DAL.Entities;

namespace YanehCheck.EpicGamesUtils.DAL.Mappers;

public class ItemEntityMapper : IEntityMapper<ItemEntity> {
    public void Map(ItemEntity originalEntity, ItemEntity newEntity) {
        newEntity.FortniteId = originalEntity.FortniteId;
        newEntity.FortniteGgId = originalEntity.FortniteGgId;
        newEntity.Name = originalEntity.Name;
        newEntity.Description = originalEntity.Description;
        newEntity.PriceVbucks = originalEntity.PriceVbucks;
        newEntity.PriceUsd = originalEntity.PriceUsd;
        newEntity.Season = originalEntity.Season;
        newEntity.Source = originalEntity.Source;
        newEntity.SourceDescription = originalEntity.SourceDescription;
        newEntity.Rarity = originalEntity.Rarity;
        newEntity.Type = originalEntity.Type;
        newEntity.Set = originalEntity.Set;
        newEntity.Release = originalEntity.Release;
        newEntity.LastSeen = originalEntity.LastSeen;
        newEntity.Styles = originalEntity.Styles;
        newEntity.Tags = originalEntity.Tags;
    }
}