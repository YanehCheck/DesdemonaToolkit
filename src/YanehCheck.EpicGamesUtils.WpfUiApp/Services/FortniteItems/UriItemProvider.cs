using System.Web;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using YanehCheck.EpicGamesUtils.BL.Models;
using YanehCheck.EpicGamesUtils.FortniteGGScraper.Model;
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
                i.Name = HttpUtility.HtmlDecode(i.Name);
                i.Description = HttpUtility.HtmlDecode(i.Description);
                i.SourceDescription = HttpUtility.HtmlDecode(i.SourceDescription);
                i.Set = HttpUtility.HtmlDecode(i.Set);
                i.Styles = i.Styles.Select(HttpUtility.HtmlDecode)!;
                return mapper.MapToModel(i);
            });
        }
        catch(Exception e) {
            return null;
        }
    }
}