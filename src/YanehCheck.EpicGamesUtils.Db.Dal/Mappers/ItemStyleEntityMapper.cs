using YanehCheck.EpicGamesUtils.Db.Dal.Entities;

namespace YanehCheck.EpicGamesUtils.Db.Dal.Mappers;

public class ItemStyleEntityMapper : IEntityMapper<ItemStyleEntity> {
    public void Map(ItemStyleEntity originalEntity, ItemStyleEntity newEntity) {
        newEntity.Name = originalEntity.Name;
        newEntity.Description = originalEntity.Description;
        newEntity.FortniteId = originalEntity.FortniteId;
        newEntity.Channel = originalEntity.Channel;
        newEntity.Property = originalEntity.Property;
    }
}