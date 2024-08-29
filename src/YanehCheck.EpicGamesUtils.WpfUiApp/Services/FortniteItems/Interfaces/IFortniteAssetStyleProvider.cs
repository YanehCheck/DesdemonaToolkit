using System.Collections.Concurrent;
using YanehCheck.EpicGamesUtils.Db.Bl.Models;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems.Interfaces;

public interface IFortniteAssetStyleProvider {
    Task<ConcurrentBag<ItemStyleModel>> GetFromPackagePropertiesFileRecursive(string directory, Action<double>? progressReport = null);
    Task<List<ItemStyleModel>?> GetFromSerializedJson(string file, Action<double>? progressReport = null);
}