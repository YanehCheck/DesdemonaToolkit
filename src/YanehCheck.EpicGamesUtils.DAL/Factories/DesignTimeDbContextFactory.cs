using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;

namespace YanehCheck.EpicGamesUtils.DAL.Factories;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<EpicGamesUtilsDbContext> {
    private readonly DbContextSqLiteFactory dbContextSqLiteFactory = new(Options.Create(new DalOptions()));

    public EpicGamesUtilsDbContext CreateDbContext(string[] args) => dbContextSqLiteFactory.CreateDbContext();
}