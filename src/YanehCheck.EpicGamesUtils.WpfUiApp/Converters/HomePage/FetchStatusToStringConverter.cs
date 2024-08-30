using System.Globalization;
using System.Windows.Data;
using YanehCheck.EpicGamesUtils.WpfUiApp.Types.Enums;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Converters.HomePage;

public class FetchStatusToStringConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        if(value is FetchStatus status) {
            return status switch {
                FetchStatus.NotFetched => "Not found.",
                FetchStatus.BundledSource => "Using bundled data.",
                FetchStatus.StableSource => "Fetched from stable source on:",
                FetchStatus.UpToDateSource => "Fetched from up-to-date source on:",
                _ => Binding.DoNothing
            };
        }

        return Binding.DoNothing;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}