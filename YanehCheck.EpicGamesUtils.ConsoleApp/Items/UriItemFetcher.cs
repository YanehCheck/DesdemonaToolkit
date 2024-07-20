using System.Text.Json;
using Microsoft.Extensions.Options;
using RestSharp;
using YanehCheck.EpicGamesUtils.ConsoleApp.Options;
using YanehCheck.EpicGamesUtils.FortniteGGScraper.Model;

namespace YanehCheck.EpicGamesUtils.ConsoleApp.Items;

public class UriItemFetcher(IRestClient client, IOptions<ItemOptions> config) : IUriItemFetcher {
    
    public async Task<string?> FetchAsJson() {
        var request = new RestRequest(config.Value.StableItemDataUri);
        var response = await client.GetAsync(request);
        return response.IsSuccessful ? response.Content : null;
    }

    public async Task<IEnumerable<Item>?> Fetch() {
        var json = await FetchAsJson();
        if (json == null) {
            return null;
        }

        return (List<Item>) JsonSerializer.Deserialize(json, typeof(List<Item>))!;
    }
}