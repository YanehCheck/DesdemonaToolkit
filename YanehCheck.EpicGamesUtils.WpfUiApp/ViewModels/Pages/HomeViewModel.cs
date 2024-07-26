using Wpf.Ui.Controls;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.Interfaces;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.ViewModels.Pages;

public partial class HomeViewModel(IBrowserService browserService, ISettingsProvider settingsProvider) : ObservableObject, IViewModel, INavigationAware {
    private readonly bool _isInitialized = false;

    [ObservableProperty] 
    private string _authorizationCode;
    public DateTime AccessTokenExpiry => settingsProvider.AccessTokenExpiry;

    public void OnNavigatedTo() {
        if (!_isInitialized) {

        }
    }

    private void InitializeViewModel() {
    }

    public void OnNavigatedFrom() { }

    [RelayCommand]
    public async Task OnButtonConfirmClick() {
        settingsProvider.AccessToken = AuthorizationCode;
        settingsProvider.Save();
    }

    [RelayCommand]
    public void OnButtonGetCodeClick() {
        browserService.StartAndNavigateToAuthCode();
    }
}