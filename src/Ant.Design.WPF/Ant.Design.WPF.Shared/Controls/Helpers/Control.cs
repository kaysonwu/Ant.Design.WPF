using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace Antd.Controls
{
    /// <summary>
    /// A helper class that provides various controls.
    /// </summary>
    public static class Control
    {
        #region Border

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached(
            "CornerRadius", 
            typeof(CornerRadius), 
            typeof(Control), 
            new FrameworkPropertyMetadata(
                default(CornerRadius), 
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender
            ));

        /// <summary>
        /// Gets a value that represents the degree to which the corners of a control border are rounded.
        /// </summary>
        [Category(DesignerConstants.LibraryName)]
        public static CornerRadius GetCornerRadius(DependencyObject obj)
        {
            return (CornerRadius)obj.GetValue(CornerRadiusProperty);
        }

        /// <summary>
        /// Sets a value that represents the degree to which the corners of a control border are rounded.
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
        /// Gets the style of the control border.
        /// </summary>
        public static BorderStyle GetBorderStyle(DependencyObject obj)
        {
            return (BorderStyle)obj.GetValue(BorderStyleProperty);
        }

        /// <summary>
        /// Sets the style of the control border.
        /// </summary>
        public static void SetBorderStyle(DependencyObject obj, BorderStyle value)
        {
            obj.SetValue(BorderStyleProperty, value);
        }

        #endregion

        #region Size

        public static readonly DependencyProperty SizeProperty = DependencyProperty.RegisterAttached(
            "Size", 
            typeof(Sizes?), 
            typeof(Control), 
            new PropertyMetadata(null));

        /// <summary>
        /// Gets the size of the control.
        /// </summary>
        public static Sizes? GetSize(DependencyObject obj)
        {
            return (Sizes?)obj.GetValue(SizeProperty);
        }

        /// <summary>
        /// Sets the size of the control.
        /// </summary>
        public static void SetSize(DependencyObject obj, Sizes? value)
        {
            obj.SetValue(SizeProperty, value);
        }

        #endregion

        #region Brushes

        public static readonly DependencyProperty MouseOverForegroundProperty = DependencyProperty.RegisterAttached(
            "MouseOverForeground", 
            typeof(Brush), 
            typeof(Control), 
            new FrameworkPropertyMetadata(
                Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits
            ));

        /// <summary>
        /// Gets the foreground for controls in mouse over.
        /// </summary>
        public static Brush GetMouseOverForeground(DependencyObject obj)
        {
            return (Brush)obj.GetValue(MouseOverForegroundProperty);
        }

        /// <summary>
        /// Gets the foreground for controls in mouse over.
        /// </summary>
        public static void SetMouseOverForeground(DependencyObject obj, Brush value)
        {
            obj.SetValue(MouseOverForegroundProperty, value);
        }

        public static readonly DependencyProperty MouseOverBorderBrushProperty = DependencyProperty.RegisterAttached(
            "MouseOverBorderBrush", 
            typeof(Brush), 
            typeof(Control),
            new FrameworkPropertyMetadata(
                Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits
            ));

        /// <summary>
        /// Gets the border brush for controls in mouse over.
        /// </summary>
        public static Brush GetMouseOverBorderBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(MouseOverBorderBrushProperty);
        }

        /// <summary>
        /// Sets the border brush for controls in mouse over.
        /// </summary>
        public static void SetMouseOverBorderBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(MouseOverBorderBrushProperty, value);
        }

        public static readonly DependencyProperty MouseOverBackgroundProperty = DependencyProperty.RegisterAttached(
            "MouseOverBackground", 
            typeof(Brush), 
            typeof(Control), 
            new FrameworkPropertyMetadata(
                Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits
            ));

        /// <summary>
        /// Gets the background brush for controls in mouse over.
        /// </summary>
        public static Brush GetMouseOverBackground(DependencyObject obj)
        {
            return (Brush)obj.GetValue(MouseOverBackgroundProperty);
        }

        /// <summary>
        /// Sets the background brush for controls in mouse over.
        /// </summary>
        public static void SetMouseOverBackground(DependencyObject obj, Brush value)
        {
            obj.SetValue(MouseOverBackgroundProperty, value);
        }

        public static readonly DependencyProperty FocusedForegroundProperty = DependencyProperty.RegisterAttached(
            "FocusedForeground",
            typeof(Brush),
            typeof(Control),
            new FrameworkPropertyMetadata(
                Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits
            ));

        /// <summary>
        /// Gets the foreground for controls in focused.
        /// </summary>
        public static Brush GetFocusedForeground(DependencyObject obj)
        {
            return (Brush)obj.GetValue(FocusedForegroundProperty);
        }

        /// <summary>
        /// Gets the foreground for controls in focused.
        /// </summary>
        public static void SetFocusedForeground(DependencyObject obj, Brush value)
        {
            obj.SetValue(FocusedForegroundProperty, value);
        }

        public static readonly DependencyProperty FocusedBorderBrushProperty = DependencyProperty.RegisterAttached(
            "FocusedBorderBrush",
            typeof(Brush),
            typeof(Control),
            new FrameworkPropertyMetadata(
                Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits
            ));

        /// <summary>
        /// Gets the border brush for controls in focused.
        /// </summary>
        public static Brush GetFocusedBorderBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(FocusedBorderBrushProperty);
        }

        /// <summary>
        /// Sets the border brush for controls in focused.
        /// </summary>
        public static void SetFocusedBorderBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(FocusedBorderBrushProperty, value);
        }

        public static readonly DependencyProperty FocusedBackgroundProperty = DependencyProperty.RegisterAttached(
            "FocusedBackground",
            typeof(Brush),
            typeof(Control),
            new FrameworkPropertyMetadata(
                Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits
            ));

        /// <summary>
        /// Gets the background brush for controls in focused.
        /// </summary>
        public static Brush GetFocusedBackground(DependencyObject obj)
        {
            return (Brush)obj.GetValue(FocusedBackgroundProperty);
        }

        /// <summary>
        /// Sets the background brush for controls in focused.
        /// </summary>
        public static void SetFocusedBackground(DependencyObject obj, Brush value)
        {
            obj.SetValue(FocusedBackgroundProperty, value);
        }

        public static readonly DependencyProperty PressedForegroundProperty = DependencyProperty.RegisterAttached(
            "PressedForeground",
            typeof(Brush),
            typeof(Control),
            new FrameworkPropertyMetadata(
                Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits
            ));

        /// <summary>
        /// Gets the foreground for controls in pressed.
        /// </summary>
        public static Brush GetPressedForeground(DependencyObject obj)
        {
            return (Brush)obj.GetValue(PressedForegroundProperty);
        }

        /// <summary>
        /// Gets the foreground for controls in pressed.
        /// </summary>
        public static void SetPressedForeground(DependencyObject obj, Brush value)
        {
            obj.SetValue(PressedForegroundProperty, value);
        }

        public static readonly DependencyProperty PressedBorderBrushProperty = DependencyProperty.RegisterAttached(
            "PressedBorderBrush",
            typeof(Brush),
            typeof(Control),
            new FrameworkPropertyMetadata(
                Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits
            ));

        /// <summary>
        /// Gets the border brush for controls in pressed.
        /// </summary>
        public static Brush GetPressedBorderBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(PressedBorderBrushProperty);
        }

        /// <summary>
        /// Sets the border brush for controls in pressed.
        /// </summary>
        public static void SetPressedBorderBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(PressedBorderBrushProperty, value);
        }

        public static readonly DependencyProperty PressedBackgroundProperty = DependencyProperty.RegisterAttached(
            "PressedBackground",
            typeof(Brush),
            typeof(Control),
            new FrameworkPropertyMetadata(
                Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits
            ));

        /// <summary>
        /// Gets the background brush for controls in pressed.
        /// </summary>
        public static Brush GetPressedBackground(DependencyObject obj)
        {
            return (Brush)obj.GetValue(PressedBackgroundProperty);
        }

        /// <summary>
        /// Sets the background brush for controls in pressed.
        /// </summary>
        public static void SetPressedBackground(DependencyObject obj, Brush value)
        {
            obj.SetValue(PressedBackgroundProperty, value);
        }

        #endregion

    }
}
