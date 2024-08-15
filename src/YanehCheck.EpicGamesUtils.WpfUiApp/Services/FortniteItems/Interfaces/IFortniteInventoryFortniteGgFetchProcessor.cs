using YanehCheck.EpicGamesUtils.Db.Bl.Models;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems.Interfaces;

public interface IFortniteInventoryFortniteGgFetchProcessor
{
    string Create(IEnumerable<ItemModel> items);
}