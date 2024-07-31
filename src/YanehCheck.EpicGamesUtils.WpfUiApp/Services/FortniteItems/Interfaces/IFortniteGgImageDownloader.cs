using System.Windows.Media.Imaging;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems.Interfaces;

public interface IFortniteGgImageDownloader
{
    Task<BitmapFrame> GetImageAsync(string fortniteGgId);
    Task<BitmapFrame> GetImageAsync(string fortniteGgId, int width, int height);
}