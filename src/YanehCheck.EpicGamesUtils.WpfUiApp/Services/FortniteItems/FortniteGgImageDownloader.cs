using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Windows.Media.Imaging;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems.Interfaces;
using Size = System.Drawing.Size;

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

    public async Task<BitmapFrame> GetImageAsync(string fortniteGgId, int width, int height) {
        var buffer = await httpClient.GetByteArrayAsync(Url(fortniteGgId));
        using var originalStream = new MemoryStream(buffer);
        using var originalBitmap = new Bitmap(originalStream);
        using var resizedBitmap = new Bitmap(originalBitmap, new Size(width, height));

        using var resizedStream = new MemoryStream();
        resizedBitmap.Save(resizedStream, ImageFormat.Png);
        resizedStream.Position = 0;

        return BitmapFrame.Create(resizedStream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
    }
}