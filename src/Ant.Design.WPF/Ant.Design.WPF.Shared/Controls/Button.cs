using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using ButtonBase = System.Windows.Controls.Button;

namespace Antd.Controls
{

    [TemplatePart(Name = PART_Border, Type = typeof(FrameworkElement))]
    public class Button : ButtonBase
    {
        #region Fields

        private const string PART_Border = "PART_Border";

        private VisualState mouseOverState;

        private VisualState pressedState;

        private VisualState focusedState;

        // Border control type
        private BorderType? borderType;

        #endregion

        #region Properties

        public static readonly DependencyProperty HrefProperty =
            DependencyProperty.Register("Href", typeof(string), typeof(Button), new PropertyMetadata(null));

        /// <summary>
        /// Gets/sets click the button to jump to the url
        /// </summary>
        public string Href
        {
            get { return (string)GetValue(HrefProperty); }
            set { SetValue(HrefProperty, value); }
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(string), typeof(Button), new PropertyMetadata(null));

        /// <summary>
        /// Gets/sets the icon type of the button
        /// </summary>
        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty LoadingProperty =
            DependencyProperty.Register("Loading", typeof(bool), typeof(Button), new PropertyMetadata(false));

        /// <summary>
        /// Gets/sets the loading state of the button
        /// </summary>
        public bool Loading
        {
            get { return (bool)GetValue(LoadingProperty); }
            set { SetValue(LoadingProperty, value); }
        }

        public static readonly DependencyProperty CircularProperty =
            DependencyProperty.Register("Circular", typeof(bool), typeof(Button), new PropertyMetadata(false));

        /// <summary>
        /// Gets/sets the shape of the button to be circular. Used to replace the shape attribute in ant design
        /// </summary>
        public bool Circular
        {
            get { return (bool)GetValue(CircularProperty); }
            set { SetValue(CircularProperty, value); }
        }

        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register("Size", typeof(Size?), typeof(Button), new PropertyMetadata(null));

        /// <summary>
        /// Gets/sets the size of the button
        /// </summary>
        public Size? Size
        {
            get { return (Size?)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        public static readonly DependencyProperty TypeProperty = 
            DependencyProperty.Register("Type", typeof(ButtonType?), typeof(Button), new PropertyMetadata(null, OnBrushChanged));


        /// <summary>
        /// Gets/sets button type
        /// </summary>
        public ButtonType? Type
        {
            get { return (ButtonType?)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        public static readonly DependencyProperty EffectBrushProperty = DependencyProperty.Register(
            "EffectBrush", 
            typeof(Brush), 
            typeof(Button), 
            new FrameworkPropertyMetadata(
                Brushes.Transparent,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits,
                OnBrushChanged));

        /// <summary>
        /// Gets/sets the border effect brush of the button
        /// </summary>
        public Brush EffectBrush
        {
            get { return (Brush)GetValue(EffectBrushProperty); }
            set { SetValue(EffectBrushProperty, value); }
        }

        private static void OnBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Button).ApplyVisualState(e.Property);
        }

        #endregion

        #region Constructors

        static Button()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Button), new FrameworkPropertyMetadata(typeof(Button)));
            BackgroundProperty.OverrideMetadata(typeof(Button), new FrameworkPropertyMetadata(OnBrushChanged));
            ForegroundProperty.OverrideMetadata(typeof(Button), new FrameworkPropertyMetadata(OnBrushChanged));
        }

        #endregion

        #region Overrides

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var border = GetTemplateChild(PART_Border);
            borderType = border is Border ? BorderType.Border : (border is Shape ? BorderType.Shape : (BorderType?)null);

            mouseOverState = GetTemplateChild("MouseOver") as VisualState;
            focusedState = GetTemplateChild("Focused") as VisualState;

            pressedState = GetTemplateChild("Pressed") as VisualState;
            ApplyVisualState(TypeProperty);
        }

        private void ApplyVisualState(DependencyProperty property)
        {
            // No initialization or no need for me to handle
            if (!borderType.HasValue || mouseOverState == null && focusedState == null && pressedState == null) return;

            Color color;
            Func<Color, int, VisualStateType, BorderType, Storyboard> func;

            // If the primary color does not change, it will not be processed.
            if (!Type.HasValue || Type.Value == ButtonType.Dashed)
            {
                if (property != TypeProperty && property != EffectBrushProperty) return;

                func = CreateDefaultStoryboard;
                color = (EffectBrush as SolidColorBrush).Color;
            }
            else if (Type.Value == ButtonType.Primary)
            {
                if (property != TypeProperty && property != BackgroundProperty) return;

                func  = CreatePrimaryStoryboard;
                color = (Background as SolidColorBrush).Color;
            }
            else
            {
                // Danger
                if (property != TypeProperty && property != ForegroundProperty) return;

                func  = CreateDangerStoryboard;
                color = (Foreground as SolidColorBrush).Color;
            }

            if (mouseOverState != null)
            {
                mouseOverState.Storyboard = func(color, 5, VisualStateType.MouseOver, borderType.Value);
            }

            if (focusedState != null)
            {
                focusedState.Storyboard = func(color, 5, VisualStateType.Focused, borderType.Value);
            }

            if (pressedState != null)
            {
                pressedState.Storyboard = func(color, 7, VisualStateType.Pressed, borderType.Value);
            }
        }

        private static Storyboard CreateDefaultStoryboard(Color color, int index, VisualStateType visualStateType, BorderType borderType)
        {
            var storyboard = new Storyboard();
            var children = storyboard.Children;

            color = ColorPalette.Toning(color, index);
            children.Add(CreateForegroundAnimation(color, visualStateType));
            children.Add(CreateBorderAnimation(color, visualStateType, borderType));

            return storyboard;
        }

        private static Storyboard CreatePrimaryStoryboard(Color color, int index, VisualStateType visualStateType, BorderType borderType)
        {
            var storyboard = new Storyboard();
            var children   = storyboard.Children;

            color = ColorPalette.Toning(color, index);
            children.Add(CreateBackgroundAnimation(color, visualStateType, borderType));
            children.Add(CreateBorderAnimation(color, visualStateType, borderType));

            return storyboard;
        }

        private static Storyboard CreateDangerStoryboard(Color color, int index, VisualStateType visualStateType, BorderType borderType)
        {
            var storyboard = new Storyboard();
            var children   = storyboard.Children;

            Color foreground, background;
            color = ColorPalette.Toning(color, index);

            if (visualStateType == VisualStateType.Focused)
            {
                foreground = color;
                background = Colors.White;
            } else
            {
                foreground = Colors.White;
                background = color;
            }

            children.Add(CreateForegroundAnimation(foreground, visualStateType));
            children.Add(CreateBackgroundAnimation(background, visualStateType, borderType));
            children.Add(CreateBorderAnimation(color, visualStateType, borderType));

            return storyboard;
        }

        private static Timeline CreateForegroundAnimation(Color color, VisualStateType visualStateType)
        {
            return CreateColorAnimation("(Control.Foreground).(SolidColorBrush.Color)", color, visualStateType, null);
        }

        private static Timeline CreateBackgroundAnimation(Color color, VisualStateType visualStateType, BorderType borderType, string name = PART_Border)
        {
            return CreateColorAnimation((borderType == BorderType.Shape ? "Fill" : "Background") + ".Color", color, visualStateType, name);
        }

        private static Timeline CreateBorderAnimation(Color color, VisualStateType visualStateType, BorderType borderType, string name = PART_Border)
        {
            return CreateColorAnimation((borderType == BorderType.Shape ? "Stroke" : "BorderBrush") + ".Color", color, visualStateType, name);
        }

        private static Timeline CreateColorAnimation(string path, Color color, VisualStateType visualStateType, string name)
        {
            var animation = new ColorAnimation() { To = color };

            if (visualStateType == VisualStateType.Pressed)
            {
                animation.Duration = TimeSpan.FromSeconds(0);
            }

            if (!string.IsNullOrEmpty(name))
            {
                Storyboard.SetTargetName(animation, name);
            }
            
            Storyboard.SetTargetProperty(animation, new PropertyPath(path));

            return animation;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (Loading) return;
            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            if (Loading)
            {
                VisualStateManager.GoToState(this, "Normal", true);
            } else
            {
                base.OnMouseEnter(e);
            }
        }

        protected override void OnClick()
        {
            // Preventing events in ClickMode.Press mode
            if (Loading) return;
            base.OnClick();
        }

        #endregion
    }

    public enum ButtonType : byte
    {
        Primary, Dashed, Danger
    }
}
