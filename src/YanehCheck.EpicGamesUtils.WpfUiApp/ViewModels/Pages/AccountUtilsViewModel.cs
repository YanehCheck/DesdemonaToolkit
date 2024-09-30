using System.ComponentModel;
using Wpf.Ui;
using Wpf.Ui.Controls;
using YanehCheck.EpicGamesUtils.EgsApi.Service;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.Persistence.Interfaces;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.ViewModels.Pages;

public partial class AccountUtilsViewModel(
    IEpicGamesService epicGamesService,
    ISnackbarService snackbarService,
    ISessionService sessionService) : ObservableObject, IViewModel, INotifyPropertyChanged {

    [ObservableProperty]
    private string _code = string.Empty;

    private bool AuthCheck() {
        if(!sessionService.IsAuthenticated) {
            snackbarService.Show(
                "Failure",
                "No account authenticated. Please authenticate on the Home page.",
                ControlAppearance.Danger,
                null,
                TimeSpan.FromSeconds(5));
            return false;
        }
        return true;
    }

    [RelayCommand]
    private async Task RedeemCode() {
        if(!AuthCheck()) {
            return;
        }

        var realCode = string.Concat(Code.Split(' ', '-'));

        /* TODO: Some research on format
        if(realCode is not { Length: 16 or 20 }) { 
        }*/
        try {
            await epicGamesService.RedeemCodeAccount(sessionService.AccountId!, sessionService.AccessToken!, realCode);
        }
        catch(Exception ex) {
            snackbarService.Show(
                  "Failure",
                  ex.Message,
                  ControlAppearance.Danger,
                  null,
                  TimeSpan.FromSeconds(5));
            return;
        }
        snackbarService.Show(
                  "Success",
                  "Code redeemed.",
                  ControlAppearance.Success,
                  null,
                  TimeSpan.FromSeconds(5));
    }
}