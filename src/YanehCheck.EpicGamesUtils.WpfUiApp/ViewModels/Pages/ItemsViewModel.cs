using System.Collections.ObjectModel;
using Wpf.Ui;
using Wpf.Ui.Controls;
using YanehCheck.EpicGamesUtils.Common.Enums.Items;
using YanehCheck.EpicGamesUtils.Db.Bl.Facades.Interfaces;
using YanehCheck.EpicGamesUtils.Db.Bl.Models;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Exceptions;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.CustomFilters.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.Persistence.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.UI.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Types.Enums;
using ItemPresentationModel = YanehCheck.EpicGamesUtils.WpfUiApp.Types.Classes.Presentation.ItemPresentationModel;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.ViewModels.Pages;

public partial class ItemsViewModel : ObservableObject, IViewModel, INavigationAware {
    private readonly ICachedEpicGamesService epicGamesService;
    private readonly IItemFacade itemFacade;
    private readonly ISessionService sessionService;
    private readonly ISnackbarService snackbarService;
    private readonly IFortniteInventoryImageProcessor imageInventoryProcessor;
    private readonly IFortniteInventoryFortniteGgFetchProcessor fetchInventoryProcessor;
    private readonly IFortniteImageProvider imageProvider;
    private readonly IDialogService service;
    private readonly ICustomFilterProvider filterProvider;
    private readonly IItemStyleFacade itemStyleFacade;

    private string _initializedForAccountId = "";

    [ObservableProperty]
    private bool _itemsLoaded;

    // Having custom filter and the search bar responsive to typing
    // can be expensive. Multiple "backing" fields provide performance
    // and even stability.

    [ObservableProperty]
    private IEnumerable<ItemPresentationModel> _presentedItems = [];
    private IEnumerable<ItemPresentationModel> filteredItems = [];
    private IEnumerable<ItemPresentationModel> sortedItems = [];
    private IEnumerable<ItemPresentationModel> customFilteredItems = [];
    private IEnumerable<ItemPresentationModel> items = [];

    [ObservableProperty] 
    private string _search = "";

    [ObservableProperty] 
    private ObservableCollection<IFilter> _customFilters = [];

    [ObservableProperty] 
    private IFilter? _customFilter;

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
    public ItemsViewModel(ICachedEpicGamesService epicGamesService,
        IItemFacade itemFacade,
        ISessionService sessionService,
        ISnackbarService snackbarService,
        IFortniteImageProvider imageProvider,
        IFortniteInventoryImageProcessor imageInventoryProcessor,
        IFortniteInventoryFortniteGgFetchProcessor fetchInventoryProcessor,
        IDialogService service,
        ICustomFilterProvider filterProvider,
        IItemStyleFacade itemStyleFacade) {
        this.epicGamesService = epicGamesService;
        this.itemFacade = itemFacade;
        this.sessionService = sessionService;
        this.snackbarService = snackbarService;
        this.imageProvider = imageProvider;
        this.imageInventoryProcessor = imageInventoryProcessor;
        this.service = service;
        this.filterProvider = filterProvider;
        this.fetchInventoryProcessor = fetchInventoryProcessor;
        this.itemStyleFacade = itemStyleFacade;
    }

    public bool AnyFilterApplied => SourceFilter.Any() || RarityFilter.Any() || SeasonFilter.Any() || TagFilter.Any();

    [RelayCommand]
    public async Task ExportInventory(InventoryExport to) {
        var fileName = $"{sessionService.DisplayName}-inventory-{DateTime.Now:yyyy-M-d}";

        if (to == InventoryExport.Text) {
            var names = PresentedItems.Select(i => i.Name);
            var content = string.Join('\n', names);
            await service.SaveTextFile(content, fileName);
        }
        else if (to == InventoryExport.Image) {
            try {
                using var image = imageInventoryProcessor.Create(PresentedItems.ToList(), sessionService.DisplayName!);
                await service.SaveImageFile(image, fileName);
            }
            catch (ArgumentException e) {
                snackbarService.Show(
                    "Failure",
                    $"An error occured while exporting the image. Please check your connection and if all the images load properly.\nError: {e.Message}",
                    ControlAppearance.Danger,
                    null,
                    TimeSpan.FromSeconds(5));
            }
        }
        else if (to == InventoryExport.FortniteGgScript) {
            var fetchString = fetchInventoryProcessor.Create(PresentedItems.ToList());
            Clipboard.SetText(fetchString);
            snackbarService.Show(
                "Success",
                $"The script was copied to clipboard. Please paste it into your browser's console where you are logged into Fortnite.gg",
                ControlAppearance.Success,
                null,
                TimeSpan.FromSeconds(10));
        }

    }

    [RelayCommand]
    public void ReloadItems() {
        ItemsLoaded = false;
        items = [];
        CustomFilterUpdate();
        epicGamesService.Invalidate(nameof(ICachedEpicGamesService.GetFortniteProfile));
        InitializeViewModelForUserAndShowResult(() => {
            ItemsLoaded = true;
        });
    }

    #region FilteringSortingSearchingMethods

    [RelayCommand]
    public void ClearFilters() {
        SourceFilter = [];
        RarityFilter = [];
        SeasonFilter = [];
        TagFilter = [];
        OnPropertyChanged(nameof(AnyFilterApplied));
        FilterUpdate();
    }

    [RelayCommand]
    public void OnCustomFilter(IFilter filter) {
        CustomFilter = CustomFilter == filter ? null : filter;
        CustomFilterUpdate();
    }

    [RelayCommand]
    public void OnSearch() => FilterUpdate();

    [RelayCommand]
    public void OnSort(ItemSortFilter sort) {
        SortFilter = sort;
        SortUpdate();
    }

    [RelayCommand]
    public void OnSource(ItemSource source) {
        SourceFilter = new ObservableCollection<ItemSource>(
            SourceFilter.Contains(source)
            ? SourceFilter.Where(s => s != source)
            : SourceFilter.Append(source));
        OnPropertyChanged(nameof(AnyFilterApplied));
        FilterUpdate();
    }

    [RelayCommand]
    public void OnRarity(ItemRarity rarity) {
        RarityFilter = new ObservableCollection<ItemRarity>(
            RarityFilter.Contains(rarity)
            ? RarityFilter.Where(s => s != rarity)
            : RarityFilter.Append(rarity));
        OnPropertyChanged(nameof(AnyFilterApplied));
        FilterUpdate();
    }

    [RelayCommand]
    public void OnTag(ItemTag tag) {
        TagFilter = new ObservableCollection<ItemTag>(
            TagFilter.Contains(tag)
            ? TagFilter.Where(s => s != tag)
            : TagFilter.Append(tag));
        OnPropertyChanged(nameof(AnyFilterApplied));
        FilterUpdate();
    }

    [RelayCommand]
    public void OnSeason(string season) {
        SeasonFilter = new ObservableCollection<string>(
            SeasonFilter.Contains(season)
            ? SeasonFilter.Where(s => s != season)
            : SeasonFilter.Append(season));
        OnPropertyChanged(nameof(AnyFilterApplied));
        FilterUpdate();
    }

    [RelayCommand]
    public void OnType(ItemTypeFilter type) {
        TypeFilter = type;
        FilterUpdate();
    }

    private void CustomFilterUpdate() {
        if (CustomFilter == null) {
            customFilteredItems = items;
        }
        else {
            try {
                customFilteredItems = CustomFilter.Apply(items).OfType<ItemPresentationModel>();
            }
            catch (FilterException e) {
                snackbarService.Show(
                    "Failure",
                    $"An unexpected error occured while applying filter \"{CustomFilter.Name}\" with message:\n{e.Message}.",
                    ControlAppearance.Danger,
                    null,
                    TimeSpan.FromSeconds(5));
            }
        }

        SortUpdate();
    }

    private void SortUpdate() {
        sortedItems = SortFilter switch {
            ItemSortFilter.AtoZ => customFilteredItems.OrderBy(i => i.Name),
            ItemSortFilter.ZtoA => customFilteredItems.OrderByDescending(i => i.Name),
            ItemSortFilter.Newest => customFilteredItems.OrderByDescending(i => i.Release ?? DateTime.MaxValue)
                .ThenBy(i => i.Set)
                .ThenBy(i => i.Type),
            ItemSortFilter.Oldest => customFilteredItems.OrderBy(i => i.Release ?? DateTime.MaxValue)
                .ThenBy(i => i.Set)
                .ThenBy(i => i.Type),
            ItemSortFilter.ShopMostRecent => customFilteredItems.OrderByDescending(i => i.LastSeen ?? DateTime.MinValue),
            ItemSortFilter.ShopLongestWait => customFilteredItems.OrderBy(i => i.LastSeen ?? DateTime.MaxValue),
            ItemSortFilter.Rarity => customFilteredItems.OrderByDescending(i => i.Rarity)
                .ThenBy(i => i.Set) // This should somewhat group related items together
                .ThenBy(i => i.Type), 
            ItemSortFilter.Type => customFilteredItems.OrderBy(i => i.Type)
                .ThenByDescending(i => i.Rarity)
        };
        FilterUpdate();
    }

    private void FilterUpdate() {
        bool FiltersCond(ItemPresentationModel i) =>
            (!SourceFilter.Any() || SourceFilter.Contains(i.Source)) &&
            (!RarityFilter.Any() || RarityFilter.Contains(i.Rarity)) &&
            (!SeasonFilter.Any() || SeasonFilter.Contains(i.Season)) &&
            (!TagFilter.Any() || TagFilter.Intersect(i.Tags).Any()) &&
            TypeFilter.Satisfied(i.Type);

        filteredItems = sortedItems.Where(FiltersCond);
        SearchUpdate();
    }

    private void SearchUpdate() {
        bool SearchCond(ItemPresentationModel i) =>
            Search == "" ||
            i.Name!.Contains(Search, StringComparison.InvariantCultureIgnoreCase) ||
            (!string.IsNullOrEmpty(i.Set) && i.Set!.Contains(Search, StringComparison.InvariantCultureIgnoreCase));

        PresentedItems = filteredItems.Where(SearchCond);
    }

    #endregion

    public void OnNavigatedTo() {
        InitializeViewModel().ConfigureAwait(false);
        if(!sessionService.IsAuthenticated) {
            ItemsLoaded = true;
            snackbarService.Show(
                "Failure",
                "No account authenticated. Please authenticate on the Home page.",
                ControlAppearance.Danger,
                null,
                TimeSpan.FromSeconds(5));
            return;
        }

        if(!sessionService.IsItemDataFetched) {
            ItemsLoaded = true;
            snackbarService.Show(
                "Failure",
                "No item data found. Please fetch item data on the Home page.",
                ControlAppearance.Danger,
                null,
                TimeSpan.FromSeconds(5));
            return;
        }

        if (_initializedForAccountId != sessionService.AccountId) {
            ItemsLoaded = false;
            _initializedForAccountId = sessionService.AccountId!;
            InitializeViewModelForUserAndShowResult();
        }
    }
    public void OnNavigatedFrom() { }

    private void InitializeViewModelForUserAndShowResult(Action? actionOnComplete = null) {
        Task.Run(InitializeViewModelForUser).ContinueWith(task => {
            if (task.Result.ErrorMessage != null) {
                snackbarService.Show(
                    "Failure",
                    task.Result.ErrorMessage,
                    ControlAppearance.Danger,
                    null,
                    TimeSpan.FromSeconds(5));
            }
            else if(task.Result.MissingItems == 0) {
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
                    $"{task.Result.MissingItems} item{(task.Result.MissingItems == 1 ? string.Empty : "s")} could not be loaded. Consider fetching item data from up-to-date source.",
                    ControlAppearance.Caution,
                    null,
                    TimeSpan.FromSeconds(5));
            }
            actionOnComplete?.Invoke();
        }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    private async Task InitializeViewModel() {
        CustomFilters = new ObservableCollection<IFilter>(
            await filterProvider.GetAllAsync((f, e) => {
            snackbarService.Show(
                "Failure",
                $"An error occured while parsing filter \"{f}\":\n{e.Message}",
                ControlAppearance.Danger,
                null,
                TimeSpan.FromSeconds(5));
        }));
    }

    private async Task<(int MissingItems, string? ErrorMessage)> InitializeViewModelForUser() {
        var fetchResult = await FetchItems();
        if(fetchResult.ErrorMessage != null) {
            return (0, fetchResult.ErrorMessage);
        }
        var fetchedItems = fetchResult.Items!.ToList();
        items = fetchedItems.Where(i => !string.IsNullOrEmpty(i.FortniteGgId)).ToList();
        ItemsLoaded = true;
        var missingItems = fetchedItems.Count - items.Count();
        CustomFilterUpdate();
        await Task.Run(() => LoadImages(filteredItems));
        return (missingItems, null);
    }

    private async Task LoadImages(IEnumerable<ItemPresentationModel> items) {
        var test = items.ToList();
        await Parallel.ForEachAsync(test, async (item, _) => {
            item.BitmapFrame = await imageProvider.GetImageAsync(item.FortniteGgId);
        });
    }

    private async Task<(IEnumerable<ItemPresentationModel>? Items, string? ErrorMessage)> FetchItems() {
        // Get owned item IDs and styles
        var profileItemsResult = await epicGamesService.GetFortniteProfile(sessionService.AccountId!, sessionService.AccessToken!);
        if (!profileItemsResult.Success) {
            _initializedForAccountId = "";
            return ( null, $"An error occured while fetching your inventory.\nError: {profileItemsResult.ErrorMessage!}");
        }
        var profileItems = profileItemsResult.Items!.ToList();

        // Create models with set ID
        var emptyProfileItems = profileItemsResult.Items!.Select(i => new ItemModel {
            FortniteId = i.FortniteId.Split(':').Last()
        });
        try {
            // Pull item data from DB
            var items = await itemFacade.GetByFortniteIdAsync(emptyProfileItems);
            // Add owned raw styles and map to ItemPresentationModel
            var itemsWithStyles = items.Select((item, index) => new ItemPresentationModel(item) {
                OwnedStylesRaw = profileItems.ElementAt(index).OwnedStylesRaw
            }).ToList();
            // Pull required style information from DB for UNLOCKABLE styles
            foreach (var item in itemsWithStyles.Where(e => e.OwnedStylesRaw.Any())) {
                var validProperties = item.OwnedStylesRaw.SelectMany(i => i.Property);
                item.OwnedStyles = await itemStyleFacade.GetByFortniteItemIdAsync(item.FortniteId, validProperties);
            }
            return (itemsWithStyles,null);
        }
        catch (Exception ex) {
            return (null, $"An error occured while fetching item data from database.\nError: {ex.Message}");
        }
    }
}