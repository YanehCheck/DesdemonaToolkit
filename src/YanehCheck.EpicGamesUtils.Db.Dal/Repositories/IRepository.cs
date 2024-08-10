using YanehCheck.EpicGamesUtils.Db.Dal.Entities;

namespace YanehCheck.EpicGamesUtils.Db.Dal.Repositories;

public interface IRepository<TEntity> where TEntity : class, IEntity {
    Task<TEntity> GetByIdAsync(Guid id);
    Task<List<TEntity>> GetAllAsync();
    IQueryable<TEntity> Get();
    Task<bool> ExistsAsync(TEntity entity);
    Task<bool> ExistsByIdAsync(Guid id);
    Task<TEntity> AddAsync(TEntity entity);
    Task BulkAddOrUpdateAsync(IEnumerable<TEntity> entity);
    Task BulkReadAsync(IEnumerable<TEntity> entities, List<string> byProperties = null);
    Task<TEntity> UpdateAsync(TEntity entity);
    void Delete(TEntity entity);
    void DeleteById(Guid id);
}
