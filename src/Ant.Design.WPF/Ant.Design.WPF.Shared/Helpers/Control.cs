using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Antd.Controls;

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
    }
}
