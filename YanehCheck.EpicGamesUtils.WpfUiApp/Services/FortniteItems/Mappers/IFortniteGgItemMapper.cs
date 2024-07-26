using YanehCheck.EpicGamesUtils.BL.Models;
using YanehCheck.EpicGamesUtils.FortniteGGScraper.Model;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems.Mappers;

public interface IFortniteGgItemMapper {
    ItemFullModel MapToModel(FortniteGgItem item);
}