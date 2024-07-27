using System.Text.Json;
using Microsoft.Extensions.Options;
using RestSharp;
using YanehCheck.EpicGamesUtils.BL.Models;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.Options;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems;

public class UriItemProvider(IRestClient restClient, IOptions<ItemFetchOptions> options) : IUriItemProvider
{
    public async Task<IEnumerable<ItemFullModel>?> GetItemsAsync()
    {
        var request = new RestRequest(options.Value.StableSourceUri);
        var response = await restClient.GetAsync(request);
        return response.IsSuccessful ?
            (List<ItemFullModel>)JsonSerializer.Deserialize(response.Content!, typeof(List<ItemFullModel>))! :
            null;
    }
}