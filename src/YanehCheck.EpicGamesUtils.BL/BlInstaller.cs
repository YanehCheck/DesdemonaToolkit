using Microsoft.Extensions.DependencyInjection;
using YanehCheck.EpicGamesUtils.BL.Facades.Interfaces;
using YanehCheck.EpicGamesUtils.BL.Mappers.Interfaces;
using YanehCheck.EpicGamesUtils.DAL.UnitOfWork;

namespace YanehCheck.EpicGamesUtils.BL;

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
