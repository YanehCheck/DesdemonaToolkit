using YanehCheck.EpicGamesUtils.WpfUiApp.Services.EpicGames.Results;

namespace YanehCheck.EpicGamesUtils.EgsApi.Service.Dtos;

public class FortniteItemsResult(List<OwnedFortniteItem> items) : IResult
{
    public List<OwnedFortniteItem> Items { get; set; } = items;
}