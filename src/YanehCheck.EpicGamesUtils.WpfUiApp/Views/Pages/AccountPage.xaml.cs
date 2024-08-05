using Wpf.Ui.Controls;
using YanehCheck.EpicGamesUtils.WpfUiApp.ViewModels.Pages;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Views.Pages;

public partial class AccountPage : INavigableView<AccountViewModel> {
    public AccountViewModel ViewModel { get; }

    public AccountPage(AccountViewModel viewModel) {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }
}