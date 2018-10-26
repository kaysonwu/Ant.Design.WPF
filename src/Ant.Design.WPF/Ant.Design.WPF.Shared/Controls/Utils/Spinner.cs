using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Antd.Controls
{
    public static class Spinner
    {
        private const string storyBoardName = "Antd.SpinnerStoryBoard";

        /// <summary>
        /// Start the spinning animation. From https://github.com/charri/Font-Awesome-WPF/blob/master/src/WPF/FontAwesome.WPF/ControlExtensions.cs
        /// </summary>
        public static void BeginSpin<T>(this T control, Duration duration) where T : FrameworkElement, ISpinable
        {
            var transformGroup  = control.RenderTransform as TransformGroup ?? new TransformGroup();
            var rotateTransform = transformGroup.Children.OfType<RotateTransform>().FirstOrDefault();

            if (rotateTransform != null)
            {
                rotateTransform.Angle = 0;
            }
            else
            {
                transformGroup.Children.Add(new RotateTransform(0.0));
                control.RenderTransform = transformGroup;
                control.RenderTransformOrigin = new Point(0.5, 0.5);
            }

            if (!(control.Resources[storyBoardName] is Storyboard storyboard))
            {
                storyboard = new Storyboard();
                var animation = new DoubleAnimation
                {
                    From = 0,
                    To = 360,
                    AutoReverse = false,
                    Duration = duration,
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
