using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Antd.Converters
{
    public class StringToBooleanConverter : IValueConverter
    {
        private static StringToBooleanConverter instance;

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static StringToBooleanConverter() { }

        private StringToBooleanConverter() { }

        public static StringToBooleanConverter Instance
        {
            get { return instance ?? (instance = new StringToBooleanConverter()); }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrEmpty((string)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
