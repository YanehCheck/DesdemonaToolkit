using YanehCheck.EpicGamesUtils.BL.Models;
using YanehCheck.EpicGamesUtils.DAL.Entities;

namespace YanehCheck.EpicGamesUtils.BL.Facades.Interfaces;

public interface IItemFacade : IFacade<ItemEntity, ItemModel> {
    Task<ItemModel> SaveByFortniteIdAsync(ItemModel model);
    Task<IEnumerable<ItemModel>> SaveByFortniteIdAsync(IEnumerable<ItemModel> models);
}