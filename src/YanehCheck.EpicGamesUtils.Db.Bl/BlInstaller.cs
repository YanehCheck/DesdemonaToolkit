using Microsoft.Extensions.DependencyInjection;
using YanehCheck.EpicGamesUtils.Db.Bl.Facades.Interfaces;
using YanehCheck.EpicGamesUtils.Db.Bl.Mappers.Interfaces;
using YanehCheck.EpicGamesUtils.Db.Dal.UnitOfWork;

namespace YanehCheck.EpicGamesUtils.Db.Bl;

public static class BlInstaller {
    public static IServiceCollection RegisterBlServices(this IServiceCollection services) {
        services.AddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>();

        services.Scan(s => s
            .FromAssemblyOf<Bl>()
            .AddClasses(f => f.AssignableTo(typeof(IFacade<,>)))
            .AsMatchingInterface()
            .WithSingletonLifetime());

        services.Scan(s => s
            .FromAssemblyOf<Bl>()
            .AddClasses(f => f.AssignableTo(typeof(IModelMapper<,>)))
            .AsSelfWithInterfaces()
            .WithSingletonLifetime());

        return services;
    }
}
