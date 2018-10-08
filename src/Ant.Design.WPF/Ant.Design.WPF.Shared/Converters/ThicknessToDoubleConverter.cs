using System;
using System.Globalization;
using System.Linq;
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

    public class DoubleToThicknessMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var parent = values.Length > 0 ? values[0] as double? : null;
            var self   = values.Length > 1 ? values[1] as double? : null;

            if (parent == null || self == null)
            {
                return default(Thickness);
            }

            double param;
            double.TryParse(parameter as string, out param);

            return new Thickness(parent.Value - self.Value - param, 0.0, 0.0, 0.0);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return targetTypes.Select(t => DependencyProperty.UnsetValue).ToArray();
        }
    }
}
