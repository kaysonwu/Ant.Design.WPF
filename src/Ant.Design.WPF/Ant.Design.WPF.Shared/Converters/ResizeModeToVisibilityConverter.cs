using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Antd.Converters
{
    [ValueConversion(typeof(ResizeMode), typeof(Visibility))]
    class ResizeModeToVisibilityConverter : IValueConverter
    {
        public object Convert(object values, Type targetType, object parameter, CultureInfo culture)
        {
            var mode = (ResizeMode)values;
            var type = parameter as string;

            return mode == ResizeMode.NoResize || (type != "MIN" && mode == ResizeMode.CanMinimize) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
