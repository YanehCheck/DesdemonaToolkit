using YanehCheck.EpicGamesUtils.BL.Models;
using YanehCheck.EpicGamesUtils.DAL.Entities;

namespace YanehCheck.EpicGamesUtils.BL.Facades.Interfaces;

public interface IItemFacade : IFacade<ItemEntity, ItemModel> {
    Task<ItemModel> SaveAsyncByFortniteId(ItemModel model);
    Task<IEnumerable<ItemModel>> SaveAsyncByFortniteId(IEnumerable<ItemModel> models);
}