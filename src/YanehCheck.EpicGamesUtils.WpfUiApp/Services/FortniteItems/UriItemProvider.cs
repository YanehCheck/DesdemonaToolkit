using System.Net;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using YanehCheck.EpicGamesUtils.Db.Bl.Models;
using YanehCheck.EpicGamesUtils.Utils.FortniteGgScraper.Model;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems.Mappers;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.Options;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems;

/// <inheritdoc cref="IUriItemProvider"/>
public class UriItemProvider(IRestClient restClient, IFortniteGgItemMapper mapper, IOptions<ItemFetchOptions> options) : IUriItemProvider
{
    public async Task<IEnumerable<ItemModel>?> GetItemsAsync() {
        return await GetItemsAsync(null);
    }

    public async Task<IEnumerable<ItemModel>?> GetItemsAsync(Action<double>? progressReport) {
        try {
            var request = new RestRequest(options.Value.StableSourceUri);
            var response = await restClient.GetAsync(request);
            if(!response.IsSuccessStatusCode) {
                return null;
            }

            progressReport?.Invoke(100);

            var items = JsonConvert.DeserializeObject<List<FortniteGgItem>>(response.Content!);
            return items.Select(i => {
                i.Name = WebUtility.HtmlDecode(i.Name);
                i.Description = WebUtility.HtmlDecode(i.Description);
                i.SourceDescription = WebUtility.HtmlDecode(i.SourceDescription);
                i.Set = WebUtility.HtmlDecode(i.Set);
                i.Styles = i.Styles.Select(WebUtility.HtmlDecode)!;
                return mapper.MapToModel(i);
            });
        }
        catch(Exception e) {
            return null;
        }
    }
}