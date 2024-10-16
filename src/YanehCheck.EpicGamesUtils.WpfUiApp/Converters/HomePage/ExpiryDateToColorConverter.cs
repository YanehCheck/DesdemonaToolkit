﻿using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Converters.HomePage;

public class ExpiryDateToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DateTime date)
        {
            return date < DateTime.Now ?
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