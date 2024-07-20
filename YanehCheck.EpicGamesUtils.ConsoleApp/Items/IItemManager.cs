using YanehCheck.EpicGamesUtils.FortniteGGScraper.Model;

namespace YanehCheck.EpicGamesUtils.ConsoleApp.Items;

public interface IItemManager {
    Task Save(IEnumerable<Item> items);
    Task Save(string json);
    Task<IEnumerable<Item>?> Load();
}