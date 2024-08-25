using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Windows.Media.Imaging;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems.Interfaces;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.Options;
using YanehCheck.EpicGamesUtils.WpfUiApp.Utilities.Options.Interfaces;
using Size = System.Drawing.Size;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems;

public class FortniteImageProvider(IWritableOptions<ItemImageCachingOptions> options) : IFortniteImageProvider {
    private readonly HttpClient httpClient = new();

    public string ImageDirectory { get; } = Path.Combine(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule!.FileName)!, "data/images");

    private string Url(string fortniteGgId) => $"https://fortnite.gg/img/items/{fortniteGgId}/icon.jpg";

    public async Task<BitmapFrame> GetImageAsync(string fortniteGgId) {
        Directory.CreateDirectory(ImageDirectory);
        var filePath = Path.Combine(ImageDirectory, $"{fortniteGgId}.jpg");

        if(File.Exists(filePath)) {
            await using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
        }
        else {
            var image = await FetchImageFromFortniteGgAsync(fortniteGgId);
            if (options.Value.CacheDownloadedImages) {
                await SaveImageToFileAsync(image, filePath);
            }
            return image;
        }
    }

    public async Task<BitmapFrame> FetchImageFromFortniteGgAsync(string fortniteGgId)
    {
        var buffer = await httpClient.GetByteArrayAsync(Url(fortniteGgId));
        using var stream = new MemoryStream(buffer);
        return BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
    }

    public async Task<BitmapFrame> FetchImageFromFortniteGgAsync(string fortniteGgId, int width, int height) {
        var buffer = await httpClient.GetByteArrayAsync(Url(fortniteGgId));
        using var originalStream = new MemoryStream(buffer);
        using var originalBitmap = new Bitmap(originalStream);
        using var resizedBitmap = new Bitmap(originalBitmap, new Size(width, height));

        using var resizedStream = new MemoryStream();
        resizedBitmap.Save(resizedStream, ImageFormat.Png);
        resizedStream.Position = 0;

        return BitmapFrame.Create(resizedStream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
    }

    private async Task SaveImageToFileAsync(BitmapFrame image, string filePath) {
        await using var fileStream = new FileStream(filePath, FileMode.Create);
        var encoder = new JpegBitmapEncoder();
        encoder.Frames.Add(image);
        encoder.Save(fileStream);
        await fileStream.FlushAsync();
    }
}