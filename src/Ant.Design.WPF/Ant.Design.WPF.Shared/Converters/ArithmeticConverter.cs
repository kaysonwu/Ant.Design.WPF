namespace Antd.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    public abstract class ArithmeticConverter : IValueConverter
    {
        public abstract double Convert(double value, Type targetType, double parameter, CultureInfo culture);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var val = (double)value;

                if (double.IsNaN(val))
                {
                    return value;
                }

                return Convert(val, targetType, double.Parse(parameter as string), culture);

            } catch { }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    public class AdditionConverter : ArithmeticConverter
    {
        public override double Convert(double value, Type targetType, double parameter, CultureInfo culture)
        {
            return value + parameter;
        }
    }

    public class SubtractionConverter : ArithmeticConverter
    {
        public override double Convert(double value, Type targetType, double parameter, CultureInfo culture)
        {
            return value - parameter;
        }
    }

    public class DultiplicationConverter : ArithmeticConverter
    {
        public override double Convert(double value, Type targetType, double parameter, CultureInfo culture)
        {
            return value * parameter;
        }
    }

    public class DivisionConverter : ArithmeticConverter
    {
        public override double Convert(double value, Type targetType, double parameter, CultureInfo culture)
        {
            // Divisor cannot be 0
            if (parameter == 0.0)
            {
                return value;
            }

            return value / parameter;
        }
    }
}
