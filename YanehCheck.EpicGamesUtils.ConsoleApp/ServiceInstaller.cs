using Microsoft.Extensions.DependencyInjection;
using RestSharp;
using YanehCheck.EpicGamesUtils.Api;
using YanehCheck.EpicGamesUtils.ConsoleApp.Items;
using YanehCheck.EpicGamesUtils.FortniteGGScraper;

namespace YanehCheck.EpicGamesUtils.ConsoleApp;

public static class ServiceInstaller {
    public static IServiceCollection AddStandardServices(this IServiceCollection services) {
        services.AddTransient<IRestClient, RestClient>();
        services.AddTransient<IEpicGamesClient, EpicGamesClient>();
        services.AddTransient<IUriItemFetcher, UriItemFetcher>();
        services.AddTransient<IFortniteGgScrapper, FortniteGgScrapper>();
        services.AddTransient<IItemManager, ItemManager>();

        services.AddSingleton<IConsoleApp, ConsoleApp>();

        return services;
    }
}