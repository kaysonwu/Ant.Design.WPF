using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Antd.Converters
{
    [ValueConversion(typeof(Thickness), typeof(Thickness))]
    public class ThicknessReverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var thickness = (Thickness)value;

            thickness.Left *= -1;
            thickness.Top *= -1;

            thickness.Right *= -1;
            thickness.Bottom *= -1;

            return thickness;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
