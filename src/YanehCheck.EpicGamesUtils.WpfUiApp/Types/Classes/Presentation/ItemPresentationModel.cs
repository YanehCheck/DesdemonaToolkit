using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;
using YanehCheck.EpicGamesUtils.Db.Bl.Models;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Types.Classes.Presentation;

/// <summary>
/// Version of <see cref="ItemOwnedModel"/> with added presentation-related properties and commands.
/// </summary>
[INotifyPropertyChanged]
public partial class ItemPresentationModel : ItemOwnedModel {
    [ObservableProperty] private BitmapFrame? _bitmapFrame;

    [ObservableProperty] private bool _detailFlyoutOpened;

    [RelayCommand]
    public void ToggleItemDetailFlyout() => DetailFlyoutOpened = !DetailFlyoutOpened;

    public bool FromChallenge => PriceVbucks == null && PriceUsd == null && SourceDescription != null;
    public IEnumerable<IEnumerable<StyleNamePresentation>> StylesToPresent => GetStylesToPresent();

    private IEnumerable<IEnumerable<StyleNamePresentation>> GetStylesToPresent() {
        var stylesByChannel = new Dictionary<string, List<(string Name, string Channel, string Property)>>();

        // Process ItemStyleModel objects
        foreach(var style in OwnedStyles) {
            if(!stylesByChannel.ContainsKey(style.Channel)) {
                stylesByChannel[style.Channel] = new List<(string, string, string)>();
            }

            var altName = $"{style.Channel}:{style.Property}";
            string stylePresentation = style.Name ?? altName;
            stylesByChannel[style.Channel].Add((stylePresentation,style.Channel, style.Property));
        }

        // Fallback if some data is missing
        foreach(var rawStyle in OwnedStylesRaw) {
            if(!stylesByChannel.ContainsKey(rawStyle.Channel)) {
                stylesByChannel[rawStyle.Channel] = new List<(string, string, string)>();
            }

            foreach(var property in rawStyle.Property) {
                string rawStylePresentation = $"{rawStyle.Channel}:{property}";
                if(stylesByChannel[rawStyle.Channel].All(s => s.Name != rawStylePresentation) &&
                    !OwnedStyles.Any(s => s.Channel == rawStyle.Channel && s.Property == property)) {
                    stylesByChannel[rawStyle.Channel].Add((rawStylePresentation, rawStyle.Channel, property));
                }
            }
        }

        // Sort styles in each channel by Property, normal string sort should do the trick
        // Apply some transformation to things that don't have a name defined
        // Add a flag that says if the name is a property, so it received slightly dimmer color
        return stylesByChannel.Select(kvp => kvp.Value
            .OrderBy(s => s.Property)
            .Select(s => {
                var newName = TransformPresentationString(s.Name, s.Property);
                return new StyleNamePresentation(newName, newName == $"{s.Channel}:{s.Property}");
            }));

        string TransformPresentationString(string str, string property) {
            // Case for painted vehicle items:
            var vehiclePrefix = "Vehicle.Painted.";
            if (property.StartsWith(vehiclePrefix)) {
                // Split the pascal case
                var colorPascalCase = property.Substring(vehiclePrefix.Length);
                return string.Join(" ", Regex.Split(colorPascalCase, @"(?<!^)(?=[A-Z])"));
            }

            return str;
        }
    }


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
        FortniteGgStyles = item.FortniteGgStyles ?? [];
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
        FortniteGgStyles = item.FortniteGgStyles ?? [];
        Tags = item.Tags ?? [];
    }
}