using YanehCheck.EpicGamesUtils.Db.Bl.Mappers.Interfaces;
using YanehCheck.EpicGamesUtils.Db.Bl.Models;
using YanehCheck.EpicGamesUtils.Db.Dal.Entities;

namespace YanehCheck.EpicGamesUtils.Db.Bl.Mappers;

public class ItemStyleModelMapper : IModelMapper<ItemStyleEntity, ItemStyleModel> {
    public ItemStyleModel MapToModel(ItemStyleEntity entity) {
        return new ItemStyleModel() {
            Id = entity.Id,
            FortniteId = entity.FortniteId,
            Name = entity.Name,
            Description = entity.Description,
            Channel = entity.Channel,
            Property = entity.Property,
            ItemFortniteId = entity.ItemFortniteId
        };
    }

    public ItemStyleEntity MapToEntity(ItemStyleModel model) {
        return new ItemStyleEntity() {
            Id = model.Id,
            FortniteId = model.FortniteId,
            Name = model.Name,
            Description = model.Description,
            Channel = model.Channel,
            Property = model.Property,
            ItemFortniteId = model.ItemFortniteId
        };
    }
}