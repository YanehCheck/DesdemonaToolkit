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

    #region FilteringSortingSearchingMethods

    [RelayCommand]
    public void ToggleSourceFilterFlyout()  {
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

    [RelayCommand]
    public void OnSearch() => FilterAndSearchUpdate();

    [RelayCommand]
    public void OnSort(ItemSortFilter sort) => SortUpdate(sort);

    [RelayCommand]
    public void OnSource(ItemSource source) {
        SourceFilter = SourceFilter.Contains(source)
            ? SourceFilter.Where(s => s != source)
            : SourceFilter.Append(source);
        FilterAndSearchUpdate();
    }

    [RelayCommand]
    public void OnRarity(ItemRarity rarity) {
        RarityFilter = RarityFilter.Contains(rarity)
            ? RarityFilter.Where(s => s != rarity)
            : RarityFilter.Append(rarity);
        FilterAndSearchUpdate();
    }

    [RelayCommand]
    public void OnTag(ItemTag tag) {
        TagFilter = TagFilter.Contains(tag)
            ? TagFilter.Where(s => s != tag)
            : TagFilter.Append(tag);
        FilterAndSearchUpdate();
    }

    [RelayCommand]
    public void OnSeason(string season) {
        SeasonFilter = SeasonFilter.Contains(season)
            ? SeasonFilter.Where(s => s != season)
            : SeasonFilter.Append(season);
        FilterAndSearchUpdate();
    }

    private void SortUpdate(ItemSortFilter sort) {
        // We want to sort the original collection here
        // Otherwise we would have to resort everytime user applies filter/types character
        SortFilter = sort;
        Items = sort switch {
            ItemSortFilter.AtoZ => Items.OrderBy(i => i.Name),
            ItemSortFilter.ZtoA => Items.OrderByDescending(i => i.Name),
            ItemSortFilter.Newest => Items.OrderByDescending(i => i.Release),
            ItemSortFilter.Oldest => Items.OrderBy(i => i.Release),
            ItemSortFilter.ShopMostRecent => Items.OrderByDescending(i => i.LastSeen),
            ItemSortFilter.ShopLongestWait => Items.OrderBy(i => i.LastSeen),
            ItemSortFilter.Rarity => Items.OrderByDescending(i => i.Rarity)
                .ThenBy(i => i.Set) // This should somewhat group related items together
        };
        FilterAndSearchUpdate();
    }

    private void FilterAndSearchUpdate() {
        bool SearchCond(ItemWithImageModel i) => 
             Search == "" ||
             i.Name!.Contains(Search, StringComparison.InvariantCultureIgnoreCase) ||
             (!string.IsNullOrEmpty(i.Set) && i.Set!.Contains(Search, StringComparison.InvariantCultureIgnoreCase));

        bool FiltersCond(ItemWithImageModel i) => 
            (!SourceFilter.Any() || SourceFilter.Contains(i.Source)) &&
            (!RarityFilter.Any() || RarityFilter.Contains(i.Rarity)) &&
            (!SeasonFilter.Any() || SeasonFilter.Contains(i.Season)) &&
            (!TagFilter.Any()    || TagFilter.Intersect(i.Tags).Any());

        PresentedItems = Items.Where((i) => SearchCond(i) && FiltersCond(i));
    }

    #endregion

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
        SortUpdate(SortFilter);


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