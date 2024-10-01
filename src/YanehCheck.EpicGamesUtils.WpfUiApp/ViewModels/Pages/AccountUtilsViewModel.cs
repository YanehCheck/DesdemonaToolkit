using System.ComponentModel;
using Wpf.Ui;
using Wpf.Ui.Controls;
using YanehCheck.EpicGamesUtils.EgsApi.Api.Exceptions;
using YanehCheck.EpicGamesUtils.EgsApi.Service;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.Persistence.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.UI;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.UI.Interfaces;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.ViewModels.Pages;

public partial class AccountUtilsViewModel(
    IEpicGamesService epicGamesService,
    ISnackbarService snackbarService,
    ISessionService sessionService,
    IClipboardService clipboardService) : ObservableObject, IViewModel, INotifyPropertyChanged, INavigationAware {

    [ObservableProperty] private string _code = string.Empty;
    [ObservableProperty] private bool _monitorClipboardRedeemCode;

    private bool _initialized;

    public void OnNavigatedTo() {
        if(!_initialized) {
            clipboardService.OnClipboardChanged += async (s, a) => await OnClipboardChanged(s, a);
            _initialized = true;
        }
    }

    public void OnNavigatedFrom() { }

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

    private async Task OnClipboardChanged(object? sender, ClipboardService.ClipboardChangedEventArgs args) {
        if(args.Type != ClipboardService.ContentType.Text) {
            return;
        }

        var content = (args.Content as string)!;
        var realCode = string.Concat(content.Split(' ', '-'));
        if(realCode is not { Length: > 5 and < 30 }) {
            return;
        }
        if (!realCode.All(char.IsAsciiLetterOrDigit)) {
            return;
        }

        try {
            await epicGamesService.RedeemCodeAccount(sessionService.AccountId!, sessionService.AccessToken!, realCode);
        }
        catch(EpicGamesApiException ex) {
            switch (ex.ErrorCode) {
                // Let's ignore not found.
                case "errors.com.epicgames.coderedemption.code_not_found":
                    return;
                // These should probably be alerted though
                // case "errors.com.epicgames.coderedemption.code_expired":
                // case "errors.com.epicgames.common.throttled":
                // case "errors.com.epicgames.coderedemption.code_used":
                default:
                    await ShowSnackbarOnUIThread(
                        "Failure",
                        ex.Message,
                        ControlAppearance.Danger,
                        TimeSpan.FromSeconds(10));
                    return;
            }
        }
        await ShowSnackbarOnUIThread(
            "Success",
            "Code redeemed.",
            ControlAppearance.Success,
            TimeSpan.FromSeconds(15));
    }

    private Task ShowSnackbarOnUIThread(string title, string message, ControlAppearance appearance, TimeSpan duration) {
        return Application.Current.Dispatcher.InvokeAsync(() =>
        {
            snackbarService.Show(title, message, appearance, null, duration);
        }).Task;
    }



    [RelayCommand]
    private void ToggleRedeemCodeMonitorClipboard() {
        if(MonitorClipboardRedeemCode) {
            clipboardService.Start();
        }
        else {
            clipboardService.Stop();
        }
    }

    [RelayCommand]
    private async Task RedeemCode() {
        if(!AuthCheck()) {
            return;
        }

        var realCode = string.Concat(Code.Split(' ', '-'));

        // TODO: Format check?
        // Maybe it's better to keep this more permissive?s

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