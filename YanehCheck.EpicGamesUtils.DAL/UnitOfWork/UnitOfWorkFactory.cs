using Microsoft.EntityFrameworkCore;

namespace YanehCheck.EpicGamesUtils.DAL.UnitOfWork;

public class UnitOfWorkFactory(IDbContextFactory<EpicGamesUtilsDbContext> dbContextFactory) : IUnitOfWorkFactory {
    public IUnitOfWork Create() => new UnitOfWork(dbContextFactory.CreateDbContext());
}
