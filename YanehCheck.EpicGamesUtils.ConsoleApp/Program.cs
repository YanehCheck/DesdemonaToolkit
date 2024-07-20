using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using YanehCheck.EpicGamesUtils.ConsoleApp.Options;

namespace YanehCheck.EpicGamesUtils.ConsoleApp;

internal class Program {
    private static async Task Main(string[] args) {
        var hostBuilder = Host.CreateDefaultBuilder(args);
        hostBuilder.ConfigureServices((context, services) => {
                services.AddStandardServices();
                services.AddOptions<ItemOptions>();
            })
            .ConfigureAppConfiguration((context, config) => {
                config.AddJsonFile("appsettings.json", false, true);
            });
        var host = hostBuilder.Build();

        var app = host.Services.GetRequiredService<IConsoleApp>();
        await app.Run();
    }
}