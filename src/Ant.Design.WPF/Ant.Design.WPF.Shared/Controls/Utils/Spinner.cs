using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Antd.Controls
{
    /// <summary>
    /// Provides the ability to spin for controls.
    /// Known defects:
    /// Using trigger to change control RenderTransform property will result in animation lose. 
    /// Try adding the Spin property Setter after the RenderTransform property Setter for notification purposes.
    /// Example: Theme/Switch.xaml
    /// </summary>
    public static class Spinner
    {
        private const string storyBoardName = "Antd.SpinnerStoryBoard";

        /// <summary>
        /// Start the spinning animation.
        /// </summary>
        public static void BeginSpin<T>(this T control, double seconds) where T : FrameworkElement, ISpinable
        {
            var transform = control.RenderTransform;
            control.SetCurrentValue(UIElement.RenderTransformOriginProperty, new Point(0.5, 0.5));
            TransformGroup transformGroup;

            if (transform is TransformGroup)
            {
                if (!(((TransformGroup)transform).Children.FirstOrDefault() is RotateTransform))
                {
                    transformGroup = (TransformGroup)transform.Clone();
                    transformGroup.Children.Insert(0, new RotateTransform(0.0));
                    control.SetCurrentValue(UIElement.RenderTransformProperty, transformGroup);
                }
            }
            else
            {
                transformGroup = new TransformGroup();

                if (transform is RotateTransform)
                {
                    transformGroup.Children.Add(transform);
                }
                else
                {
                    transformGroup.Children.Add(new RotateTransform(0.0));

                    if (transform != null && transform != Transform.Identity)
                    {
                        transformGroup.Children.Add(transform);
                    }
                }

                control.SetCurrentValue(UIElement.RenderTransformProperty, transformGroup);
            }

            if (!(control.Resources[storyBoardName] is Storyboard storyboard))
            {
                storyboard = new Storyboard();
                var animation = new DoubleAnimation
                {
                    From = 0,
                    To = 360,
                    AutoReverse = false,
                    Duration = TimeSpan.FromSeconds(seconds),
                    RepeatBehavior = RepeatBehavior.Forever
                };

                Storyboard.SetTarget(animation, control);
                Storyboard.SetTargetProperty(animation,
                    new PropertyPath("(0).(1)[0].(2)", UIElement.RenderTransformProperty,
                        TransformGroup.ChildrenProperty, RotateTransform.AngleProperty));

                storyboard.Children.Add(animation);
                control.Resources.Add(storyBoardName, storyboard);
            }

            storyboard.Begin();
        }

        /// <summary>
        /// Stop the spinning animation.
        /// </summary>
        public static void StopSpin<T>(this T control) where T : FrameworkElement, ISpinable
        {
            (control.Resources[storyBoardName] as Storyboard)?.Stop();
        }
    }
}
