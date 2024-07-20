namespace YanehCheck.EpicGamesUtils.WpfUiApp.ViewModels.Pages;

public partial class DashboardViewModel : ObservableObject, IViewModel {
    [ObservableProperty]
    private int _counter = 0;

    [RelayCommand]
    private void OnCounterIncrement() {
        Counter++;
    }
}