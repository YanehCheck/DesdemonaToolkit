using YanehCheck.EpicGamesUtils.EgsApi.Api.Responses.DataObjects;
using YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames.Results;

namespace YanehCheck.EpicGamesUtils.EgsApi.Service.Mappers;

internal class ItemMapper {
    public OwnedFortniteItem MapFromItem(Item item) {
        var variants = item.Attributes.Variants?.Select(v => new OwnedVariant(v.Channel, v.OwnedProperties)).ToList();
        var ownedItem = new OwnedFortniteItem(item.FortniteId, variants ?? []);
        return ownedItem;
    }
}