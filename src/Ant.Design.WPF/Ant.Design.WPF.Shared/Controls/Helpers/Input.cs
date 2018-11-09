using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Antd.Controls
{
    public static class Input
    {
        public static readonly DependencyProperty PlaceholderProperty = 
            DependencyProperty.RegisterAttached("Placeholder", typeof(string), typeof(Input), new UIPropertyMetadata(string.Empty));

        /// <summary>
        /// Get the placeholder for the input control.
        /// </summary>
        [AttachedPropertyBrowsableForType(typeof(TextBoxBase))]
        [AttachedPropertyBrowsableForType(typeof(PasswordBox))]
        [AttachedPropertyBrowsableForType(typeof(ComboBox))]
        public static string GetPlaceholder(DependencyObject obj)
        {
            return (string)obj.GetValue(PlaceholderProperty);
        }

        /// <summary>
        /// Set the placeholder for the input control.
        /// </summary>
        public static void SetPlaceholder(DependencyObject obj, string value)
        {
            obj.SetValue(PlaceholderProperty, value);
        }

        public static readonly DependencyProperty PlaceholderBrushProperty =
            DependencyProperty.RegisterAttached("PlaceholderBrush", typeof(Brush), typeof(Input), 
                new FrameworkPropertyMetadata(
                    Brushes.Silver,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender |
                    FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        /// Get the placeholder foreground brush for the input control.
        /// </summary>
        [AttachedPropertyBrowsableForType(typeof(TextBoxBase))]
        [AttachedPropertyBrowsableForType(typeof(PasswordBox))]
        [AttachedPropertyBrowsableForType(typeof(ComboBox))]
        public static Brush GetPlaceholderBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(PlaceholderBrushProperty);
        }

        /// <summary>
        /// Set the placeholder foreground brush for the input control.
        /// </summary>
        public static void SetPlaceholderBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(PlaceholderBrushProperty, value);
        }

        public static readonly DependencyProperty PrefixProperty = 
            DependencyProperty.RegisterAttached("Prefix", typeof(object), typeof(Input), new PropertyMetadata(null));

        /// <summary>
        /// Get the prefix object of the input control.
        /// </summary>
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        [AttachedPropertyBrowsableForType(typeof(PasswordBox))]
        public static object GetPrefix(DependencyObject obj)
        {
            return obj.GetValue(PrefixProperty);
        }

        /// <summary>
        /// Set the prefix object of the input control.
        /// </summary>
        public static void SetPrefix(DependencyObject obj, object value)
        {
            obj.SetValue(PrefixProperty, value);
        }

        public static readonly DependencyProperty SuffixProperty =
            DependencyProperty.RegisterAttached("Suffix", typeof(object), typeof(Input), new PropertyMetadata(null));

        /// <summary>
        /// Get the suffix object of the input control.
        /// </summary>
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        [AttachedPropertyBrowsableForType(typeof(PasswordBox))]
        public static object GetSuffix(DependencyObject obj)
        {
            return obj.GetValue(SuffixProperty);
        }

        /// <summary>
        /// Set the suffix object of the input control.
        /// </summary>
        public static void SetSuffix(DependencyObject obj, object value)
        {
            obj.SetValue(SuffixProperty, value);
        }

        public static readonly DependencyProperty ClearableProperty = 
            DependencyProperty.RegisterAttached("Clearable", typeof(bool), typeof(Input), new PropertyMetadata(false, OnClearableChanged));

        private static void OnClearableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
         //   (d as Button).GetAncestors();
         //   VisualTreeHelper.
        }

        /// <summary>
        /// 
        /// </summary>
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        [AttachedPropertyBrowsableForType(typeof(PasswordBox))]
        public static bool GetClearable(DependencyObject obj)
        {
            return (bool)obj.GetValue(ClearableProperty);
        }

        public static void SetClearable(DependencyObject obj, bool value)
        {
            obj.SetValue(ClearableProperty, value);
        }
    }
}
