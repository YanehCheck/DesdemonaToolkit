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
using YanehCheck.EpicGamesUtils.EgsApi.Api;
using YanehCheck.EpicGamesUtils.EgsApi.Service;
using YanehCheck.EpicGamesUtils.Utils.FortniteAssetSerializer;
using YanehCheck.EpicGamesUtils.Utils.FortniteAssetSerializer.Interfaces;
using YanehCheck.EpicGamesUtils.Utils.FortniteGgScraper;
using YanehCheck.EpicGamesUtils.WpfUiApp.Cli;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems.Mappers;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.Options;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.Persistence;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.Persistence.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.UI;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.UI.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Utilities.Options;
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
            builder.AddJsonFile("appsettings.json", false, true);
        })
        .ConfigureServices((context, services) => {
            services.ConfigureWritableOptions<UserOptions>(context.Configuration, context.Configuration.GetSection(UserOptions.Key));
            services.ConfigureWritableOptions<DalOptions>(context.Configuration, context.Configuration.GetSection(DalOptions.Key));
            services.ConfigureWritableOptions<ItemFetchOptions>(context.Configuration, context.Configuration.GetSection(ItemFetchOptions.Key));
            services.ConfigureWritableOptions<ItemImageCachingOptions>(context.Configuration, context.Configuration.GetSection(ItemImageCachingOptions.Key));
            services.ConfigureWritableOptions<ItemExportImageAppearanceOptions>(context.Configuration, context.Configuration.GetSection(ItemExportImageAppearanceOptions.Key));
            services.ConfigureWritableOptions<ItemExportImageFormatOptions>(context.Configuration, context.Configuration.GetSection(ItemExportImageFormatOptions.Key));
            services.ConfigureWritableOptions<ItemExportFortniteGgOptions>(context.Configuration, context.Configuration.GetSection(ItemExportFortniteGgOptions.Key));

            services.RegisterDalServices();
            services.RegisterBlServices();

            services.AddHostedService<ApplicationHostService>();

            services.AddTransient<IRestClient, RestClient>();

            // EGS API
            services.AddSingleton<ICachedEpicGamesClient, CachedEpicGamesClient>();
            services.AddSingleton<IEpicGamesService, EpicGamesService>();

            // Other fortnite
            services.AddTransient<IFortniteImageProvider, FortniteImageProvider>();
            services.AddTransient<FortniteGgItemMapper>();
            services.AddTransient<ItemStyleAssetMapper>();
            services.AddSingleton<IFortniteGgScrapper, FortniteGgScrapper>();
            services.AddSingleton<IFortniteItemProvider, FortniteItemProvider>();
            services.AddSingleton<IFortniteInventoryFortniteGgFetchProcessor, FortniteInventoryFortniteGgFetchProcessor>();
            services.AddSingleton<IFortniteInventoryImageProcessor, FortniteInventoryImageProcessor>();
            services.AddSingleton<ICustomFilterParser, CustomFilterParser>();
            services.AddSingleton<ICustomFilterProvider, CustomFilterProvider>();
            services.AddSingleton<IFortniteAssetSerializer, FortniteAssetSerializer>();
            services.AddSingleton<IFortniteStyleProvider, FortniteStyleProvider>();

            // General
            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<IBrowserService, BrowserService>();
            services.AddSingleton<IPersistenceProvider, PersistenceProvider>();
            services.AddSingleton<ISessionService, SessionService>();

            // UI services
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<IThemeService, ThemeService>();
            services.AddSingleton<ITaskBarService, TaskBarService>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<ISnackbarService, SnackbarService>();
            services.AddSingleton<IExtendedNavigationWindow, MainWindow>();
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

            // Cli
            services.AddTransient<ICliHandler, CliHandler>();
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
    private async void OnStartup(object sender, StartupEventArgs e) {
        await _host.StartAsync();

        if(e.Args.Length > 0) {
            await RunCliMode(e.Args);
            Shutdown();
        }
    }
    private async Task RunCliMode(string[] args) {
        using var scope = _host.Services.CreateScope();
        var cliHandler = scope.ServiceProvider.GetRequiredService<ICliHandler>();
        var mainWindow = scope.ServiceProvider.GetRequiredService<IExtendedNavigationWindow>();
        cliHandler.ShowConsole(); // Doesnt actually really work for now, but Ill use it atleast like an indicator
        mainWindow.HideWindow();
        await cliHandler.HandleAsync(args);
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