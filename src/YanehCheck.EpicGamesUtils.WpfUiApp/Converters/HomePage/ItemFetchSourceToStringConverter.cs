using System.Globalization;
using System.Windows.Data;
using YanehCheck.EpicGamesUtils.WpfUiApp.Types.Enums;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Converters.HomePage;

public class ItemFetchSourceToStringConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        if (value is ItemFetchSource itemSource) {
            return itemSource switch {
                ItemFetchSource.AllBundled => "All - Bundled (recommended)",
                ItemFetchSource.ItemsBundled => "Items - Bundled",
                ItemFetchSource.ItemsFortniteGg => "Items - Fortnite.GG",
                ItemFetchSource.ItemsStable => "Items - Stable",
                ItemFetchSource.StylesBundled => "Styles - Bundled",
                ItemFetchSource.StylesDirectoryProperties => "Styles - Directory properties",
                _ => Binding.DoNothing
            };
        }
        return Binding.DoNothing;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        return value switch {
            ItemFetchSource itemSource => itemSource,
            string itemValue => itemValue switch {
                "All - Bundled (recommended)" => ItemFetchSource.AllBundled,
                "Items - Bundled" => ItemFetchSource.ItemsBundled,
                "Items - FortniteGg" or "Items - Fortnite.GG" => ItemFetchSource.ItemsFortniteGg,
                "Items - Stable" => ItemFetchSource.ItemsStable,
                "Styles - Bundled" => ItemFetchSource.StylesBundled,
                "Styles - Directory properties" => ItemFetchSource.StylesDirectoryProperties,
                _ => Binding.DoNothing
            },
            _ => Binding.DoNothing
        };
    }
}