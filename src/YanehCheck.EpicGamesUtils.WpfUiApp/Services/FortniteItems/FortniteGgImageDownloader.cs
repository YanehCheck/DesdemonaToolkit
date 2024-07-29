using System.IO;
using System.Net.Http;
using System.Windows.Media.Imaging;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems.Interfaces;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems;

public class FortniteGgImageDownloader : IFortniteGgImageDownloader {
    private readonly HttpClient httpClient = new();
    private string Url(string fortniteGgId) => $"https://fortnite.gg/img/items/{fortniteGgId}/icon.jpg";
    public async Task<BitmapFrame> GetImageAsync(string fortniteGgId)
    {
        var buffer = await httpClient.GetByteArrayAsync(Url(fortniteGgId));
        using var stream = new MemoryStream(buffer);
        return BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
    }
}