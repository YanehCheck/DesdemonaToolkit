using System.Globalization;
using System.Windows.Data;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Converters;

public class BoolToVisibilityConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        if(value is bool val) {
            return val ? Visibility.Visible : Visibility.Hidden;
        }

        return Visibility.Hidden;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}