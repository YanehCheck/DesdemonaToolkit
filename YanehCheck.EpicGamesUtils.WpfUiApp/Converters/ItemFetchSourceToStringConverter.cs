using System.Globalization;
using System.Windows.Data;
using YanehCheck.EpicGamesUtils.WpfUiApp.Helpers;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Converters;

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
        if(value is string itemValue) {
            return itemValue switch {
                "FortniteGg" or "Fortnite.GG" => ItemFetchSource.FortniteGg,
                "Stable" => ItemFetchSource.Stable,
                _ => throw new ArgumentException("ItemSourceToStringConverterInvalidValue")
            };
        }
        throw new ArgumentException("ItemSourceToStringConverterInvalidValue");
    }
}