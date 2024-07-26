using System.Collections.Concurrent;
using YanehCheck.EpicGamesUtils.FortniteGGScraper.Model;

namespace YanehCheck.EpicGamesUtils.FortniteGGScraper;

public interface IFortniteGgScrapper {
    public Task<List<FortniteGgItem>> ScrapIdRangeAsync(int from, int to);
    Task<ConcurrentBag<FortniteGgItem>> ScrapIdRangeParallelAsync(int from, int to);
    Task<FortniteGgItem?> ScrapItemAsync(string id);
}