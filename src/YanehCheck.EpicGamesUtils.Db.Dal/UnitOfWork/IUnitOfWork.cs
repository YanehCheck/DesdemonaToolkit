using YanehCheck.EpicGamesUtils.Db.Dal.Entities;
using YanehCheck.EpicGamesUtils.Db.Dal.Mappers;
using YanehCheck.EpicGamesUtils.Db.Dal.Repositories;

namespace YanehCheck.EpicGamesUtils.Db.Dal.UnitOfWork;

public interface IUnitOfWork {
    IRepository<TEntity> GetRepository<TEntity, TEntityMapper>() where TEntity : class, IEntity where TEntityMapper : IEntityMapper<TEntity>, new();
    Task<int> SaveChangesAsync();
    ValueTask DisposeAsync();
}