using YanehCheck.EpicGamesUtils.Db.Bl.Models;
using YanehCheck.EpicGamesUtils.Utils.FortniteAssetSerializer.Dtos;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems.Mappers;

public interface IItemStyleAssetMapper {
    ItemStyleModel MapToModel(ItemStyleAssetDto dto);
}