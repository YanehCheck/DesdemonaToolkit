using YanehCheck.EpicGamesUtils.Db.Bl.Models;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems.Interfaces;

public interface IFortniteItemProvider
{
    Task<IEnumerable<ItemModel>?> GetItemsAsync();
    Task<IEnumerable<ItemModel>?> GetItemsAsync(Action<double>? progressReport);
}