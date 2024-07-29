using System.Windows.Media.Imaging;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems.Interfaces;

public interface IFortniteGgImageDownloader
{
    Task<BitmapFrame> GetImageAsync(string fortniteGgId);
}