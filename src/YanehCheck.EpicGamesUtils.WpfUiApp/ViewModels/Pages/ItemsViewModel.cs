using Wpf.Ui;
using Wpf.Ui.Controls;
using YanehCheck.EpicGamesUtils.BL.Facades.Interfaces;
using YanehCheck.EpicGamesUtils.BL.Models;
using YanehCheck.EpicGamesUtils.Common.Enums.Items;
using YanehCheck.EpicGamesUtils.WpfUiApp.Helpers;
using YanehCheck.EpicGamesUtils.WpfUiApp.Models;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.Interfaces;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.ViewModels.Pages;

public partial class ItemsViewModel(
    IEpicGamesService epicGamesService,
    IItemFacade itemFacade,
    ISessionService sessionService,
    ISnackbarService snackbarService,
    IFortniteGgImageDownloader imageDownloader) : ObservableObject, IViewModel, INavigationAware {
    private string _initializedForAccountId = "";

    [ObservableProperty]
    private IEnumerable<ItemWithImageModel> _presentedItems = [];

    [ObservableProperty]
    private IEnumerable<ItemWithImageModel> _items = [];

    [ObservableProperty] 
    private string _search = "";
    [ObservableProperty] 
    private IEnumerable<ItemSource> _sourceFilter = [];
    [ObservableProperty]
    private IEnumerable<ItemRarity> _rarityFilter = [];
    [ObservableProperty]
    private IEnumerable<string> _seasonFilter = [];
    [ObservableProperty]
    private IEnumerable<ItemTag> _tagFilter = [];
    [ObservableProperty]
    private ItemSortFilter _sortFilter = ItemSortFilter.Newest;

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

    [RelayCommand]
    public void ToggleSourceFilterFlyout() => SourceFilterFlyoutOpen = !SourceFilterFlyoutOpen;
    [RelayCommand]
    public void ToggleRarityFilterFlyout() => RarityFilterFlyoutOpen = !RarityFilterFlyoutOpen;
    [RelayCommand]
    public void ToggleSeasonFilterFlyout() => SeasonFilterFlyoutOpen = !SeasonFilterFlyoutOpen;
    [RelayCommand]
    public void ToggleTagFilterFlyout() => TagFilterFlyoutOpen = !TagFilterFlyoutOpen;
    [RelayCommand]
    public void ToggleSortFilterFlyout() => SortFilterFlyoutOpen = !SortFilterFlyoutOpen;


    [RelayCommand]
    public void OnFilterOrSearchUpdate() {
        PresentedItems = Items.Where((i) => 
            Search == "" || 
            i.Name!.Contains(Search, StringComparison.InvariantCultureIgnoreCase) ||
            (!string.IsNullOrEmpty(i.Set) && i.Set!.Contains(Search, StringComparison.InvariantCultureIgnoreCase)));
    }

    [RelayCommand]
    public void OnSortUpdate(ItemSortFilter sort) {
        PresentedItems = sort switch {
            ItemSortFilter.AtoZ => PresentedItems.OrderBy(i => i.Name),
            ItemSortFilter.ZtoA => PresentedItems.OrderByDescending(i => i.Name),
            ItemSortFilter.Newest => PresentedItems.OrderByDescending(i => i.Release),
            ItemSortFilter.Oldest => PresentedItems.OrderBy(i => i.Release),
            ItemSortFilter.ShopMostRecent => PresentedItems.OrderByDescending(i => i.LastSeen),
            ItemSortFilter.ShopLongestWait => PresentedItems.OrderBy(i => i.LastSeen),
            ItemSortFilter.Rarity => PresentedItems.OrderByDescending(i => i.Rarity)
                    .ThenBy(i => i.Set) // This should somewhat group related items together
        };
    }

    public void OnNavigatedTo() {
        if(!sessionService.IsAuthenticated) {
            snackbarService.Show(
                "Failure",
                "No account authenticated. Please authenticate on the Home page.",
                ControlAppearance.Danger,
                null,
                TimeSpan.FromSeconds(5));
            return;
        }

        if(!sessionService.IsItemDataFetched) {
            snackbarService.Show(
                "Failure",
                "No item data found. Please fetch item data on the Home page.",
                ControlAppearance.Danger,
                null,
                TimeSpan.FromSeconds(5));
            return;
        }

        if (_initializedForAccountId != sessionService.AccountId) {
            _initializedForAccountId = sessionService.AccountId!;
            InitializeViewModel().ConfigureAwait(false);
        }
    }

    public void OnNavigatedFrom() { }

    private async Task InitializeViewModel() {
        var items = (await FetchItems()).ToList();
        var filteredItems = items.Where(i => !string.IsNullOrEmpty(i.FortniteGgId)).ToList();
        var missingItems = items.Count - filteredItems.Count;
        if(missingItems == 0) {
            snackbarService.Show(
                "Success",
                "All inventory items loaded!",
                ControlAppearance.Success,
                null,
                TimeSpan.FromSeconds(5));
        }
        else {
            snackbarService.Show(
                "Warning",
                $"{missingItems} items could not be loaded. Consider fetching item data from up-to-date source.",
                ControlAppearance.Caution,
                null,
                TimeSpan.FromSeconds(5));
        }
        Items = filteredItems;
        PresentedItems = filteredItems;


        await Task.Run(() => LoadImages(filteredItems));
    }

    private async Task LoadImages(IEnumerable<ItemWithImageModel> items) {
        await Parallel.ForEachAsync(items, async (item, _) => {
            item.BitmapFrame = await imageDownloader.GetImageAsync(item.FortniteGgId);
        });
    }

    private async Task<IEnumerable<ItemWithImageModel>> FetchItems() {
        // Handle error
        var ownedItems = await epicGamesService.GetItems(sessionService.AccountId!, sessionService.AccessToken!);
        var ownedItemModels = ownedItems.Items!.Select(i => new ItemModel() {
            FortniteId = i.FortniteId.Split(':').Last()
        });
        return (await itemFacade.GetByFortniteIdAsync(ownedItemModels)).Select(i => new ItemWithImageModel(i));
    }
}