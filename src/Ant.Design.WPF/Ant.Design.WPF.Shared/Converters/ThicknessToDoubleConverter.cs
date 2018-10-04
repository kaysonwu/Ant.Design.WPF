using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Antd.Converters
{
    [ValueConversion(typeof(Thickness), typeof(double))]
    public class ThicknessToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Thickness))
            {
                return default(double);
            }

            var thickness = (Thickness)value;
            var d = Math.Max(thickness.Left, Math.Max(thickness.Top, Math.Max(thickness.Right, thickness.Bottom)));

            // If it is a shape, it may be necessary to discard approximately 0.2px to remove the sawtooth.
            // Recorded in: Window10 x64, xiaomi laptop pro15.6
            if (bool.Parse(parameter as string))
            {
                d -= d * 0.176;
            }

            return d;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
