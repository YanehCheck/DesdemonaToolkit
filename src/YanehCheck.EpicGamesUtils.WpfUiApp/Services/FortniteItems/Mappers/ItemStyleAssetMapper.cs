using YanehCheck.EpicGamesUtils.Db.Bl.Models;
using YanehCheck.EpicGamesUtils.Utils.FortniteAssetSerializer.Dtos;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems.Mappers;

public class ItemStyleAssetMapper : IItemStyleAssetMapper {
    public ItemStyleModel MapToModel(ItemStyleAssetDto dto) {
        return new ItemStyleModel {
            FortniteId = dto.FortniteId,
            ItemFortniteId = dto.ItemFortniteId,
            Name = dto.Name,
            Description = dto.Description,
            Channel = dto.Channel,
            Property = dto.Property
        };
    }
}