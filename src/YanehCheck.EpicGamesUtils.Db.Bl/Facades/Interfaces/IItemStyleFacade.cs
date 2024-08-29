using YanehCheck.EpicGamesUtils.Db.Bl.Models;
using YanehCheck.EpicGamesUtils.Db.Dal.Entities;

namespace YanehCheck.EpicGamesUtils.Db.Bl.Facades.Interfaces;

public interface IItemStyleFacade : IFacade<ItemStyleEntity, ItemStyleModel> {
    Task<ItemStyleModel> SaveByFortniteIdAsync(ItemStyleModel model);
    Task<IEnumerable<ItemStyleModel>> SaveByFortniteIdAsync(IEnumerable<ItemStyleModel> models);
    Task<IEnumerable<ItemStyleModel>> GetByFortniteIdAsync(IEnumerable<ItemStyleModel> models);
}