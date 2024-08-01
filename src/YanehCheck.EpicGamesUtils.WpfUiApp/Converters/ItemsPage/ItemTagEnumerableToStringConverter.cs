using System.Globalization;
using System.Windows.Data;
using YanehCheck.EpicGamesUtils.Common.Enums.Items;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Converters.ItemsPage;

public class ItemTagEnumerableToStringConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        if(value == null) {
            return string.Empty;
        }

        if (value is IEnumerable<ItemTag> tags) {
            return string.Join(", ", tags.Select(s => "[" + s + "]"));
        }

        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}