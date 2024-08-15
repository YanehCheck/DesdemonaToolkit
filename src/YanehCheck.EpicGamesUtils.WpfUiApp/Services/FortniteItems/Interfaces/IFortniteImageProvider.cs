using System.Windows.Media.Imaging;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems.Interfaces;

public interface IFortniteImageProvider
{
    Task<BitmapFrame> GetImageAsync(string fortniteGgId);
    Task<BitmapFrame> FetchImageFromFortniteGgAsync(string fortniteGgId, int width, int height);
}