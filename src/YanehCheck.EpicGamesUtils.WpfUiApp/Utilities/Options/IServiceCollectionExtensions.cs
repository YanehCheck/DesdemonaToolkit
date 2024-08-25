using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using YanehCheck.EpicGamesUtils.WpfUiApp.Utilities.Options.Interfaces;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Utilities.Options;

public static class ServiceCollectionExtensions {
    public static void ConfigureWritableOptions<T>(
        this IServiceCollection services,
        IConfiguration configuration,
        IConfigurationSection section,
        string file = "appsettings.json") where T : class, new() {

        services.Configure<T>(section);
        
        services.AddTransient<IWritableOptions<T>>(provider =>
        {
            var environment = provider.GetService<IHostEnvironment>();
            var options = provider.GetService<IOptionsMonitor<T>>();
            var writer = new JsonOptionsWriter(environment!, (IConfigurationRoot) configuration, file);
            return new WritableOptions<T>(section.Key, writer, options!);
        });
    }
}