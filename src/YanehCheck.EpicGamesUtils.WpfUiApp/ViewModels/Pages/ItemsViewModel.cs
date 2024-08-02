using System.Collections.ObjectModel;
using Wpf.Ui;
using Wpf.Ui.Controls;
using YanehCheck.EpicGamesUtils.BL.Facades.Interfaces;
using YanehCheck.EpicGamesUtils.BL.Models;
using YanehCheck.EpicGamesUtils.Common.Enums.Items;
using YanehCheck.EpicGamesUtils.WpfUiApp.Helpers.Enums;
using YanehCheck.EpicGamesUtils.WpfUiApp.Models;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.Interfaces;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.ViewModels.Pages;

public partial class ItemsViewModel : ObservableObject, IViewModel, INavigationAware {
    private readonly IEpicGamesService epicGamesService;
    private readonly IItemFacade itemFacade;
    private readonly ISessionService sessionService;
    private readonly ISnackbarService snackbarService;
    private readonly IFortniteInventoryImageProcessor inventoryProcessor;
    private readonly IFortniteGgImageDownloader imageDownloader;
    private readonly IFileSaveDialogService fileSaveService;

    private string _initializedForAccountId = "";

    [ObservableProperty]
    private IEnumerable<ItemPresentationModel> _presentedItems = [];

    private IEnumerable<ItemPresentationModel> sortedItems = [];
    private IEnumerable<ItemPresentationModel> items = [];

    public bool AnyFilterApplied => SourceFilter.Any() || RarityFilter.Any() || SeasonFilter.Any() || TagFilter.Any();

    [ObservableProperty] 
    private string _search = "";

    [ObservableProperty] 
    private ItemTypeFilter _typeFilter = ItemTypeFilter.All;
    [ObservableProperty] 
    private ObservableCollection<ItemSource> _sourceFilter = [];
    [ObservableProperty]
    private ObservableCollection<ItemRarity> _rarityFilter = [];
    [ObservableProperty]
    private ObservableCollection<string> _seasonFilter = [];
    [ObservableProperty]
    private ObservableCollection<ItemTag> _tagFilter = [];
    [ObservableProperty]
    private ItemSortFilter _sortFilter = ItemSortFilter.Newest;


    /// <inheritdoc/>
    public ItemsViewModel(IEpicGamesService epicGamesService,
        IItemFacade itemFacade,
        ISessionService sessionService,
        ISnackbarService snackbarService,
        IFortniteGgImageDownloader imageDownloader,
        IFortniteInventoryImageProcessor inventoryProcessor,
        IFileSaveDialogService fileSaveService) {
        this.epicGamesService = epicGamesService;
        this.itemFacade = itemFacade;
        this.sessionService = sessionService;
        this.snackbarService = snackbarService;
        this.imageDownloader = imageDownloader;
        this.inventoryProcessor = inventoryProcessor;
        this.fileSaveService = fileSaveService;
    }

    [RelayCommand]
    public async Task ExportInventory(InventoryExport to) {
        var fileName = $"{sessionService.DisplayName}-inventory-{DateTime.Now:yyyy-M-d}";

        if (to == InventoryExport.Text) {
            var names = PresentedItems.Select(i => i.Name);
            var content = string.Join('\n', names);
            await fileSaveService.SaveTextFile(content, fileName);
        }
        else if (to == InventoryExport.Image) {
            using var image = inventoryProcessor.Create(PresentedItems.ToList());
            await fileSaveService.SaveImageFile(image, fileName);
        }
    }

    #region FilteringSortingSearchingMethods

    [RelayCommand]
    public void ClearFilters() {
        SourceFilter = [];
        RarityFilter = [];
        SeasonFilter = [];
        TagFilter = [];
        OnPropertyChanged(nameof(AnyFilterApplied));
        FilterAndSearchUpdate();
    }

    [RelayCommand]
    public void OnSearch() => FilterAndSearchUpdate();

    [RelayCommand]
    public void OnSort(ItemSortFilter sort) => SortUpdate(sort);

    [RelayCommand]
    public void OnSource(ItemSource source) {
        SourceFilter = new ObservableCollection<ItemSource>(
            SourceFilter.Contains(source)
            ? SourceFilter.Where(s => s != source)
            : SourceFilter.Append(source));
        OnPropertyChanged(nameof(AnyFilterApplied));
        FilterAndSearchUpdate();
    }

    [RelayCommand]
    public void OnRarity(ItemRarity rarity) {
        RarityFilter = new ObservableCollection<ItemRarity>(
            RarityFilter.Contains(rarity)
            ? RarityFilter.Where(s => s != rarity)
            : RarityFilter.Append(rarity));
        OnPropertyChanged(nameof(AnyFilterApplied));
        FilterAndSearchUpdate();
    }

    [RelayCommand]
    public void OnTag(ItemTag tag) {
        TagFilter = new ObservableCollection<ItemTag>(
            TagFilter.Contains(tag)
            ? TagFilter.Where(s => s != tag)
            : TagFilter.Append(tag));
        OnPropertyChanged(nameof(AnyFilterApplied));
        FilterAndSearchUpdate();
    }

    [RelayCommand]
    public void OnSeason(string season) {
        SeasonFilter = new ObservableCollection<string>(
            SeasonFilter.Contains(season)
            ? SeasonFilter.Where(s => s != season)
            : SeasonFilter.Append(season));
        OnPropertyChanged(nameof(AnyFilterApplied));
        FilterAndSearchUpdate();
    }

    [RelayCommand]
    public void OnType(ItemTypeFilter type) {
        TypeFilter = type;
        FilterAndSearchUpdate();
    }

    private void SortUpdate(ItemSortFilter sort) {
        // We want to sort the original collection here
        // Otherwise we would have to resort everytime user applies filter/types character
        SortFilter = sort;
        sortedItems = sort switch {
            ItemSortFilter.AtoZ => items.OrderBy(i => i.Name),
            ItemSortFilter.ZtoA => items.OrderByDescending(i => i.Name),
            ItemSortFilter.Newest => items.OrderByDescending(i => i.Release ?? DateTime.MaxValue)
                .ThenBy(i => i.Set)
                .ThenBy(i => i.Type),
            ItemSortFilter.Oldest => items.OrderBy(i => i.Release ?? DateTime.MaxValue)
                .ThenBy(i => i.Set)
                .ThenBy(i => i.Type),
            ItemSortFilter.ShopMostRecent => items.OrderByDescending(i => i.LastSeen ?? DateTime.MinValue),
            ItemSortFilter.ShopLongestWait => items.OrderBy(i => i.LastSeen ?? DateTime.MaxValue),
            ItemSortFilter.Rarity => items.OrderByDescending(i => i.Rarity)
                .ThenBy(i => i.Set) // This should somewhat group related items together
                .ThenBy(i => i.Type), 
            ItemSortFilter.Type => items.OrderBy(i => i.Type)
                .ThenByDescending(i => i.Rarity)
        };
        FilterAndSearchUpdate();
    }

    private void FilterAndSearchUpdate() {
        bool SearchCond(ItemPresentationModel i) => 
             Search == "" ||
             i.Name!.Contains(Search, StringComparison.InvariantCultureIgnoreCase) ||
             (!string.IsNullOrEmpty(i.Set) && i.Set!.Contains(Search, StringComparison.InvariantCultureIgnoreCase));

        bool FiltersCond(ItemPresentationModel i) =>
            (!SourceFilter.Any() || SourceFilter.Contains(i.Source)) &&
            (!RarityFilter.Any() || RarityFilter.Contains(i.Rarity)) &&
            (!SeasonFilter.Any() || SeasonFilter.Contains(i.Season)) &&
            (!TagFilter.Any() || TagFilter.Intersect(i.Tags).Any()) &&
            TypeFilter.Satisfied(i.Type);

        PresentedItems = sortedItems.Where((i) => SearchCond(i) && FiltersCond(i));
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
        var fetchedItems = (await FetchItems())?.ToList();
        if(fetchedItems == null) {
            return;
        }
        var filteredItems = fetchedItems.Where(i => !string.IsNullOrEmpty(i.FortniteGgId)).ToList();
        var missingItems = fetchedItems.Count - filteredItems.Count;
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
        items = filteredItems;
        SortUpdate(SortFilter);


        await Task.Run(() => LoadImages(filteredItems));
    }

    private async Task LoadImages(IEnumerable<ItemPresentationModel> items) {
        await Parallel.ForEachAsync(items, async (item, _) => {
            item.BitmapFrame = await imageDownloader.GetImageAsync(item.FortniteGgId);
        });
    }

    private async Task<IEnumerable<ItemPresentationModel>?> FetchItems() {
        var ownedItemsResult = await epicGamesService.GetItems(sessionService.AccountId!, sessionService.AccessToken!);
        if (!ownedItemsResult.Success) {
            snackbarService.Show("Failure", ownedItemsResult.ErrorMessage!, ControlAppearance.Danger, null, TimeSpan.FromSeconds(5));
            _initializedForAccountId = "";
            return null;
        }

        var ownedItemModels = ownedItemsResult.Items!.Select(i => new ItemModel() {
            FortniteId = i.FortniteId.Split(':').Last()
        });
        return (await itemFacade.GetByFortniteIdAsync(ownedItemModels)).Select(i => new ItemPresentationModel(i));
    }
}