using System.Text.Json;
using Microsoft.Extensions.Options;
using YanehCheck.EpicGamesUtils.ConsoleApp.Options;
using YanehCheck.EpicGamesUtils.FortniteGGScraper.Model;

namespace YanehCheck.EpicGamesUtils.ConsoleApp.Items;

public class ItemManager(IOptions<ItemOptions> config) : IItemManager {
    public async Task Save(IEnumerable<Item> items) {
        var json = JsonSerializer.Serialize(items);

        Directory.CreateDirectory(Path.GetDirectoryName(config.Value.ItemsSavePath)!);
        await File.WriteAllTextAsync(config.Value.ItemsSavePath, json);
    }

    public async Task Save(string json) {
        Directory.CreateDirectory(Path.GetDirectoryName(config.Value.ItemsSavePath)!);
        await File.WriteAllTextAsync(config.Value.ItemsSavePath, json);
    }

    public async Task<IEnumerable<Item>?> Load() {
        if (!Directory.Exists(config.Value.ItemsSavePath) || !File.Exists(config.Value.ItemsSavePath)) {
            return null;
        }

        var json = await File.ReadAllTextAsync(config.Value.ItemsSavePath);
        var items = JsonSerializer.Deserialize<List<Item>>(json);
        return items;

    }
}