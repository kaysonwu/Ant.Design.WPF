using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media.Animation;
using ControlHelper = Antd.Controls.Control;

namespace Antd.Themes
{
    class Button
    {
        private ControlTemplate c()
        {
            var template = new  ControlTemplate(typeof(Antd.Controls.Button));
            var grid     = new FrameworkElementFactory(typeof(Grid));
           
            grid.SetValue(FrameworkElement.UseLayoutRoundingProperty, true);
            grid.AppendChild(CreateEffect());
            grid.AppendChild(CreateBorder());

        //    grid.SetValue(VisualStateManager.VisualStateGroupsProperty, );
            template.VisualTree = grid;
            return template;
        }

        protected FrameworkElementFactory CreateEffect()
        {
            var effect = new FrameworkElementFactory(typeof(Border), "Effect");

            effect.SetBinding(ControlHelper.CornerRadiusProperty, TemplateBinding(ControlHelper.CornerRadiusProperty));
            effect.SetBinding(UIElement.SnapsToDevicePixelsProperty, TemplateBinding(UIElement.SnapsToDevicePixelsProperty));

            effect.SetValue(UIElement.OpacityProperty, 0);
            effect.SetValue(Border.BorderThicknessProperty, 6);

            return effect;
        }

        protected FrameworkElementFactory CreateBorder()
        {
            var border = new FrameworkElementFactory(typeof(Border), "Border");
            border.SetBinding(ControlHelper.CornerRadiusProperty, TemplateBinding(ControlHelper.CornerRadiusProperty));

            border.SetBinding(Border.BackgroundProperty, TemplateBinding(Control.BackgroundProperty));
            border.SetBinding(Border.BorderBrushProperty, TemplateBinding(Control.BorderBrushProperty));

            border.SetBinding(Border.BorderThicknessProperty, TemplateBinding(Control.BorderThicknessProperty));
            border.SetBinding(UIElement.SnapsToDevicePixelsProperty, TemplateBinding(UIElement.SnapsToDevicePixelsProperty));

            return border;
        }

        private void CreateVisualState()
        {
            var common = new VisualStateGroup() { Name = "CommonStates" };
            var resources = Application.Current.Resources;

            common.Transitions.Add(
                new VisualTransition() {
                    To = "Pressed",
                    GeneratedDuration = TimeSpan.FromSeconds(0)
                }
            );
            common.Transitions.Add(
                new VisualTransition()
                {
                    GeneratedDuration = TimeSpan.FromSeconds(0.3),
                    GeneratedEasingFunction = resources["EaseInOut"] as IEasingFunction
                }
            );

            common.States.Add(new VisualState() { Name = "Normal" });
            //common.States.Add(new VisualState() { Name = "MouseOver", Storyboard =  });
        }

        private BindingBase TemplateBinding(DependencyProperty property)
        {
            return new Binding(property.Name) { RelativeSource = RelativeSource.TemplatedParent };
        }
    }
}
