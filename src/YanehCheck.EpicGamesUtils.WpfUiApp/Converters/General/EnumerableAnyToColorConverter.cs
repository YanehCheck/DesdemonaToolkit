using System.Collections;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Converters.General;

public class EnumerableAnyToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return Colors.Transparent;
        }

        if (value is IEnumerable list)
        {
            if (list.Cast<object?>().Any())
            {
                return Application.Current.Resources["SolidBackgroundFillColorTertiaryBrush"] ?? Brushes.Red;
            }
        }

        return Application.Current.Resources["CardBackgroundFillColorDefaultBrush"] ?? Brushes.Red;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}