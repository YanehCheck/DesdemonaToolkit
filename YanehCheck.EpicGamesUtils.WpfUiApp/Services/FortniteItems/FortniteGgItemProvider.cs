using YanehCheck.EpicGamesUtils.BL.Models;
using YanehCheck.EpicGamesUtils.FortniteGGScraper;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems.Mappers;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems;

public class FortniteGgItemProvider(IFortniteGgScrapper fortniteGgScrapper, IFortniteGgItemMapper mapper) : IFortniteGgItemProvider
{
    public async Task<IEnumerable<ItemFullModel>?> GetItemsAsync()
    {
        var items = await fortniteGgScrapper.ScrapIdRangeParallelAsync(0, 14000);
        return items is null or { IsEmpty: true } ?
            null :
            items.Select(mapper.MapToModel);
    }
}