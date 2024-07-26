using YanehCheck.EpicGamesUtils.BL.Models;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems.Interfaces;

public interface IFortniteItemProvider
{
    public Task<IEnumerable<ItemFullModel>?> GetItemsAsync();
}