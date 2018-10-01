using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace AntdDemo.Converters
{
    public class StringToMarkDownConverter : IValueConverter
    {
        private static StringToMarkDownConverter instance;

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static StringToMarkDownConverter() { }

        private StringToMarkDownConverter() { }

        public static StringToMarkDownConverter Instance
        {
            get { return instance ?? (instance = new StringToMarkDownConverter()); }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str       = value as string;
            var textBlock = new TextBlock() { TextWrapping = TextWrapping.Wrap };

            if (!string.IsNullOrEmpty(str))
            {
                string pattern = @"`(.*?)`";
                int index      = 0;

                var inlines    = textBlock.Inlines;
                str            = str.Replace(@"\n", Environment.NewLine);

                var foreground = new SolidColorBrush(Color.FromRgb(31, 46, 59));
                var background = new SolidColorBrush(Color.FromRgb(242, 244, 245));

                foreach (Match match in Regex.Matches(str, pattern))
                {
                    inlines.Add(str.Substring(index, match.Index - index));
                    inlines.Add(new Run(match.Value.Replace('`',' ')) { Foreground = foreground, Background = background });
                    index = match.Index + match.Length;
                }

                inlines.Add(str.Substring(index));
            } 

            return textBlock;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
