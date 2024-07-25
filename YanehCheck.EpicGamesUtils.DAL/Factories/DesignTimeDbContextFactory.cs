using Microsoft.EntityFrameworkCore.Design;

namespace YanehCheck.EpicGamesUtils.DAL.Factories;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<EpicGamesUtilsDbContext> {
    private readonly DbContextSqLiteFactory dbContextSqLiteFactory = new(new DalOptions());

    public EpicGamesUtilsDbContext CreateDbContext(string[] args) => dbContextSqLiteFactory.CreateDbContext();
}