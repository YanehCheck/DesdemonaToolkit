using System.Globalization;
using System.Windows.Data;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Converters;

public class DateToStringConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        if(value is DateTime date) {
            return date == DateTime.MinValue ? 
                "Not logged in." :
                date > DateTime.Now ? 
                $"Session expires at: {date:yyyy-M-d HH:mm:ss}" :
                "Expired.";
        }

        return "Not logged in.";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}