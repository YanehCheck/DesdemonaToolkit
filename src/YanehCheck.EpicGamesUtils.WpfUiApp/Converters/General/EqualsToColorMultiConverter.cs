using System.Globalization;
using System.Windows.Data;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Converters.General;

public class EqualsToColorMultiConverter : IMultiValueConverter {
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
        if (values[0] is null || values[1] is null) {
            return Application.Current.Resources["CardBackgroundFillColorDefaultBrush"] ?? Binding.DoNothing;
        }

        return values[0].Equals(values[1]) ? Application.Current.Resources["SolidBackgroundFillColorTertiaryBrush"] ?? Binding.DoNothing
            : Application.Current.Resources["CardBackgroundFillColorDefaultBrush"] ?? Binding.DoNothing;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}