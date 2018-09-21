using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Antd.Helpers
{
    /// <summary>
    /// A helper class that provides various controls.
    /// </summary>
    public static class ControlHelper
    {
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached(
            "CornerRadius", 
            typeof(CornerRadius), 
            typeof(ControlHelper), 
            new FrameworkPropertyMetadata(
                new CornerRadius(), 
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender
            ));

        /// <summary> 
        /// The CornerRadius property allows users to control the roundness of the button corners independently by 
        /// setting a radius value for each corner. Radius values that are too large are scaled so that they
        /// smoothly blend from corner to corner. (Can be used e.g. at MetroButton style)
        /// Description taken from original Microsoft description :-D
        /// </summary>
        [Category(DesignerConstants.LibraryName)]
        public static CornerRadius GetCornerRadius(DependencyObject obj)
        {
            return (CornerRadius)obj.GetValue(CornerRadiusProperty);
        }

        public static void SetCornerRadius(DependencyObject obj, CornerRadius value)
        {
            obj.SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty MouseOverBorderBrushProperty = DependencyProperty.RegisterAttached(
            "MouseOverBorderBrush", 
            typeof(Brush), 
            typeof(ControlHelper), 
            new FrameworkPropertyMetadata(
                Brushes.Transparent, 
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        /// Gets the brush used to draw the focus border.
        /// </summary>
        [Category(DesignerConstants.LibraryName)]
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        [AttachedPropertyBrowsableForType(typeof(CheckBox))]
        [AttachedPropertyBrowsableForType(typeof(RadioButton))]
        [AttachedPropertyBrowsableForType(typeof(DatePicker))]
        [AttachedPropertyBrowsableForType(typeof(ComboBox))]
        public static Brush GetMouseOverBorderBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(MouseOverBorderBrushProperty);
        }

        /// <summary>
        /// Sets the brush used to draw the mouse over brush.
        /// </summary>
        public static void SetMouseOverBorderBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(MouseOverBorderBrushProperty, value);
        }

    }
}
