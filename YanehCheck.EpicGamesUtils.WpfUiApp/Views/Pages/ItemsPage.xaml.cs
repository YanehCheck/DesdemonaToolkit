using Wpf.Ui.Controls;
using YanehCheck.EpicGamesUtils.WpfUiApp.ViewModels.Pages;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Views.Pages;

public partial class ItemsPage : INavigableView<ItemsViewModel> {
    public ItemsViewModel ViewModel { get; }

    public ItemsPage(ItemsViewModel viewModel) {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }
}