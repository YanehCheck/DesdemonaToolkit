using System.Text.Json;
using Microsoft.Extensions.Options;
using RestSharp;
using YanehCheck.EpicGamesUtils.BL.Models;
using YanehCheck.EpicGamesUtils.FortniteGGScraper.Model;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems.Mappers;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.Options;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems;

public class UriItemProvider(IRestClient restClient, IFortniteGgItemMapper mapper, IOptions<ItemFetchOptions> options) : IUriItemProvider
{
    public async Task<IEnumerable<ItemModel>?> GetItemsAsync()
    {
        var request = new RestRequest(options.Value.StableSourceUri);
        var response = await restClient.GetAsync(request);
        if (!response.IsSuccessStatusCode) {
            return null;
        }

        var items = (List<FortniteGgItem>) JsonSerializer.Deserialize(response.Content!, typeof(List<FortniteGgItem>))!;
        return items.Select(mapper.MapToModel);
    }
}