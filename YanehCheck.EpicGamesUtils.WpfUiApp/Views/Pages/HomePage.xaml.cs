using Wpf.Ui.Controls;
using YanehCheck.EpicGamesUtils.WpfUiApp.ViewModels.Pages;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Views.Pages;

public partial class HomePage : INavigableView<HomeViewModel> {
    public HomeViewModel ViewModel { get; }

    public HomePage(HomeViewModel viewModel) {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }
}