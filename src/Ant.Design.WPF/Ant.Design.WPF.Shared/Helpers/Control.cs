using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Antd.Helpers
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

        public static readonly DependencyProperty ContentCharacterCasingProperty = DependencyProperty.RegisterAttached(
            "ContentCharacterCasing", 
            typeof(CharacterCasing), 
            typeof(Control), 
            new FrameworkPropertyMetadata(
                CharacterCasing.Normal, 
                FrameworkPropertyMetadataOptions.AffectsMeasure
            ),
            new ValidateValueCallback(value => CharacterCasing.Normal <= (CharacterCasing)value && (CharacterCasing)value <= CharacterCasing.Upper));

        /// <summary>
        /// Gets the character casing of the control
        /// </summary>
        [Category(DesignerConstants.LibraryName)]
        [AttachedPropertyBrowsableForType(typeof(ContentControl))]
        public static CharacterCasing GetContentCharacterCasing(DependencyObject obj)
        {
            return (CharacterCasing)obj.GetValue(ContentCharacterCasingProperty);
        }

        /// <summary>
        /// Sets the character casing of the control
        /// </summary>
        public static void SetContentCharacterCasing(DependencyObject obj, CharacterCasing value)
        {
            obj.SetValue(ContentCharacterCasingProperty, value);
        }

        public static readonly DependencyProperty MouseOverBorderBrushProperty = DependencyProperty.RegisterAttached(
            "MouseOverBorderBrush",
            typeof(Brush),
            typeof(Control),
            new FrameworkPropertyMetadata(
                Brushes.Transparent,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        /// Gets the brush used to draw the mouseover border.
        /// </summary>
        [Category(DesignerConstants.LibraryName)]
        [AttachedPropertyBrowsableForType(typeof(Button))]
        public static Brush GetMouseOverBorderBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(MouseOverBorderBrushProperty);
        }

        /// <summary>
        /// Sets the brush used to draw the mouseover brush.
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
                Brushes.Transparent,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        /// Gets the brush used to draw the mouseover background.
        /// </summary>
        [Category(DesignerConstants.LibraryName)]
        [AttachedPropertyBrowsableForType(typeof(Button))]
        public static Brush GetMouseOverBackground(DependencyObject obj)
        {
            return (Brush)obj.GetValue(MouseOverBackgroundProperty);
        }

        /// <summary>
        /// Sets the brush used to draw the mouseover brush.
        /// </summary>
        public static void SetMouseOverBackground(DependencyObject obj, Brush value)
        {
            obj.SetValue(MouseOverBackgroundProperty, value);
        }

        public static readonly DependencyProperty MouseOverForegroundProperty = DependencyProperty.RegisterAttached(
            "MouseOverForeground",
            typeof(Brush),
            typeof(Control),
            new FrameworkPropertyMetadata(
                Brushes.Transparent,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        /// Gets the brush used to draw the mouseover border.
        /// </summary>
        [Category(DesignerConstants.LibraryName)]
        [AttachedPropertyBrowsableForType(typeof(Button))]
        public static Brush GetMouseOverForeground(DependencyObject obj)
        {
            return (Brush)obj.GetValue(MouseOverForegroundProperty);
        }

        /// <summary>
        /// Sets the brush used to draw the mouseover brush.
        /// </summary>
        public static void SetMouseOverForeground(DependencyObject obj, Brush value)
        {
            obj.SetValue(MouseOverForegroundProperty, value);
        }
    }
}
