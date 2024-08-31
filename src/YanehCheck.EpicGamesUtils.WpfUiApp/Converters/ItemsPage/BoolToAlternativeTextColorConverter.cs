using System.Globalization;
using System.Windows.Data;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Converters.ItemsPage;

public class BoolToAlternativeTextColorConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        if(value is bool boolValue) {
            return (boolValue ?
                Application.Current.Resources["TextFillColorDisabledBrush"] :
                Application.Current.Resources["TextFillColorSecondaryBrush"]) ?? 
                   Binding.DoNothing;
        }

        return Binding.DoNothing;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}