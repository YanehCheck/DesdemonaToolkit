using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using YanehCheck.EpicGamesUtils.DAL.Factories;
using YanehCheck.EpicGamesUtils.DAL.Mappers;
using YanehCheck.EpicGamesUtils.DAL.Migrators;

namespace YanehCheck.EpicGamesUtils.DAL;

public static class DalInstaller {
    public static IServiceCollection RegisterDalServices(this IServiceCollection services) { ;
        services.AddSingleton<IDbContextFactory<EpicGamesUtilsDbContext>, DbContextSqLiteFactory>();
        services.AddSingleton<IDatabaseMigrator, DatabaseMigrator>();

        services.Scan(s => s
            .FromAssemblyOf<Dal>()
            .AddClasses(f => f.AssignableTo(typeof(IEntityMapper<>)))
            .AsSelfWithInterfaces()
            .WithSingletonLifetime());

        return services;
    }
}
