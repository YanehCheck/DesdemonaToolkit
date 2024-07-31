using System.Windows.Media.Imaging;
using YanehCheck.EpicGamesUtils.BL.Models;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Models;

/// <summary>
/// Version of <see cref="ItemModel"/> with added presentation-related properties and commands.
/// </summary>
[INotifyPropertyChanged]
public partial class ItemPresentationModel : ItemModel {
    [ObservableProperty] 
    private BitmapFrame? _bitmapFrame;

    [ObservableProperty] 
    private bool _detailFlyoutOpened;

    [RelayCommand]
    public void ToggleItemDetailFlyout() => DetailFlyoutOpened = !DetailFlyoutOpened;

    public ItemPresentationModel() { }

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