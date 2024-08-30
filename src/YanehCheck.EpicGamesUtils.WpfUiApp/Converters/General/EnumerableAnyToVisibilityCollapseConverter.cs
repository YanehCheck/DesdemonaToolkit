using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Converters.General;

public class EnumerableAnyToVisibilityCollapseConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        return value switch {
            null => Visibility.Collapsed,
            IEnumerable list when list.Cast<object?>().Any() => Visibility.Visible,
            _ => Binding.DoNothing
    };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}