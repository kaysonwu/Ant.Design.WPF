using System;
using System.Collections.Generic;
using System.Text;
using ControlzEx.Behaviors;
using Antd.Controls;
using System.Windows;

namespace Antd.Behaviors
{
    public class BorderlessWindowBehavior : WindowChromeBehavior
    {
        protected override void OnAttached()
        {
            this.GlassFrameThickness = new Thickness(0, 0, 0, 0.1);
            Console.WriteLine(this.ResizeBorderThickness.ToString());
          
            base.OnAttached();
        }

        protected override void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            var window = sender as Antd.Controls.Window;
            if (window == null)
            {
                return;
            }

            if (window.ResizeMode != ResizeMode.NoResize)
            {
                //window.SetIsHitTestVisibleInChromeProperty<Border>("PART_Border");
               // window.SetIsHitTestVisibleInChromeProperty<UIElement>("PART_Icon");
               // window.SetWindowChromeResizeGripDirection("WindowResizeGrip", ResizeGripDirection.BottomRight);
            }
        }
    }
}
