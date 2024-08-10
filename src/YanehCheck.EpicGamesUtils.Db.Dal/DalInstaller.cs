using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using YanehCheck.EpicGamesUtils.Db.Dal.Factories;
using YanehCheck.EpicGamesUtils.Db.Dal.Mappers;
using YanehCheck.EpicGamesUtils.Db.Dal.Migrators;

namespace YanehCheck.EpicGamesUtils.Db.Dal;

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
