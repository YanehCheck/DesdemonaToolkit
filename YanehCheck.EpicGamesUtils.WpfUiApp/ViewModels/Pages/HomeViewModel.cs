using System.Collections.ObjectModel;
using Wpf.Ui;
using Wpf.Ui.Controls;
using YanehCheck.EpicGamesUtils.WpfUiApp.Helpers;
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
    IUriItemProvider uriItemProvider) : ObservableObject, IViewModel, INavigationAware {
    private bool _isInitialized = false;

    [ObservableProperty] 
    private string _authorizationCode;

    [ObservableProperty]
    public DateTime _accessTokenExpiry;

    [ObservableProperty]
    public DateTime _lastItemFetch;

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
    }

    public void OnNavigatedFrom() { }

    [RelayCommand]
    public async Task OnButtonConfirmClick() {
        if(AuthorizationCode is null or { Length: not 32}) {
            snackbarService.Show("Error", "The authorization code must be 32 characters long.", ControlAppearance.Caution, null, TimeSpan.FromSeconds(5));
            return;
        }

        var response = await epicGamesService.AuthenticateAccount(AuthorizationCode);
        if (response) {
            snackbarService.Show("Success", "Successfully authenticated", ControlAppearance.Success, null, TimeSpan.FromSeconds(5));

            persistenceProvider.AccountId = response.AccountId!;
            persistenceProvider.AccessToken = response.AccessToken!;
            persistenceProvider.AccessTokenExpiry = response.AccessTokenExpiry!.Value;
            persistenceProvider.Save();

            sessionService.AccountId = response.AccountId!;
            sessionService.AccessToken = response.AccessToken!;
            sessionService.AccessTokenExpiry = response.AccessTokenExpiry!.Value;

            AccessTokenExpiry = response.AccessTokenExpiry!.Value;
        }
        else {
            snackbarService.Show("Failure", response.ErrorMessage!, ControlAppearance.Danger, null, TimeSpan.FromSeconds(5));
        }
    }

    [RelayCommand]
    public void OnButtonGetCodeClick() {
        browserService.StartAndNavigateToAuthCode();
    }

    [RelayCommand]
    public async Task OnButtonFetchItemDataClick() {
        IFortniteItemProvider source = SelectedItemFetchSource switch {
            ItemFetchSource.FortniteGg => fortniteGgItemProvider,
            ItemFetchSource.Stable => uriItemProvider,
            _ => throw new ArgumentException("Invalid item source selected.")
        };

        var items = await source.GetItemsAsync();
        if (items == null) {
            snackbarService.Show("Failure", "Could not fetch item data.", ControlAppearance.Danger, null, TimeSpan.FromSeconds(5));
        }

        LastItemFetch = DateTime.Now;
        persistenceProvider.LastItemFetch = LastItemFetch;
        persistenceProvider.Save();
        sessionService.IsItemDataFetched = true;

        snackbarService.Show("Success", $"Successfully fetched {items!.Count()} items!", ControlAppearance.Success, null, TimeSpan.FromSeconds(5));
    }
}