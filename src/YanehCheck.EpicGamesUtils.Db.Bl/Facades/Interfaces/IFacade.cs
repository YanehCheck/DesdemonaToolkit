using YanehCheck.EpicGamesUtils.Db.Bl.Models.Interfaces;
using YanehCheck.EpicGamesUtils.Db.Dal.Entities;

namespace YanehCheck.EpicGamesUtils.Db.Bl.Facades.Interfaces;

public interface IFacade<TEntity, TModel> where TEntity : class, IEntity where TModel : class, IModel {
    Task DeleteAsync(Guid id);
    Task<IEnumerable<TModel>> GetAllAsync();
    Task<TModel?> GetAsync(Guid id, bool includeRelatedObjects = true);
    Task<TModel> SaveAsync(TModel model);
    Task<bool> AnyAsync();
}