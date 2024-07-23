using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using YanehCheck.EpicGamesUtils.DAL.Factories;
using YanehCheck.EpicGamesUtils.DAL.Mappers;
using YanehCheck.EpicGamesUtils.DAL.Migrators;

namespace YanehCheck.EpicGamesUtils.DAL;

public static class DalInstaller {
    public static IServiceCollection RegisterDalServices(this IServiceCollection services, DalOptions options) {
        services.AddSingleton<DalOptions>(options);

        services.AddSingleton<IDbContextFactory<EpicGamesUtilsDbContext>>(_ =>
            new DbContextSqLiteFactory(options.DatabasePath, options.SeedDatabase));
        services.AddSingleton<IDatabaseMigrator, DatabaseMigrator>();

        services.Scan(s => s
            .FromAssemblyOf<Dal>()
            .AddClasses(f => f.AssignableTo(typeof(IEntityMapper<>)))
            .AsSelfWithInterfaces()
            .WithSingletonLifetime());

        return services;
    }
}
