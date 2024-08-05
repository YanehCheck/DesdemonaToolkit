using System.Collections.ObjectModel;
using Wpf.Ui;
using Wpf.Ui.Controls;
using YanehCheck.EpicGamesUtils.BL.Facades.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Helpers.Enums;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.Interfaces;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.ViewModels.Pages;

public partial class HomeViewModel(ISnackbarService snackbarService, 
    IBrowserService browserService,
    IPersistenceProvider persistenceProvider, 
    ISessionService sessionService, 
    IEpicGamesService epicGamesService,
    IFortniteGgItemProvider fortniteGgItemProvider,
    IUriItemProvider uriItemProvider,
    IItemFacade itemFacade) : ObservableObject, IViewModel, INavigationAware {
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
    private ItemFetchSource selectedItemFetchSource = ItemFetchSource.FortniteGg;

    public ObservableCollection<ItemFetchSource> ItemFetchSources { get; set; } = new(Enum.GetValues<ItemFetchSource>());


    public void OnNavigatedTo() {
        if (!_isInitialized) {
            _isInitialized = true;
            InitializeViewModel();
        }
    }

    private void InitializeViewModel() {
        AccessTokenExpiry = persistenceProvider.AccessTokenExpiry;
        LastItemFetch = persistenceProvider.LastItemFetch;
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
    }

    [RelayCommand]
    public void OnButtonGetCodeClick() {
        browserService.StartAndNavigateToAuthCodeWebsite();
    }

    [RelayCommand]
    public void OnHyperlinkDataFolderClick() => browserService.StartAndNavigateToDataDirectory();

    [RelayCommand]
    public async Task OnButtonFetchItemDataClick() {
        IFortniteItemProvider source = SelectedItemFetchSource switch {
            ItemFetchSource.FortniteGg => fortniteGgItemProvider,
            ItemFetchSource.Stable => uriItemProvider,
            _ => throw new ArgumentException("Invalid item source selected.")
        };

        var items = await source.GetItemsAsync();
        if (items == null) {
            snackbarService.Show("Failure",
                "An error occured while fetching item data. Please check your connection or use different source.",
                ControlAppearance.Danger, null, TimeSpan.FromSeconds(5));
            return;
        }

        try {
            await itemFacade.SaveByFortniteIdAsync(items);

            LastItemFetch = DateTime.Now;
            persistenceProvider.LastItemFetch = LastItemFetch;
            persistenceProvider.Save();
            sessionService.IsItemDataFetched = true;

            snackbarService.Show("Success", $"Successfully fetched {items!.Count()} items!", ControlAppearance.Success,
                null, TimeSpan.FromSeconds(5));
        }
        catch (Exception ex) {
            snackbarService.Show("Failure", $"An error occured while saving item data to database.\nError: {ex.Message}", ControlAppearance.Danger, null, TimeSpan.FromSeconds(5));
        }
    }
}