using System.Collections.Concurrent;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using YanehCheck.EpicGamesUtils.FortniteGGScraper;
using YanehCheck.EpicGamesUtils.FortniteGGScraper.Model;

namespace Test;
internal class Program {
    private static readonly string dest = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "fortnite-items-ml.json");

    static async Task Main(string[] args) {
        var scrapper = new FortniteGgScrapper();
        var jsonOptions = new JsonSerializerOptions() {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            AllowTrailingCommas = false,
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        var items = new ConcurrentBag<Item>();

        // Use Parallel.ForEachAsync for concurrent item processing
        await Parallel.ForEachAsync(Enumerable.Range(0, 13000), async (i, token) => {
            Item? item = null;
                item = await scrapper.ScrapItemAsync(i.ToString());

            if(item != null) {
                items.Add(item);
                Console.WriteLine(i);
            }
        });

        Console.WriteLine("[");

        foreach(var item in items) {
            var json = JsonSerializer.Serialize(item, jsonOptions);

            Console.WriteLine(json + ",");
        }

        Console.WriteLine("]");
    }
}