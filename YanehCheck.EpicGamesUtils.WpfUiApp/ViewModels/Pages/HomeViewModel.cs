using Wpf.Ui;
using Wpf.Ui.Controls;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.Interfaces;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.ViewModels.Pages;

public partial class HomeViewModel(ISnackbarService snackbarService, IBrowserService browserService, ISettingsProvider settingsProvider, IEpicGamesService epicGamesService) : ObservableObject, IViewModel, INavigationAware {
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
        AccessTokenExpiry = settingsProvider.AccessTokenExpiry;
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
            settingsProvider.AccessToken = response.AccessToken!;
            settingsProvider.AccessTokenExpiry = response.AccessTokenExpiry!.Value;
            AccessTokenExpiry = response.AccessTokenExpiry!.Value;
            settingsProvider.Save();
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