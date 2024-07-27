using YanehCheck.EpicGamesUtils.BL.Models.Interfaces;
using YanehCheck.EpicGamesUtils.DAL.Entities;

namespace YanehCheck.EpicGamesUtils.BL.Facades.Interfaces;

public interface IFacade<TEntity, TModel> where TEntity : class, IEntity where TModel : class, IModel {
    Task DeleteAsync(Guid id);
    Task<IEnumerable<TModel>> GetAllAsync();
    Task<TModel?> GetAsync(Guid id, bool includeRelatedObjects = true);
    Task<TModel> SaveAsync(TModel model);
}