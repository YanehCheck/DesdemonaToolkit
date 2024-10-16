﻿using System.ComponentModel;
using System.Reflection;
using Wpf.Ui;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using YanehCheck.EpicGamesUtils.EgsApi.Service;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.Options;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.Persistence.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.UI.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Types.Enums;
using YanehCheck.EpicGamesUtils.WpfUiApp.Utilities.Options.Interfaces;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.ViewModels.Pages;

public partial class SettingsViewModel(IBrowserService browserService,
    IEpicGamesService epicGamesService,
    ISnackbarService snackbarService,
    ISessionService sessionService,
    IWritableOptions<ItemImageCachingOptions> itemImageCacheOptions,
    IWritableOptions<ItemFetchOptions> itemFetchOptions,
    IWritableOptions<ItemExportImageFormatOptions> itemExportFormatOptions,
    IWritableOptions<ItemExportImageAppearanceOptions> itemExportAppearanceOptions,
    IWritableOptions<ItemExportFortniteGgOptions> itemExportFortniteGgOptions) : ObservableObject, IViewModel, INavigationAware, INotifyPropertyChanged {
    public string SacCode => "DESDEMONATOOLKIT";
    private bool _isInitialized = false;

    [ObservableProperty]
    private string _appVersion = string.Empty;

    [ObservableProperty]
    private ApplicationTheme _currentTheme = ApplicationTheme.Unknown;

    public bool CacheDownloadedImages {
        get => itemImageCacheOptions.Value.CacheDownloadedImages;
        set {
            if(value != itemImageCacheOptions.Value.CacheDownloadedImages) {
                itemImageCacheOptions.Update(o => o.CacheDownloadedImages = value);
                OnPropertyChanged();
            }
        }
    }

    public string StableSourceUri {
        get => itemFetchOptions.Value.StableSourceUri;
        set {
            if(value != itemFetchOptions.Value.StableSourceUri) {
                itemFetchOptions.Update(o => o.StableSourceUri = value);
                OnPropertyChanged();
            }
        }
    }

    public int FortniteGgIdRange {
        get => itemFetchOptions.Value.FortniteGgIdRange;
        set {
            if(value != itemFetchOptions.Value.FortniteGgIdRange) {
                itemFetchOptions.Update(o => o.FortniteGgIdRange = value);
                OnPropertyChanged();
            }
        }
    }

    public ImageFormat ImageFormat {
        get => itemExportFormatOptions.Value.ImageFormat;
        set {
            if(value != itemExportFormatOptions.Value.ImageFormat) {
                itemExportFormatOptions.Update(o => o.ImageFormat = value);
                OnPropertyChanged();
            }
        }
    }

    public IList<ImageFormat> ImageFormatValues => (ImageFormat[]) Enum.GetValues(typeof(ImageFormat));

    public int ImageJpegQuality {
        get => itemExportFormatOptions.Value.ImageJpegQuality;
        set {
            if(value != itemExportFormatOptions.Value.ImageJpegQuality) {
                itemExportFormatOptions.Update(o => o.ImageJpegQuality = value);
                OnPropertyChanged();
            }
        }
    }

    public bool ImageIncludeDisplayName {
        get => itemExportAppearanceOptions.Value.StampIncludePlayerName;
        set {
            if(value != itemExportAppearanceOptions.Value.StampIncludePlayerName) {
                itemExportAppearanceOptions.Update(o => o.StampIncludePlayerName = value);
                OnPropertyChanged();
            }
        }
    }

    public bool ImageCensorDisplayName {
        get => itemExportAppearanceOptions.Value.StampCensorPlayerName;
        set {
            if(value != itemExportAppearanceOptions.Value.StampCensorPlayerName) {
                itemExportAppearanceOptions.Update(o => o.StampCensorPlayerName = value);
                OnPropertyChanged();
            }
        }
    }

    public bool ImageIncludeDate {
        get => itemExportAppearanceOptions.Value.StampIncludeDate;
        set {
            if(value != itemExportAppearanceOptions.Value.StampIncludeDate) {
                itemExportAppearanceOptions.Update(o => o.StampIncludeDate = value);
                OnPropertyChanged();
            }
        }
    }

    public bool ImageIncludeItemCount {
        get => itemExportAppearanceOptions.Value.StampIncludeItemCount;
        set {
            if(value != itemExportAppearanceOptions.Value.StampIncludeItemCount) {
                itemExportAppearanceOptions.Update(o => o.StampIncludeItemCount = value);
                OnPropertyChanged();
            }
        }
    }

    public bool ImageIncludeCustomText {
        get => itemExportAppearanceOptions.Value.StampIncludeCustomText;
        set {
            if(value != itemExportAppearanceOptions.Value.StampIncludeCustomText) {
                itemExportAppearanceOptions.Update(o => o.StampIncludeCustomText = value);
                OnPropertyChanged();
            }
        }
    }

    public string ImageCustomText {
        get => itemExportAppearanceOptions.Value.StampCustomText;
        set {
            if(value != itemExportAppearanceOptions.Value.StampCustomText) {
                itemExportAppearanceOptions.Update(o => o.StampCustomText = value);
                OnPropertyChanged();
            }
        }
    }

    public bool ImageIncludeItemRemarks {
        get => itemExportAppearanceOptions.Value.ItemIncludeRemark;
        set {
            if(value != itemExportAppearanceOptions.Value.ItemIncludeRemark) {
                itemExportAppearanceOptions.Update(o => o.ItemIncludeRemark = value);
                OnPropertyChanged();
            }
        }
    }

    public int ImageItemsPerRow {
        get => itemExportAppearanceOptions.Value.ItemsPerRow;
        set {
            if(value != itemExportAppearanceOptions.Value.ItemsPerRow) {
                itemExportAppearanceOptions.Update(o => o.ItemsPerRow = value);
                OnPropertyChanged();
            }
        }
    }

    public int FortniteGgRequestsPerItem {
        get => itemExportFortniteGgOptions.Value.RequestsPerItem;
        set {
            if(value != itemExportFortniteGgOptions.Value.RequestsPerItem) {
                itemExportFortniteGgOptions.Update(o => o.RequestsPerItem = value);
                OnPropertyChanged();
            }
        }
    }

    public void OnNavigatedTo() {
        if(!_isInitialized) {
            InitializeViewModel();
        }
    }

    public void OnNavigatedFrom() { }

    private void InitializeViewModel() {
        CurrentTheme = ApplicationThemeManager.GetAppTheme();
        var fullVersion = GetAssemblyVersion();
        AppVersion = $"Development version: {fullVersion[..^2]}";

        _isInitialized = true;
    }

    private string GetAssemblyVersion() {
        return Assembly.GetExecutingAssembly().GetName().Version?.ToString()
               ?? string.Empty;
    }

    [RelayCommand]
    private async Task UseSacCode() {
        if (!sessionService.IsAuthenticated) {
            snackbarService.Show(
                "Failure",
                "No account authenticated. Please authenticate on the Home page.",
                ControlAppearance.Danger,
                null,
                TimeSpan.FromSeconds(5));
            return;
        }

        try {
            await epicGamesService.UseSacCode(sessionService.AccountId!, sessionService.AccessToken!, SacCode);
        }
        catch (Exception ex) {
            snackbarService.Show(
                "Failure",
                $"{ex.Message}",
                ControlAppearance.Danger,
                null,
                TimeSpan.FromSeconds(5));
            return;
        }

        snackbarService.Show(
            "Success",
            "Thank you for supporting me!",
            ControlAppearance.Success,
            null,
            TimeSpan.FromSeconds(5));
    }

    [RelayCommand]
    private void OpenAppSettingsFile() {
        browserService.StartAndNavigateToAppSettings();
    }

    [RelayCommand]
    private void OnChangeTheme(string parameter) {
        switch(parameter) {
            case "theme_light":
                if(CurrentTheme == ApplicationTheme.Light) {
                    break;
                }

                ApplicationThemeManager.Apply(ApplicationTheme.Light);
                CurrentTheme = ApplicationTheme.Light;

                break;

            default:
                if(CurrentTheme == ApplicationTheme.Dark) {
                    break;
                }

                ApplicationThemeManager.Apply(ApplicationTheme.Dark);
                CurrentTheme = ApplicationTheme.Dark;

                break;
        }
    }
}