using System.IO;
using System.Reflection;
using System.Windows.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RestSharp;
using Wpf.Ui;
using Wpf.Ui.Controls;
using YanehCheck.EpicGamesUtils.Db.Bl;
using YanehCheck.EpicGamesUtils.Db.Dal;
using YanehCheck.EpicGamesUtils.Utils.EgApiWrapper;
using YanehCheck.EpicGamesUtils.Utils.FortniteGgScraper;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames.Cache;
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
            services.AddOptions<ItemExportImageAppearanceOptions>().Bind(context.Configuration.GetSection(ItemExportImageAppearanceOptions.Key));
            services.AddOptions<ItemExportImageFormatOptions>().Bind(context.Configuration.GetSection(ItemExportImageFormatOptions.Key));

            services.RegisterDalServices();
            services.RegisterBlServices();

            services.AddHostedService<ApplicationHostService>();

            services.AddTransient<IRestClient, RestClient>();

            // EGS API
            services.AddSingleton<IEpicGamesClient, EpicGamesClient>();
            services.AddSingleton<EpicGamesService>();
            services.AddSingleton<ICachedEpicGamesService>(sp =>
                new CachedEpicGamesService(
                    sp.GetRequiredService<EpicGamesService>(),
                    new InMemoryCache()
                )
            );

            // Other fortnite
            services.AddTransient<IFortniteGgImageDownloader, FortniteGgImageDownloader>();
            services.AddTransient<IFortniteGgItemMapper, FortniteGgItemMapper>();
            services.AddSingleton<IFortniteGgScrapper, FortniteGgScrapper>();
            services.AddSingleton<IFortniteGgItemProvider, FortniteGgItemProvider>();
            services.AddSingleton<IFortniteInventoryImageProcessor, FortniteInventoryImageProcessor>();
            services.AddSingleton<IUriItemProvider, UriItemProvider>();
            services.AddSingleton<ICustomFilterParser, CustomFilterParser>();
            services.AddSingleton<ICustomFilterProvider, CustomFilterProvider>();

            // General
            services.AddSingleton<IFileSaveDialogService, FileSaveDialogService>();
            services.AddSingleton<IBrowserService, BrowserService>();
            services.AddSingleton<IPersistenceProvider, PersistenceProvider>();
            services.AddSingleton<ISessionService, SessionService>();

            // UI services
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<IThemeService, ThemeService>();
            services.AddSingleton<ITaskBarService, TaskBarService>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<ISnackbarService, SnackbarService>();
            services.AddSingleton<INavigationWindow, MainWindow>();
            services.Scan(s => s
                .FromAssembliesOf(typeof(App))
                .AddClasses(c => c.AssignableTo<IViewModel>())
                .AsSelfWithInterfaces()
                .WithSingletonLifetime());

            services.Scan(s => s
                .FromAssembliesOf(typeof(App))
                .AddClasses(c => c.AssignableTo(typeof(INavigableView<>)))
                .AsSelfWithInterfaces()
                .WithSingletonLifetime());
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