using System.Globalization;
using System.Windows.Data;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Converters.General;

public class ObjectNullToColorConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        if(value == null) {
            return Application.Current.Resources["CardBackgroundFillColorDefaultBrush"] ?? Binding.DoNothing;
        }

        return Application.Current.Resources["SolidBackgroundFillColorTertiaryBrush"] ?? Binding.DoNothing;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}