using Microsoft.EntityFrameworkCore.Design;

namespace YanehCheck.EpicGamesUtils.DAL.Factories;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<EpicGamesUtilsDbContext> {
    private readonly DbContextSqLiteFactory dbContextSqLiteFactory = new("desdemona_toolkit.db");

    public EpicGamesUtilsDbContext CreateDbContext(string[] args) => dbContextSqLiteFactory.CreateDbContext();
}