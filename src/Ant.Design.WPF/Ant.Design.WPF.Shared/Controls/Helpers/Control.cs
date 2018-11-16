using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Antd.Controls
{
    /// <summary>
    /// A helper class that provides various controls.
    /// </summary>
    public static class Control
    {
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached(
            "CornerRadius", 
            typeof(CornerRadius), 
            typeof(Control), 
            new FrameworkPropertyMetadata(
                new CornerRadius(), 
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender
            ));

        /// <summary>
        /// Get a value that represents the degree to which the corners of a control border are rounded.
        /// </summary>
        [Category(DesignerConstants.LibraryName)]
        public static CornerRadius GetCornerRadius(DependencyObject obj)
        {
            return (CornerRadius)obj.GetValue(CornerRadiusProperty);
        }

        /// <summary>
        /// Set a value that represents the degree to which the corners of a control border are rounded.
        /// </summary>
        public static void SetCornerRadius(DependencyObject obj, CornerRadius value)
        {
            obj.SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty BorderStyleProperty = DependencyProperty.RegisterAttached(
            "BorderStyle", 
            typeof(BorderStyle), 
            typeof(Control), 
            new PropertyMetadata(BorderStyle.Solid));

        /// <summary>
        /// Get the style of the control border.
        /// </summary>
        public static BorderStyle GetBorderStyle(DependencyObject obj)
        {
            return (BorderStyle)obj.GetValue(BorderStyleProperty);
        }

        /// <summary>
        /// Set the style of the control border.
        /// </summary>
        public static void SetBorderStyle(DependencyObject obj, BorderStyle value)
        {
            obj.SetValue(BorderStyleProperty, value);
        }

        public static readonly DependencyProperty SizeProperty = 
            DependencyProperty.RegisterAttached("Size", typeof(Sizes?), typeof(Control), new PropertyMetadata(null));

        /// <summary>
        /// Get the size of the control.
        /// </summary>
        public static Sizes? GetSize(DependencyObject obj)
        {
            return (Sizes?)obj.GetValue(SizeProperty);
        }

        /// <summary>
        /// Set the size of the control.
        /// </summary>
        public static void SetSize(DependencyObject obj, Sizes? value)
        {
            obj.SetValue(SizeProperty, value);
        }

    }
}
