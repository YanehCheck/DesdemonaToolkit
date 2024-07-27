using Wpf.Ui.Controls;
using YanehCheck.EpicGamesUtils.WpfUiApp.ViewModels.Pages;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Views.Pages {
    public partial class SettingsPage : INavigableView<SettingsViewModel> {
        public SettingsViewModel ViewModel { get; }

        public SettingsPage(SettingsViewModel viewModel) {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }
    }
}
