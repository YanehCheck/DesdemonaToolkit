using System.Globalization;
using System.Windows.Data;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Converters.General;

public class EqualsToVisibilityConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        if(value == null || parameter == null) {
            return Visibility.Hidden;
        }


        if(value.Equals(parameter)) {
            return Visibility.Hidden;
        }


        return Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}