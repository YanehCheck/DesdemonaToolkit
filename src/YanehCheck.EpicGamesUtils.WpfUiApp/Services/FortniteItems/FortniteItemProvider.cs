using System.IO;
using System.Net;
using Newtonsoft.Json;
using RestSharp;
using YanehCheck.EpicGamesUtils.Db.Bl.Models;
using YanehCheck.EpicGamesUtils.Utils.FortniteGgScraper;
using YanehCheck.EpicGamesUtils.Utils.FortniteGgScraper.Model;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems.Mappers;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.Options;
using YanehCheck.EpicGamesUtils.WpfUiApp.Utilities.Options.Interfaces;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems;

public class FortniteItemProvider(IFortniteGgScrapper fortniteGgScrapper, IRestClient restClient, IFortniteGgItemMapper mapper, IWritableOptions<ItemFetchOptions> options) : IFortniteItemProvider {
    public async Task<IEnumerable<ItemModel>?> GetItemsJsonFileAsync(string path, Action<double>? progressReport) {
        try {
            var json = await File.ReadAllTextAsync(path);

            var items = JsonConvert.DeserializeObject<List<FortniteGgItem>>(json);
            return items!.Select(DecodeAndMap);
        }
        catch(Exception e) {
            return null;
        }
    }

    public async Task<IEnumerable<ItemModel>?> GetItemsStableUriAsync(Action<double>? progressReport) {
        try {
            var request = new RestRequest(options.Value.StableSourceUri);
            var response = await restClient.GetAsync(request);
            if(!response.IsSuccessStatusCode) {
                return null;
            }

            progressReport?.Invoke(100);

            var items = JsonConvert.DeserializeObject<List<FortniteGgItem>>(response.Content!);
            return items!.Select(DecodeAndMap);
        }
        catch(Exception e) {
            return null;
        }
    }

    public async Task<IEnumerable<ItemModel>?> GetItemsFortniteGgAsync(Action<double>? progressReport) {
        var items = await fortniteGgScrapper.ScrapIdRangeParallelAsync(0, options.Value.FortniteGgIdRange, progressReport);
        return items is null or { IsEmpty: true } ?
            null :
            items.Select(DecodeAndMap);
    }

    private ItemModel DecodeAndMap(FortniteGgItem item) {
        item.Name = WebUtility.HtmlDecode(item.Name);
        item.Description = WebUtility.HtmlDecode(item.Description);
        item.SourceDescription = WebUtility.HtmlDecode(item.SourceDescription);
        item.Set = WebUtility.HtmlDecode(item.Set);
        item.Styles = (item.Styles?.Select(WebUtility.HtmlDecode) ?? [])!;
        return mapper.MapToModel(item);
    }
}