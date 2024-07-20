using YanehCheck.EpicGamesUtils.FortniteGGScraper.Model;

namespace YanehCheck.EpicGamesUtils.ConsoleApp.Items;

public interface IUriItemFetcher
{
    Task<string?> FetchAsJson();
    Task<IEnumerable<Item>?> Fetch();
}