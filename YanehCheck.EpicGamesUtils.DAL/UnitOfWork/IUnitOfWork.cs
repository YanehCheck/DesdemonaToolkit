using YanehCheck.EpicGamesUtils.DAL.Entities;
using YanehCheck.EpicGamesUtils.DAL.Mappers;
using YanehCheck.EpicGamesUtils.DAL.Repositories;

namespace YanehCheck.EpicGamesUtils.DAL.UnitOfWork;

public interface IUnitOfWork {
    IRepository<TEntity> GetRepository<TEntity, TEntityMapper>() where TEntity : class, IEntity where TEntityMapper : IEntityMapper<TEntity>, new();
    Task<int> SaveChangesAsync();
    ValueTask DisposeAsync();
}