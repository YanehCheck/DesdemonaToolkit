using SixLabors.ImageSharp;
using ItemPresentationModel = YanehCheck.EpicGamesUtils.WpfUiApp.Types.Classes.Presentation.ItemPresentationModel;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems.Interfaces;

public interface IFortniteInventoryImageProcessor {
    Image Create(List<ItemPresentationModel> items, string displayName);
}