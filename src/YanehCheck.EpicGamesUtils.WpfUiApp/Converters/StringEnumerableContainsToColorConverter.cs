using System.Collections;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Converters;

public class StringEnumerableContainsToColorConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        if(value == null || parameter == null) {
            return Colors.Transparent;
        }

        if(value is IEnumerable list) {
            if (list.Cast<object?>().Any(item => item.Equals(parameter))) {
                return Application.Current.Resources["SolidBackgroundFillColorTertiaryBrush"] ?? Brushes.Red;
            }
        }

        return Application.Current.Resources["CardBackgroundFillColorDefaultBrush"] ?? Brushes.Red;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}