
using Wpf.Ui.Controls;
using YanehCheck.EpicGamesUtils.WpfUiApp.ViewModels.Pages;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Views.Pages {
    public partial class AccountUtilsPage : INavigableView<AccountUtilsViewModel> {
        public AccountUtilsViewModel ViewModel { get; }

        public AccountUtilsPage(AccountUtilsViewModel viewModel) {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }
    }
}
