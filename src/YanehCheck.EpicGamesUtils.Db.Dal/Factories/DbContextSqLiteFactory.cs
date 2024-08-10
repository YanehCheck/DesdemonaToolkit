using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace YanehCheck.EpicGamesUtils.Db.Dal.Factories;

public class DbContextSqLiteFactory : IDbContextFactory<EpicGamesUtilsDbContext> {
    private readonly bool seedDatabase;
    private readonly DbContextOptionsBuilder<EpicGamesUtilsDbContext> contextOptionsBuilder = new();

    public DbContextSqLiteFactory(IOptions<DalOptions> options) {
        seedDatabase = options.Value.SeedDatabase;
        contextOptionsBuilder.UseSqlite($"Data Source={options.Value.DatabasePath};Cache=Shared");
    }

    public EpicGamesUtilsDbContext CreateDbContext() => new(contextOptionsBuilder.Options, seedDatabase);
}
