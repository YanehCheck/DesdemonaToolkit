using Microsoft.EntityFrameworkCore;

namespace YanehCheck.EpicGamesUtils.DAL.Factories;

public class DbContextSqLiteFactory : IDbContextFactory<EpicGamesUtilsDbContext> {
    private readonly bool seedDatabase;
    private readonly DbContextOptionsBuilder<EpicGamesUtilsDbContext> contextOptionsBuilder = new();

    public DbContextSqLiteFactory(DalOptions options) {
        seedDatabase = options.SeedDatabase;
        contextOptionsBuilder.UseSqlite($"Data Source={options.DatabasePath};Cache=Shared");
    }

    public EpicGamesUtilsDbContext CreateDbContext() => new(contextOptionsBuilder.Options, seedDatabase);
}
