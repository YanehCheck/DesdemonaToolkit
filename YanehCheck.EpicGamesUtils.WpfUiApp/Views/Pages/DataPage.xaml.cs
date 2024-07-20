using Wpf.Ui.Controls;
using YanehCheck.EpicGamesUtils.WpfUiApp.ViewModels.Pages;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Views.Pages {
    public partial class DataPage : INavigableView<DataViewModel> {
        public DataViewModel ViewModel { get; }

        public DataPage(DataViewModel viewModel) {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }
    }
}
