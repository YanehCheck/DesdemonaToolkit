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
    private string _initializedForAccountId = "";

    [ObservableProperty]
    private IEnumerable<ItemWithImageModel> _items = [];

    public void OnNavigatedTo() {
        if(!sessionService.IsAuthenticated) {
            // HANDLE
            //return;
        }

        if(!sessionService.IsItemDataFetched) {
            // HANDLE
            //return;
        }

        if(_initializedForAccountId != sessionService.AccountId) {
            InitializeViewModel().ConfigureAwait(false); // Await or run task
        }
    }

    public void OnNavigatedFrom() { }

    private async Task InitializeViewModel() {
        await LoadItems();
        _initializedForAccountId = sessionService.AccountId!;
    }

    private async Task LoadItems() {
        var items = await FetchItems();
        await Parallel.ForEachAsync(items, async (item, _) => {
            if(item.FortniteGgId == null) {
                // 
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