using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Antd.Converters
{
    public class ToUpperConverter : IValueConverter
    {
        private static ToUpperConverter instance;

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static ToUpperConverter() {}

        private ToUpperConverter() { }

        public static ToUpperConverter Instance
        {
            get { return instance ?? (instance = new ToUpperConverter()); }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as string)?.ToUpper();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    public class ToLowerConverter : IValueConverter
    {
        private static ToLowerConverter instance;

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static ToLowerConverter() { }

        private ToLowerConverter() { }

        public static ToLowerConverter Instance
        {
            get { return instance ?? (instance = new ToLowerConverter()); }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as string)?.ToLower();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
