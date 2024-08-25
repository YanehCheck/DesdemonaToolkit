using System.Windows.Media.Imaging;
using YanehCheck.EpicGamesUtils.Db.Bl.Models;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Types.Models;

/// <summary>
/// Version of <see cref="ItemOwnedModel"/> with added presentation-related properties and commands.
/// </summary>
[INotifyPropertyChanged]
public partial class ItemPresentationModel : ItemOwnedModel {
    [ObservableProperty] 
    private BitmapFrame? _bitmapFrame;

    [ObservableProperty] 
    private bool _detailFlyoutOpened;

    [RelayCommand]
    public void ToggleItemDetailFlyout() => DetailFlyoutOpened = !DetailFlyoutOpened;

    public bool FromChallenge => PriceVbucks == null && PriceUsd == null && SourceDescription != null;

    public ItemPresentationModel() { }

    public ItemPresentationModel(ItemOwnedModel item) {
        Id = item.Id;
        FortniteId = item.FortniteId;
        FortniteGgId = item.FortniteGgId;
        Name = item.Name;
        Description = item.Description;
        PriceVbucks = item.PriceVbucks;
        PriceUsd = item.PriceUsd;
        Season = item.Season;
        Source = item.Source;
        SourceDescription = item.SourceDescription;
        Rarity = item.Rarity;
        Type = item.Type;
        Set = item.Set;
        Release = item.Release;
        LastSeen = item.LastSeen;
        Styles = item.Styles ?? [];
        Tags = item.Tags ?? [];
        OwnedStylesRaw = item.OwnedStylesRaw;
        Remark = item.Remark;
    }

    public ItemPresentationModel(ItemModel item) {
        Id = item.Id;
        FortniteId = item.FortniteId;
        FortniteGgId = item.FortniteGgId;
        Name = item.Name;
        Description = item.Description;
        PriceVbucks = item.PriceVbucks;
        PriceUsd = item.PriceUsd;
        Season = item.Season;
        Source = item.Source;
        SourceDescription = item.SourceDescription;
        Rarity = item.Rarity;
        Type = item.Type;
        Set = item.Set;
        Release = item.Release;
        LastSeen = item.LastSeen;
        Styles = item.Styles ?? [];
        Tags = item.Tags ?? [];
    }
}