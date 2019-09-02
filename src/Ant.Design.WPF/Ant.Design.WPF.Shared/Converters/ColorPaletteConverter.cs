namespace Antd.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Media;

    public class ColorPaletteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!int.TryParse(parameter as string, out int index))
            {
                return value;
            }

            if (value is SolidColorBrush)
            {
               return new SolidColorBrush(ColorPalette.Toning((value as SolidColorBrush).Color, index));
            }

            return (value is Color) ? ColorPalette.Toning((Color)value, index) : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
