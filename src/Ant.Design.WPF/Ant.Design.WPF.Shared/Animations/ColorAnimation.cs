using System.Windows;
using System.Windows.Media;
using ColorAnimationBase = System.Windows.Media.Animation.ColorAnimation;

namespace Antd.Animations
{
    /// </summary>
    public class ColorAnimation : ColorAnimationBase
    {
        public static readonly DependencyProperty ToExProperty = DependencyProperty.Register(
            "ToEx", 
            typeof(SolidColorBrush), 
            typeof(ColorAnimation), 
            new FrameworkPropertyMetadata(
                Brushes.Transparent,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits, 
                OnToExChanged
            ));

        public SolidColorBrush ToEx
        {
            get { return (SolidColorBrush)GetValue(ToExProperty); }
            set { SetValue(ToExProperty, value); }
        }

        private static void OnToExChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var brush = e.NewValue as SolidColorBrush;
            ((ColorAnimation)d).To = brush.Color;
        }
    }
}
