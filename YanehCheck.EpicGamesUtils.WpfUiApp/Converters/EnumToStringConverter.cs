using System.Globalization;
using System.Windows.Data;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Converters;

public class EnumToStringConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        if(value == null || !value.GetType().IsEnum) {
            throw new ArgumentException("EnumToStringConverterInvalidEnumValue");
        }

        return value.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        if(value == null || targetType == null || !targetType.IsEnum) {
            throw new ArgumentException("EnumToStringConverterInvalidValue");
        }

        return Enum.Parse(targetType, value.ToString());
    }
}