using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Converters;

public class EnumEqualsToColorConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        if(value == null || parameter == null) {
            return Colors.Transparent;
        }

        if(value.GetType().IsEnum && parameter.GetType().IsEnum) {
            if(value.Equals(parameter)) {
                return Application.Current.Resources["SolidBackgroundFillColorTertiaryBrush"] ?? Brushes.Red;
            }
        }

        return Application.Current.Resources["CardBackgroundFillColorDefaultBrush"] ?? Brushes.Red;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}