using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Antd.Converters
{
    public class BackgroundToForegroundConverter : IValueConverter, IMultiValueConverter
    {
        private static BackgroundToForegroundConverter instance;

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static BackgroundToForegroundConverter() {}

        private BackgroundToForegroundConverter() {}

        public static BackgroundToForegroundConverter Instance
        {
            get { return instance ?? (instance = new BackgroundToForegroundConverter()); }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SolidColorBrush)
            {
                var brush = new SolidColorBrush(GetIdealColor(((SolidColorBrush)value).Color));
                brush.Freeze();
                return brush;
            }

            return Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var bgBrush = values.Length > 0 ? values[0] as Brush : null;
            var titleBrush = values.Length > 1 ? values[1] as Brush : null;

            if (titleBrush != null)
            {
                return titleBrush;
            }

            return Convert(bgBrush, targetType, parameter, culture);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return targetTypes.Select(t => DependencyProperty.UnsetValue).ToArray();
        }

        /// <summary>
        /// Determining Ideal Text Color Based on Specified Background Color
        /// http://www.codeproject.com/KB/GDI-plus/IdealTextColor.aspx
        /// </summary>
        /// <param name="color">background color</param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        private Color GetIdealColor(Color color,int threshold = 86)
        {
            var delta = System.Convert.ToInt32((color.R * 0.299) + (color.G * 0.587) + (color.B * 0.114));
            return  (255 - delta < threshold) ? Colors.Black : Colors.White;
        }
    }
}
