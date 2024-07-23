using Microsoft.EntityFrameworkCore;

namespace YanehCheck.EpicGamesUtils.DAL.Factories;

public class DbContextSqLiteFactory : IDbContextFactory<EpicGamesUtilsDbContext> {
    private readonly bool seedTestingData;
    private readonly DbContextOptionsBuilder<EpicGamesUtilsDbContext> contextOptionsBuilder = new();

    public DbContextSqLiteFactory(string databaseName, bool seedTestingData = false) {
        this.seedTestingData = seedTestingData;
        contextOptionsBuilder.UseSqlite($"Data Source={databaseName};Cache=Shared");
    }

    public EpicGamesUtilsDbContext CreateDbContext() => new(contextOptionsBuilder.Options, seedTestingData);
}
