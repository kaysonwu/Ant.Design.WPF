using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Antd.Converters
{
    [ValueConversion(typeof(double), typeof(CornerRadius))]
    public class DoubleToCornerRadiusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is double))
            {
                return default(CornerRadius);
            }

            var d = (double)value;

            if (double.IsNaN(d) || d < 1)
            {
                d = 0;
            } else
            {
                // May require 50% rounded corners
                double val;
                double.TryParse(parameter as string, out val);

                if (val > 0)
                {
                    d /= val;
                }
            }
   
            return new CornerRadius(d);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    [ValueConversion(typeof(CornerRadius), typeof(double))]
    public class CornerRadiusToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is CornerRadius))
            {
                return default(double);
            }

            var cornerRadius = (CornerRadius)value;
            return Math.Max(cornerRadius.TopLeft, Math.Max(cornerRadius.TopRight, Math.Max(cornerRadius.BottomRight, cornerRadius.BottomLeft)));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
