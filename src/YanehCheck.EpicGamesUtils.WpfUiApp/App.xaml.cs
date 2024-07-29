using System.IO;
using System.Reflection;
using System.Windows.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RestSharp;
using Wpf.Ui;
using Wpf.Ui.Controls;
using YanehCheck.EpicGamesUtils.Api;
using YanehCheck.EpicGamesUtils.BL;
using YanehCheck.EpicGamesUtils.DAL;
using YanehCheck.EpicGamesUtils.DAL.Migrators;
using YanehCheck.EpicGamesUtils.FortniteGGScraper;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems.Mappers;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.Options;
using YanehCheck.EpicGamesUtils.WpfUiApp.ViewModels;
using YanehCheck.EpicGamesUtils.WpfUiApp.Views.Windows;

namespace YanehCheck.EpicGamesUtils.WpfUiApp;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App {
    // The.NET Generic Host provides dependency injection, configuration, logging, and other services.
    // https://docs.microsoft.com/dotnet/core/extensions/generic-host
    // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
    // https://docs.microsoft.com/dotnet/core/extensions/configuration
    // https://docs.microsoft.com/dotnet/core/extensions/logging
    private static readonly IHost _host = Host
        .CreateDefaultBuilder()
        .ConfigureAppConfiguration((context, builder) => {
            builder.SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location));
            builder.AddJsonFile("appsettings.json", false, false);
        })
        .ConfigureServices((context, services) => {
            services.AddOptions<UserOptions>().Bind(context.Configuration.GetSection(UserOptions.Key));
            services.AddOptions<DalOptions>().Bind(context.Configuration.GetSection(DalOptions.Key));
            services.AddOptions<ItemFetchOptions>().Bind(context.Configuration.GetSection(ItemFetchOptions.Key));

            services.RegisterDalServices();
            services.RegisterBlServices();

            services.AddHostedService<ApplicationHostService>();

            services.AddTransient<IRestClient, RestClient>();
            services.AddSingleton<IEpicGamesClient, EpicGamesClient>();

            services.AddTransient<IFortniteGgImageDownloader, FortniteGgImageDownloader>();
            services.AddTransient<IFortniteGgItemMapper, FortniteGgItemMapper>();
            services.AddSingleton<IFortniteGgScrapper, FortniteGgScrapper>();
            services.AddSingleton<IFortniteGgItemProvider, FortniteGgItemProvider>();
            services.AddSingleton<IUriItemProvider, UriItemProvider>();

            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<IThemeService, ThemeService>();
            services.AddSingleton<ITaskBarService, TaskBarService>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<ISnackbarService, SnackbarService>();

            services.AddSingleton<IBrowserService, BrowserService>();
            services.AddSingleton<IPersistenceProvider, PersistenceProvider>();
            services.AddSingleton<ISessionService, SessionService>();
            services.AddSingleton<IEpicGamesService, EpicGamesService>();

            services.AddSingleton<INavigationWindow, MainWindow>();
            services.Scan(s => s
                .FromAssembliesOf(typeof(App))
                .AddClasses(c => c.AssignableTo<IViewModel>())
                .AsSelfWithInterfaces()
                .WithTransientLifetime());

            services.Scan(s => s
                .FromAssembliesOf(typeof(App))
                .AddClasses(c => c.AssignableTo(typeof(INavigableView<>)))
                .AsSelfWithInterfaces()
                .WithTransientLifetime());
        })
        .Build();

    /// <summary>
    /// Gets registered service.
    /// </summary>
    /// <typeparam name="T">Type of the service to get.</typeparam>
    /// <returns>Instance of the service or <see langword="null"/>.</returns>
    public static T GetService<T>()
        where T : class {
        return _host.Services.GetService(typeof(T)) as T;
    }

    /// <summary>
    /// Occurs when the application is loading.
    /// </summary>
    private void OnStartup(object sender, StartupEventArgs e) {
        _host.Start();
        var migrator = GetService<IDatabaseMigrator>();
        migrator.Migrate();
    }

    /// <summary>
    /// Occurs when the application is closing.
    /// </summary>
    private async void OnExit(object sender, ExitEventArgs e) {
        await _host.StopAsync();

        _host.Dispose();
    }

    /// <summary>
    /// Occurs when an exception is thrown by an application but not handled.
    /// </summary>
    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e) {
        // For more info see https://docs.microsoft.com/en-us/dotnet/api/system.windows.application.dispatcherunhandledexception?view=windowsdesktop-6.0
    }
}