using System.Collections.Concurrent;
using YanehCheck.EpicGamesUtils.Db.Bl.Models;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems.Interfaces;

public interface IFortniteStyleProvider {
    Task<ConcurrentBag<ItemStyleModel>> GetStylesPackagePropertiesFileRecursiveAsync(string directory, Action<double>? progressReport = null);
    Task<List<ItemStyleModel>?> GetStylesJsonFileAsync(string file, Action<double>? progressReport = null);
}