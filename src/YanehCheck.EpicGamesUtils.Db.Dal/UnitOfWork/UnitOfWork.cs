using Microsoft.EntityFrameworkCore;
using YanehCheck.EpicGamesUtils.Db.Dal.Entities;
using YanehCheck.EpicGamesUtils.Db.Dal.Mappers;
using YanehCheck.EpicGamesUtils.Db.Dal.Repositories;

namespace YanehCheck.EpicGamesUtils.Db.Dal.UnitOfWork;

public class UnitOfWork(DbContext context) : IUnitOfWork {
    public IRepository<TEntity> GetRepository<TEntity, TEntityMapper>() where TEntity : class, IEntity
        where TEntityMapper : IEntityMapper<TEntity>, new()
        => new Repository<TEntity>(context, new TEntityMapper());

    public async Task<int> SaveChangesAsync() => await context.SaveChangesAsync();
    public async ValueTask DisposeAsync() => await context.DisposeAsync();
}
