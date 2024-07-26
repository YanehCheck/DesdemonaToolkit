using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Converters;

public class DateToColorConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        if(value is DateTime date) {
            return date > DateTime.Now ? 
                Application.Current.Resources["SystemFillColorCriticalBrush"] ?? Brushes.Red :
                Application.Current.Resources["SystemFillColorSuccessBrush"] ?? Brushes.GreenYellow;
        }

        return Brushes.Black;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}