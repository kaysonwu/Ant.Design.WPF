using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Antd.Converters
{
    class ResizeModeToVisibilityConverter : IValueConverter
    {
        private static ResizeModeToVisibilityConverter instance;

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static ResizeModeToVisibilityConverter() {}

        private ResizeModeToVisibilityConverter() {}

        public static ResizeModeToVisibilityConverter Instance
        {
            get { return instance ?? (instance = new ResizeModeToVisibilityConverter()); }
        }

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
