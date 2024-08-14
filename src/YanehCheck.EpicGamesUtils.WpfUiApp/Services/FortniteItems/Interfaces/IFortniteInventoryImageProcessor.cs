using SixLabors.ImageSharp;
using YanehCheck.EpicGamesUtils.WpfUiApp.Models;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems.Interfaces;

public interface IFortniteInventoryImageProcessor {
    Image Create(List<ItemPresentationModel> items, string displayName);
}