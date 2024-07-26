using System.Text.Json;
using RestSharp;
using YanehCheck.EpicGamesUtils.BL.Models;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems.Interfaces;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems;

public class UriItemProvider(IRestClient restClient) : IUriItemProvider
{
    public async Task<IEnumerable<ItemFullModel>?> GetItemsAsync()
    {
        var request = new RestRequest("123");
        var response = await restClient.GetAsync(request);
        return response.IsSuccessful ?
            (List<ItemFullModel>)JsonSerializer.Deserialize(response.Content!, typeof(List<ItemFullModel>))! :
            null;
    }
}