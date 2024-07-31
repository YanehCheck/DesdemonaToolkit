using System.Globalization;
using System.Windows.Data;
using YanehCheck.EpicGamesUtils.WpfUiApp.Helpers.Enums;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Converters.HomePage;

public class ItemFetchSourceToStringConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        if (value is ItemFetchSource itemSource) {
            return itemSource switch {
                ItemFetchSource.FortniteGg => "Fortnite.GG",
                ItemFetchSource.Stable => "Stable",
                _ => throw new ArgumentException("ItemSourceToStringConverterInvalidValue")
            };
        }
        throw new ArgumentException("ItemSourceToStringConverterInvalidValue");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        return value switch {
            ItemFetchSource itemSource => itemSource,
            string itemValue => itemValue switch {
                "FortniteGg" or "Fortnite.GG" => ItemFetchSource.FortniteGg,
                "Stable" => ItemFetchSource.Stable,
                _ => throw new ArgumentException("ItemSourceToStringConverterInvalidValue")
            },
            _ => throw new ArgumentException("ItemSourceToStringConverterInvalidValue")
        };
    }
}