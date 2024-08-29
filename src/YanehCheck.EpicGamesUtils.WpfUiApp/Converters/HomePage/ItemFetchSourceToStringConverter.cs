using System.Globalization;
using System.Windows.Data;
using YanehCheck.EpicGamesUtils.WpfUiApp.Types.Enums;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Converters.HomePage;

public class ItemFetchSourceToStringConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        if (value is ItemFetchSource itemSource) {
            return itemSource switch {
                ItemFetchSource.FortniteGg => "Items - Fortnite.GG",
                ItemFetchSource.Stable => "Items - Stable",
                ItemFetchSource.StylesBundledWithApp => "Styles - Application",
                ItemFetchSource.StylesDirectoryProperties => "Styles - Directory properties",
                _ => throw new ArgumentException("ItemSourceToStringConverterInvalidValue")
            };
        }
        throw new ArgumentException("ItemSourceToStringConverterInvalidValue");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        return value switch {
            ItemFetchSource itemSource => itemSource,
            string itemValue => itemValue switch {
                "Items - FortniteGg" or "Items - Fortnite.GG" => ItemFetchSource.FortniteGg,
                "Items - Stable" => ItemFetchSource.Stable,
                "Styles - Application" => ItemFetchSource.StylesBundledWithApp,
                "Styles - Directory properties" => ItemFetchSource.StylesDirectoryProperties,
                _ => throw new ArgumentException("ItemSourceToStringConverterInvalidValue")
            },
            _ => throw new ArgumentException("ItemSourceToStringConverterInvalidValue")
        };
    }
}