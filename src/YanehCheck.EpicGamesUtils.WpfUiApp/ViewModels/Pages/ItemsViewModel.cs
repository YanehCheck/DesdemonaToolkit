using Wpf.Ui;
using Wpf.Ui.Controls;
using YanehCheck.EpicGamesUtils.BL.Facades.Interfaces;
using YanehCheck.EpicGamesUtils.BL.Models;
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
    private int missingItems = 0;
    private string _initializedForAccountId = "";

    [ObservableProperty]
    private IEnumerable<ItemWithImageModel> _items = [];

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
            Task.Run(InitializeViewModel)
                // Snackbar only works on UI thread
                .ContinueWith((task) => {
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
                    missingItems = 0;
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }

    public void OnNavigatedFrom() { }

    private async Task InitializeViewModel() {
        await LoadItems();
    }

    private async Task LoadItems() {
        var items = await FetchItems();
        await PresentItems(items);
    }

    private async Task PresentItems(IEnumerable<ItemWithImageModel> items) {
        await Parallel.ForEachAsync(items, async (item, _) => {
            if(item.FortniteGgId == null) {
                missingItems++;
                return;
            }

            item.BitmapFrame = await imageDownloader.GetImageAsync(item.FortniteGgId);
            Items = Items.Append(item);
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