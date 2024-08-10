using Wpf.Ui;
using Wpf.Ui.Controls;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.Interfaces;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.ViewModels.Pages;

public partial class AccountViewModel(ISessionService sessionService, ISnackbarService snackbarService) : ObservableObject, IViewModel, INavigationAware {
    private string _initializedForAccountId = "";

    public void OnNavigatedTo() {
        if(_initializedForAccountId != sessionService.AccountId) {
            InitializeViewModel();
        }
    }

    public void OnNavigatedFrom() { }

    private void InitializeViewModel() {
        if(!sessionService.IsAuthenticated) {
            snackbarService.Show(
                "Failure",
                "No account authenticated. Please authenticate on the Home page.",
                ControlAppearance.Danger,
                null,
                TimeSpan.FromSeconds(5));
            return;
        }
        _initializedForAccountId = sessionService.AccountId!;
    }
}