using YanehCheck.EpicGamesUtils.DAL.Entities;

namespace YanehCheck.EpicGamesUtils.DAL.Repositories;

public interface IRepository<TEntity> where TEntity : class, IEntity {
    Task<TEntity> GetByIdAsync(Guid id);
    Task<List<TEntity>> GetAllAsync();
    IQueryable<TEntity> Get();
    Task<bool> ExistsAsync(TEntity entity);
    Task<bool> ExistsByIdAsync(Guid id);
    Task<TEntity> AddAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
    void Delete(TEntity entity);
    void DeleteById(Guid id);
}
