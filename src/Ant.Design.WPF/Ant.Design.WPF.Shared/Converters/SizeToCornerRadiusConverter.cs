using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Antd.Converters
{
    public class SizeToCornerRadiusConverter : IValueConverter
    {
        private static SizeToCornerRadiusConverter instance;

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static SizeToCornerRadiusConverter() { }

        private SizeToCornerRadiusConverter() { }

        public static SizeToCornerRadiusConverter Instance
        {
            get { return instance ?? (instance = new SizeToCornerRadiusConverter()); }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var size = (double)value;

            if (double.IsNaN(size) || size < 1)
            {
                size = 0;
            } else
            {
                double val;
                double.TryParse(parameter as string, out val);

                if (val > 0)
                {
                    size /= val;
                }
            }
   
            return new CornerRadius(size);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    public class CornerRadiusToSizeConverter : IValueConverter
    {
        private static CornerRadiusToSizeConverter instance;

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static CornerRadiusToSizeConverter() { }

        private CornerRadiusToSizeConverter() { }

        public static CornerRadiusToSizeConverter Instance
        {
            get { return instance ?? (instance = new CornerRadiusToSizeConverter()); }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var cornerRadius = (CornerRadius)value;
            return Math.Max(cornerRadius.TopLeft, Math.Max(cornerRadius.TopRight, Math.Max(cornerRadius.BottomRight, cornerRadius.BottomLeft)));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
