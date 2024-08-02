using Wpf.Ui.Controls;
using YanehCheck.EpicGamesUtils.WpfUiApp.ViewModels.Pages;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Views.Pages;

[INotifyPropertyChanged]
public partial class ItemsPage : INavigableView<ItemsViewModel> {

    [ObservableProperty]
    private bool _exportFlyoutOpen = false;
    [ObservableProperty]
    private bool _sourceFilterFlyoutOpen = false;
    [ObservableProperty]
    private bool _rarityFilterFlyoutOpen = false;
    [ObservableProperty]
    private bool _seasonFilterFlyoutOpen = false;
    [ObservableProperty]
    private bool _tagFilterFlyoutOpen = false;
    [ObservableProperty]
    private bool _sortFilterFlyoutOpen = false;

    public ItemsViewModel ViewModel { get; }

    public ItemsPage(ItemsViewModel viewModel) {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }

    [RelayCommand]
    public void ToggleExportFlyout() {
        ExportFlyoutOpen = false;
        ExportFlyoutOpen = true;
    }

    [RelayCommand]
    public void ToggleSourceFilterFlyout() {
        SourceFilterFlyoutOpen = false; // Maybe just keep it true and trigger notify?
        SourceFilterFlyoutOpen = true;
    }
    [RelayCommand]
    public void ToggleRarityFilterFlyout() {
        RarityFilterFlyoutOpen = false;
        RarityFilterFlyoutOpen = true;
    }
    [RelayCommand]
    public void ToggleSeasonFilterFlyout() {
        SeasonFilterFlyoutOpen = false;
        SeasonFilterFlyoutOpen = true;
    }
    [RelayCommand]
    public void ToggleTagFilterFlyout() {
        TagFilterFlyoutOpen = false;
        TagFilterFlyoutOpen = true;
    }
    [RelayCommand]
    public void ToggleSortFilterFlyout() {
        SortFilterFlyoutOpen = false;
        SortFilterFlyoutOpen = true;
    }
}