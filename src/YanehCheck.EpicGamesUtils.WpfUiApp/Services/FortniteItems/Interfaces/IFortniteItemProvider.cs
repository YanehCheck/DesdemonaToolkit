using YanehCheck.EpicGamesUtils.Db.Bl.Models;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems.Interfaces;

public interface IFortniteItemProvider
{
    Task<IList<ItemModel>?> GetItemsJsonFileAsync(string path, Action<double>? progressReport);
    Task<IList<ItemModel>?> GetItemsStableUriAsync(Action<double>? progressReport);
    Task<IList<ItemModel>?> GetItemsFortniteGgAsync(Action<double>? progressReport);
}