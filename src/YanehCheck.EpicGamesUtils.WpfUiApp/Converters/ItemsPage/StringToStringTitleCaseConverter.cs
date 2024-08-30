using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Converters.ItemsPage;

public class StringToStringTitleCaseConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        if(value == null) {
            return Binding.DoNothing;
        }

        if(value is string str) {
            var titleCase = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
            var adjustedCase = Regex.Replace(titleCase, @"(\s(of|in|by|and)|\'[st])\b", m => m.Value.ToLower(), RegexOptions.IgnoreCase);
            var adjustedForFortniteKws = Regex.Replace(titleCase, @"(\s(FNCS)|\'[st])\b", m => m.Value.ToUpper(), RegexOptions.IgnoreCase);
            return adjustedForFortniteKws;
        }

        return Binding.DoNothing;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}