using YanehCheck.EpicGamesUtils.Db.Bl.Models;
using YanehCheck.EpicGamesUtils.Utils.FortniteGgScraper.Model;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.FortniteItems.Mappers;

public class FortniteGgItemMapper : IFortniteGgItemMapper {
    public ItemModel MapToModel(FortniteGgItem item) {
        return new ItemModel() {
            // Id = null,
            FortniteId = item.Id,
            FortniteGgId = item.FortniteGgId,
            Name = item.Name,
            Description = item.Description,
            PriceVbucks = item.PriceVbucks,
            PriceUsd = item.PriceUsd,
            Season = item.Season,
            Source = item.Source,
            SourceDescription = item.SourceDescription,
            Rarity = item.Rarity,
            Type = item.Type,
            Set = item.Set,
            Release = item.Release,
            LastSeen = item.LastSeen,
            Styles = item.Styles ?? [],
            Tags = item.Tags ?? []
        };
    }

}