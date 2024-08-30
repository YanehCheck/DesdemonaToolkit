using YanehCheck.EpicGamesUtils.Db.Bl.Models;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems.Interfaces;

public interface IFortniteItemProvider
{
    Task<IEnumerable<ItemModel>?> GetItemsJsonFileAsync(string path, Action<double>? progressReport);
    Task<IEnumerable<ItemModel>?> GetItemsStableUriAsync(Action<double>? progressReport);
    Task<IEnumerable<ItemModel>?> GetItemsFortniteGgAsync(Action<double>? progressReport);
}