using System.Globalization;
using System.Windows.Data;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Converters.ItemsPage;

public class SeasonStringToReadableStringConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        switch (value) {
            case null:
            case string { Length: < 4 }:
                return Binding.DoNothing;
            case string season:
                return $"Chapter {season[1]}, Season {season[3..]}";
            default:
                return Binding.DoNothing;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}