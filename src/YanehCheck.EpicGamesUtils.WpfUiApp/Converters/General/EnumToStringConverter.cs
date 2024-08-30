using System.Globalization;
using System.Windows.Data;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Converters.General;

public class EnumToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || !value.GetType().IsEnum)
        {
            return Binding.DoNothing;
        }

        return value.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || targetType == null || !targetType.IsEnum)
        {
            return Binding.DoNothing;
        }

        return Enum.Parse(targetType, value.ToString());
    }
}