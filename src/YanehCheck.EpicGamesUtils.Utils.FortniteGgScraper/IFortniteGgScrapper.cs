using System.Collections.Concurrent;
using YanehCheck.EpicGamesUtils.Utils.FortniteGgScraper.Model;

namespace YanehCheck.EpicGamesUtils.Utils.FortniteGgScraper;

public interface IFortniteGgScrapper {
    public Task<List<FortniteGgItem>> ScrapIdRangeAsync(int from, int to);
    Task<ConcurrentBag<FortniteGgItem>> ScrapIdRangeParallelAsync(int from, int to);
    Task<ConcurrentBag<FortniteGgItem>> ScrapIdRangeParallelAsync(int from, int to, Action<double>? progressReport);
    Task<FortniteGgItem?> ScrapItemAsync(string id);
}