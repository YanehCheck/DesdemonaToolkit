using System.Globalization;
using System.Windows.Data;
using YanehCheck.EpicGamesUtils.Common.Enums.Items;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Converters.ItemsPage;

public class ItemSourceToStringConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        return value switch {
            null => "",
            ItemSource source => source.ToReadableString(),
            _ => ""
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}