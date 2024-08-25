using System.Globalization;
using System.Windows.Data;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Converters.General;

public class EnumToUppercaseStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Enum enumValue)
        {
            return enumValue.ToString().ToUpperInvariant();
        }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string stringValue && targetType.IsEnum)
        {
            return Enum.Parse(targetType, stringValue, true);
        }
        return Binding.DoNothing;
    }
}