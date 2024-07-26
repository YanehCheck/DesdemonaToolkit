using Wpf.Ui;
using Wpf.Ui.Controls;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.Interfaces;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.ViewModels.Pages;

public partial class HomeViewModel(ISnackbarService snackbarService, IBrowserService browserService, IPersistenceProvider persistenceProvider, ISessionService sessionService, IEpicGamesService epicGamesService) : ObservableObject, IViewModel, INavigationAware {
    private bool _isInitialized = false;

    [ObservableProperty] 
    private string _authorizationCode;

    [ObservableProperty]
    public DateTime _accessTokenExpiry;

    public void OnNavigatedTo() {
        if (!_isInitialized) {
            _isInitialized = true;
            InitializeViewModel();
        }
    }

    private void InitializeViewModel() {
        AccessTokenExpiry = persistenceProvider.AccessTokenExpiry;
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
}