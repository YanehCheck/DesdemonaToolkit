using YanehCheck.EpicGamesUtils.WpfUiApp.Services.Interfaces;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.ViewModels.Pages;

public partial class HomeViewModel(IBrowserService browserService) : ObservableObject, IViewModel {
    [ObservableProperty] 
    private string _authorizationCode;

    [RelayCommand]
    public async Task OnButtonConfirmClick() {

    }

    [RelayCommand]
    public void OnButtonGetCodeClick() {
        browserService.StartAndNavigateToAuthCode();
    }
}