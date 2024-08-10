using YanehCheck.EpicGamesUtils.Db.Bl.Models;
using YanehCheck.EpicGamesUtils.Db.Dal.Entities;

namespace YanehCheck.EpicGamesUtils.Db.Bl.Facades.Interfaces;

public interface IItemFacade : IFacade<ItemEntity, ItemModel> {
    Task<ItemModel> SaveByFortniteIdAsync(ItemModel model);
    Task<IEnumerable<ItemModel>> SaveByFortniteIdAsync(IEnumerable<ItemModel> models);
    Task<IEnumerable<ItemModel>> GetByFortniteIdAsync(IEnumerable<ItemModel> models);
}