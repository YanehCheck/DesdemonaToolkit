using System.Windows.Media.Imaging;
using YanehCheck.EpicGamesUtils.BL.Models;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Models;

[INotifyPropertyChanged]
public partial class ItemWithImageModel : ItemModel {
    [ObservableProperty] 
    private BitmapFrame? _bitmapFrame;

    public ItemWithImageModel() { }

    public ItemWithImageModel(ItemModel item) {
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