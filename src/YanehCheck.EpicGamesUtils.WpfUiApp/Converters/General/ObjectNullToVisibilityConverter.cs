﻿using System.Globalization;
using System.Windows.Data;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Converters.General;

public class ObjectNullToVisibilityConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        return value is not null ? Visibility.Visible : Visibility.Hidden;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}