using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using YanehCheck.EpicGamesUtils.WpfUiApp.Types.Enums;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Converters.HomePage;

public class FetchStatusToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is FetchStatus status)
        {
            return status == FetchStatus.NotFetched ?
                Application.Current.Resources["SystemFillColorCriticalBrush"] ?? Brushes.Red :
                Application.Current.Resources["SystemFillColorSuccessBrush"] ?? Brushes.GreenYellow;
        }

        return Binding.DoNothing;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}