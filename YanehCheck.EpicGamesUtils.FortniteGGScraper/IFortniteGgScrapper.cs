using System.Collections.Concurrent;
using YanehCheck.EpicGamesUtils.FortniteGGScraper.Model;

namespace YanehCheck.EpicGamesUtils.FortniteGGScraper;

public interface IFortniteGgScrapper {
    public Task<List<Item>> ScrapIdRangeAsync(int from, int to);
    Task<ConcurrentBag<Item>> ScrapIdRangeParallelAsync(int from, int to);
    Task<Item?> ScrapItemAsync(string id);
}