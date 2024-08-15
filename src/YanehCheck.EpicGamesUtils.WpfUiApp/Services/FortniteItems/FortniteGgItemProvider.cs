using System.Net;
using Microsoft.Extensions.Options;
using YanehCheck.EpicGamesUtils.Db.Bl.Models;
using YanehCheck.EpicGamesUtils.Utils.FortniteGgScraper;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems.Mappers;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.Options;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems;

public class FortniteGgItemProvider(IFortniteGgScrapper fortniteGgScrapper, IFortniteGgItemMapper mapper, IOptions<ItemFetchOptions> itemOptions) : IFortniteGgItemProvider
{
    public async Task<IEnumerable<ItemModel>?> GetItemsAsync() {
        return await GetItemsAsync(null);
    }

    public async Task<IEnumerable<ItemModel>?> GetItemsAsync(Action<double>? progressReport) {
        var items = await fortniteGgScrapper.ScrapIdRangeParallelAsync(0, itemOptions.Value.FortniteGgIdRange, progressReport);
        return items is null or { IsEmpty: true } ?
            null :
            items.Select(i => {
            i.Name = WebUtility.HtmlDecode(i.Name);
            i.Description = WebUtility.HtmlDecode(i.Description);
            i.SourceDescription = WebUtility.HtmlDecode(i.SourceDescription);
            i.Set = WebUtility.HtmlDecode(i.Set);
            i.Styles = (i.Styles?.Select(WebUtility.HtmlDecode) ?? [])!;
            return mapper.MapToModel(i);
        });
    }
}