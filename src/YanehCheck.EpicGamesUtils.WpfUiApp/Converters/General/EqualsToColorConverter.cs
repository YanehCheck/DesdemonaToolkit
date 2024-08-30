using System.Globalization;
using System.Windows.Data;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Converters.General;

public class EqualsToColorConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        if(value == null || parameter == null) {
            return Binding.DoNothing;
        }


        if(value.Equals(parameter)) {
            return Application.Current.Resources["SolidBackgroundFillColorTertiaryBrush"] ?? Binding.DoNothing;
        }


        return Application.Current.Resources["CardBackgroundFillColorDefaultBrush"] ?? Binding.DoNothing;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}