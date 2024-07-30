using System.Globalization;
using System.Windows.Data;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Converters;

public class EnumEqualsToColorTintConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        if(value == null || parameter == null) {
            return "#00000000";
        }

        if(value.GetType().IsEnum && parameter.GetType().IsEnum) {
            if(value.Equals(parameter)) {
                return "#B0000000";
            }
        }

        return "#00000000";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}