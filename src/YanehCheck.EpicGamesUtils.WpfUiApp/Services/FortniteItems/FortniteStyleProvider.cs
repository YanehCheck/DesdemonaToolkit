using System.Collections.Concurrent;
using System.IO;
using Newtonsoft.Json;
using YanehCheck.EpicGamesUtils.Db.Bl.Models;
using YanehCheck.EpicGamesUtils.Utils.FortniteAssetSerializer.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems.Mappers;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems;

public class FortniteStyleProvider(IFortniteAssetSerializer assetSerializer, IItemStyleAssetMapper mapper) : IFortniteStyleProvider {
    public async Task<ConcurrentBag<ItemStyleModel>> GetStylesPackagePropertiesFileRecursiveAsync(string directory, Action<double>? progressReport = null) {
        var styles = new ConcurrentBag<ItemStyleModel>();
        var files = Directory.GetFiles(directory, "*.json", SearchOption.AllDirectories);
        
        int progress = 0;
        await Parallel.ForEachAsync(files, async (f, _) => {
            var style = await assetSerializer.DeserializeCosmeticVariantAssetFileAsync(f);
            if(style != null) {
                styles.Add(mapper.MapToModel(style));
                progress++;
                if(progress % 10 == 0) {
                    progressReport?.Invoke((double) progress / files.Length);
                }
            }
        });
        return styles;
    }

    public async Task<List<ItemStyleModel>?> GetStylesJsonFileAsync(string file, Action<double>? progressReport = null) {
        progressReport?.Invoke(0);
        var content = await File.ReadAllTextAsync(file);
        progressReport?.Invoke(0.5);
        var styles = JsonConvert.DeserializeObject<List<ItemStyleModel>>(content);
        progressReport?.Invoke(1);
        return styles;
    }
}