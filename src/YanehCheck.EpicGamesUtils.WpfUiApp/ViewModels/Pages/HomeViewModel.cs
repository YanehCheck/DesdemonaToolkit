using System.Collections.ObjectModel;
using System.IO;
using Wpf.Ui;
using Wpf.Ui.Controls;
using YanehCheck.EpicGamesUtils.Db.Bl.Facades.Interfaces;
using YanehCheck.EpicGamesUtils.Db.Bl.Models;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.Persistence.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.UI.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Types.Enums;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.ViewModels.Pages;

public partial class HomeViewModel(ISnackbarService snackbarService, 
    IBrowserService browserService,
    IPersistenceProvider persistenceProvider, 
    ISessionService sessionService, 
    ICachedEpicGamesService epicGamesService,
    IFortniteItemProvider itemProvider,
    IItemFacade itemFacade,
    IItemStyleFacade itemStyleFacade,
    IFortniteStyleProvider styleProvider,
    IDialogService dialogService) : ObservableObject, IViewModel, INavigationAware {
    private bool _isInitialized = false;

    [ObservableProperty] 
    private string _authorizationCode;

    [ObservableProperty]
    public DateTime _accessTokenExpiry;

    [ObservableProperty]
    public DateTime _lastItemFetch;

    [ObservableProperty] 
    public string _displayName;

    [ObservableProperty]
    private ItemFetchSource selectedItemFetchSource = ItemFetchSource.AllBundled;

    public ObservableCollection<ItemFetchSource> ItemFetchSources { get; set; } = new(Enum.GetValues<ItemFetchSource>());

    [ObservableProperty]
    private bool _fetchingData = false;

    [ObservableProperty] 
    private int _fetchProgressPercentage = 0;


    public void OnNavigatedTo() {
        if (!_isInitialized) {
            _isInitialized = true;
            InitializeViewModel();
        }
    }

    private void InitializeViewModel() {
        AccessTokenExpiry = persistenceProvider.AccessTokenExpiry;
        if (AccessTokenExpiry > DateTime.Now) {
            Task.Run(() => epicGamesService.PreCacheAll(sessionService.AccountId, sessionService.AccessToken));
        }

        LastItemFetch = sessionService.IsItemDataFetched ? 
            persistenceProvider.LastItemFetch :
            DateTime.MinValue;
        DisplayName = persistenceProvider.DisplayName;
    }

    public void OnNavigatedFrom() { }

    [RelayCommand]
    public async Task OnButtonConfirmClick() {
        if(AuthorizationCode is null or { Length: not 32}) {
            snackbarService.Show("Error", "Authorization code must be 32 characters long.", ControlAppearance.Caution, null, TimeSpan.FromSeconds(5));
            return;
        }

        var resultAuth = await epicGamesService.AuthenticateAccount(AuthorizationCode);
        if (resultAuth) {
            persistenceProvider.AccountId = resultAuth.AccountId!;
            persistenceProvider.AccessToken = resultAuth.AccessToken!;
            persistenceProvider.AccessTokenExpiry = resultAuth.AccessTokenExpiry!.Value;
            persistenceProvider.DisplayName = resultAuth.DisplayName!;

            sessionService.AccountId = resultAuth.AccountId!;
            sessionService.AccessToken = resultAuth.AccessToken!;
            sessionService.AccessTokenExpiry = resultAuth.AccessTokenExpiry!.Value;
            sessionService.DisplayName = resultAuth.DisplayName;

            DisplayName = resultAuth.DisplayName!;
            AccessTokenExpiry = resultAuth.AccessTokenExpiry!.Value;

            snackbarService.Show("Success", $"Successfully authenticated as {resultAuth.DisplayName}.", ControlAppearance.Success, null, TimeSpan.FromSeconds(5));
        }
        else {
            snackbarService.Show("Failure", resultAuth.ErrorMessage!, ControlAppearance.Danger, null, TimeSpan.FromSeconds(5));
            return;
        }

        persistenceProvider.Save();

        // Precache information for other pages
        await Task.Run(() => epicGamesService.PreCacheAll(sessionService.AccountId, sessionService.AccessToken));
    }

    [RelayCommand]
    public void OnButtonGetCodeClick() {
        browserService.StartAndNavigateToAuthCodeWebsite();
    }

    [RelayCommand]
    public void OnHyperlinkDataFolderClick() => browserService.StartAndNavigateToDataDirectory();

    [RelayCommand]
    public async Task OnButtonFetchItemDataClick() {
        FetchingData = true;

        if (SelectedItemFetchSource is ItemFetchSource.ItemsFortniteGg or ItemFetchSource.ItemsStable or ItemFetchSource.AllBundled) {
            await FetchItemData();
        }
        if (SelectedItemFetchSource is ItemFetchSource.StylesDirectoryProperties or ItemFetchSource.AllBundled) {
            await FetchStyleData();
        }

        FetchingData = false;
        FetchProgressPercentage = 0;
    }

    private async Task FetchItemData() {
        Action<double> updateProgress = progress => {
            FetchProgressPercentage = (int) (progress * 100);
        };

        var path = Path.Combine(Path.GetDirectoryName(Environment.ProcessPath)!, "data/bundled_items.json");
        if (!File.Exists(path)) {
            snackbarService.Show("Failure",
                "An error occured while loading item data. Bundled items file not found.",
                ControlAppearance.Danger, null, TimeSpan.FromSeconds(5));
            return;
        }

        var items = SelectedItemFetchSource switch {
            ItemFetchSource.AllBundled => await itemProvider.GetItemsJsonFileAsync(path, updateProgress),
            ItemFetchSource.ItemsFortniteGg => await itemProvider.GetItemsFortniteGgAsync(updateProgress),
            ItemFetchSource.ItemsStable => await itemProvider.GetItemsStableUriAsync(updateProgress)
        };

        if(items == null) {
            snackbarService.Show("Failure",
                "An error occured while fetching item data. Please check your connection or use different source.",
                ControlAppearance.Danger, null, TimeSpan.FromSeconds(5));
            return;
        }

        try {
            var filteredItems = items.Where(i => i.FortniteId != null!).DistinctBy(i => i.FortniteId);
            await itemFacade.SaveByFortniteIdAsync(filteredItems);

            LastItemFetch = DateTime.Now;
            persistenceProvider.LastItemFetch = LastItemFetch;
            persistenceProvider.Save();
            sessionService.IsItemDataFetched = true;
            snackbarService.Show("Success", $"Successfully fetched {items!.Count()} items!", ControlAppearance.Success,
                null, TimeSpan.FromSeconds(5));
        }
        catch(Exception ex) {
            snackbarService.Show("Failure", $"An error occured while saving item data to database.\nError: {ex.Message}", ControlAppearance.Danger, null, TimeSpan.FromSeconds(5));
        }
    }

    private async Task FetchStyleData() {
        IReadOnlyCollection<ItemStyleModel> styles;
        if (SelectedItemFetchSource == ItemFetchSource.StylesDirectoryProperties) {
            var directory = dialogService.OpenFolder();
            if (directory == null) {
                return;
            }

            styles = await styleProvider.GetStylesPackagePropertiesFileRecursiveAsync(directory,
                progress => { FetchProgressPercentage = (int) (progress * 100); });
        }
        else { //(SelectedItemFetchSource == ItemFetchSource.StylesBundledWithApp)
            var path = Path.Combine(Path.GetDirectoryName(Environment.ProcessPath)!, "data/bundled_styles.json");
            if (File.Exists(path)) {
                var result = await styleProvider.GetStylesJsonFileAsync(path, progress => { FetchProgressPercentage = (int) (progress * 100); });
                if (result != null) {
                    styles = result;
                }
                else {
                    snackbarService.Show("Failure",
                        "An error occured while fetching style data.",
                        ControlAppearance.Danger, null, TimeSpan.FromSeconds(5));
                    return;
                }
            }
            else {
                snackbarService.Show("Failure",
                    "An error occured while loading style data. Bundled styles file not found.",
                    ControlAppearance.Danger, null, TimeSpan.FromSeconds(5));
                return;
            }
        }

        try {
            var filteredStyles = styles.Where(i => i.FortniteId != null!).DistinctBy(i => i.FortniteId);
            await itemStyleFacade.SaveByFortniteIdAsync(filteredStyles);
        }
        catch(Exception ex) {
            snackbarService.Show("Failure", $"An error occured while saving item data to database.\nError: {ex.Message}", ControlAppearance.Danger, null, TimeSpan.FromSeconds(5));
        }
    }
}