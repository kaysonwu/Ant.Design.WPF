using System.ComponentModel;
using System.Windows;

namespace Antd.Helpers
{
    /// <summary>
    /// A helper class that provides various controls.
    /// </summary>
    public static class ControlHelper
    {
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.RegisterAttached("CornerRadius", typeof(CornerRadius), typeof(ControlHelper), new FrameworkPropertyMetadata(new CornerRadius(),
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

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

    }
}
