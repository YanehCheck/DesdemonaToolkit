using Microsoft.EntityFrameworkCore;
using YanehCheck.EpicGamesUtils.DAL.Entities;
using YanehCheck.EpicGamesUtils.DAL.Mappers;
using YanehCheck.EpicGamesUtils.DAL.Repositories;

namespace YanehCheck.EpicGamesUtils.DAL.UnitOfWork;

public class UnitOfWork(DbContext context) : IUnitOfWork {
    public IRepository<TEntity> GetRepository<TEntity, TEntityMapper>() where TEntity : class, IEntity
        where TEntityMapper : IEntityMapper<TEntity>, new()
        => new Repository<TEntity>(context, new TEntityMapper());

    public async Task<int> SaveChangesAsync() => await context.SaveChangesAsync();
    public async ValueTask DisposeAsync() => await context.DisposeAsync();
}
