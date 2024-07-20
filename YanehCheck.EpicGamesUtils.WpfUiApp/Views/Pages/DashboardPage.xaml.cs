using Wpf.Ui.Controls;
using YanehCheck.EpicGamesUtils.WpfUiApp.ViewModels.Pages;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Views.Pages {
    public partial class DashboardPage : INavigableView<DashboardViewModel> {
        public DashboardViewModel ViewModel { get; }

        public DashboardPage(DashboardViewModel viewModel) {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }
    }
}
