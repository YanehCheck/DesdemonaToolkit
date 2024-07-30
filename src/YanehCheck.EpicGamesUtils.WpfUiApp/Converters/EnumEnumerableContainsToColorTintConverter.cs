using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Converters;

public class EnumEnumerableContainsToColorTintConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        if(value == null || parameter == null) {
            return "#00000000";
        }

        if(value is IEnumerable list && parameter.GetType().IsEnum) {
            if (list.Cast<object?>().Any(item => item.GetType().IsEnum && item.Equals(parameter))) {
                return "#B0000000";
            }
        }

        return "#00000000";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}