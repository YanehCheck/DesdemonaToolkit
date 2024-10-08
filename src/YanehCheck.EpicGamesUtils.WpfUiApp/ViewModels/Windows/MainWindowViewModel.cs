﻿using System.Collections.ObjectModel;
using Wpf.Ui.Controls;
using YanehCheck.EpicGamesUtils.Db.Bl.Facades.Interfaces;
using YanehCheck.EpicGamesUtils.Db.Dal.Migrators;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.Persistence.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Views.Pages;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.ViewModels.Windows;

public partial class MainWindowViewModel : ObservableObject, IViewModel {
    public MainWindowViewModel(IDatabaseMigrator migrator, ISessionService sessionService, IItemFacade itemFacade) {
        // App setup
        migrator.Migrate();
        var dataReallyFetched = itemFacade.AnyAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        sessionService.IsItemDataFetched = dataReallyFetched;
    }

    [ObservableProperty]
    private string _applicationTitle = "Desdemona Toolkit";

    [ObservableProperty]
    private ObservableCollection<object> _menuItems = new()
    {
        new NavigationViewItem()
        {
            Content = "Home",
            Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
            TargetPageType = typeof(HomePage)
        },
        new NavigationViewItem()
        {
            Content = "Items",
            Icon = new SymbolIcon { Symbol = SymbolRegular.Tag24 },
            TargetPageType = typeof(ItemsPage)
        },
        new NavigationViewItem()
        {
            Content = "Account Tools",
            Icon = new SymbolIcon { Symbol = SymbolRegular.Wrench24 },
            TargetPageType = typeof(AccountUtilsPage)
        }
    };

    [ObservableProperty]
    private ObservableCollection<object> _footerMenuItems = new()
    {
        new NavigationViewItem()
        {
            Content = "Settings",
            Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
            TargetPageType = typeof(SettingsPage)
        }
    };

    [ObservableProperty]
    private ObservableCollection<MenuItem> _trayMenuItems = new()
    {
        new MenuItem { Header = "Home", Tag = "tray_home" }
    };
}