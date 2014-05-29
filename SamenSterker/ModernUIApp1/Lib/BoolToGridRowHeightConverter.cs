using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace UserInteface.Lib
{

    // source : http://stackoverflow.com/a/21884516
    /// <summary>
    /// Bool to Grid Row Height Convertor
    /// </summary>
    [ValueConversion(typeof(bool), typeof(GridLength))]
    public class BoolToGridRowHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value == true) ? new GridLength(1, GridUnitType.Star) : new GridLength(0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {    // Don't need any convert back
            return null;
        }
    }
}
