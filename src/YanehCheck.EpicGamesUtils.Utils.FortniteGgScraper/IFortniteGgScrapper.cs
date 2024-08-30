using System.Collections.Concurrent;
using YanehCheck.EpicGamesUtils.Utils.FortniteGgScraper.Model;

namespace YanehCheck.EpicGamesUtils.Utils.FortniteGgScraper;

public interface IFortniteGgScrapper {
    public Task<List<FortniteGgItemDto>> ScrapIdRangeAsync(int from, int to);
    Task<ConcurrentBag<FortniteGgItemDto>> ScrapIdRangeParallelAsync(int from, int to);
    Task<ConcurrentBag<FortniteGgItemDto>> ScrapIdRangeParallelAsync(int from, int to, Action<double>? progressReport);
    Task<FortniteGgItemDto?> ScrapItemAsync(string id);
}