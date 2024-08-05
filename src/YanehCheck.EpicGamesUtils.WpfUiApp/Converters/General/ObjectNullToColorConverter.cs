using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Converters.General;

public class ObjectNullToColorConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        if(value == null) {
            return Application.Current.Resources["CardBackgroundFillColorDefaultBrush"] ?? Brushes.Red;
        }

        return Application.Current.Resources["SolidBackgroundFillColorTertiaryBrush"] ?? Brushes.Red;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}