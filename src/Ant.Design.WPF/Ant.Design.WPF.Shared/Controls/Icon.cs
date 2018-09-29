using System.Windows;
using System.Windows.Controls;

namespace Antd.Controls
{
    [TemplatePart(Name = "PART_Content", Type = typeof(ContentPresenter))]
    public class Icon : Control
    {
        private ContentPresenter contentPresenter;

        #region Properties

        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(string), typeof(Icon), new PropertyMetadata(null, OnTypeChanged));

        /// <summary>
        /// Gets/sets the type of Icon
        /// </summary>
        public string Type
        {
            get { return (string)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        private static void OnTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Icon).ApplyContent();
        }

        private void ApplyContent()
        {
            if(contentPresenter != null)
            {
                contentPresenter.Content = GetContent();
            }

            DoSpin();
        }

        private object GetContent()
        {
            if (string.IsNullOrEmpty(Type))
            {
                return Type;
            }

            var prefix = TryFindResource("IconFontPrefix") as string;

            if (!string.IsNullOrEmpty(prefix))
            {
                prefix += "-";
            }

            return TryFindResource(prefix + Type) ?? Type;
        }

        public static readonly DependencyProperty SpinProperty =
            DependencyProperty.Register("Spin", typeof(bool?), typeof(Icon), new PropertyMetadata(null, OnSpinChanged));

        /// <summary>
        /// Gets/sets whether the icon has a spin animation
        /// </summary>
        public bool? Spin
        {
            get { return (bool?)GetValue(SpinProperty); }
            set { SetValue(SpinProperty, value); }
        }

        private static void OnSpinChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Icon).DoSpin();
        }

        private void DoSpin()
        {
            if (Spin == true || (Spin == null && Type == "loading"))
            {
                VisualStateManager.GoToState(this, "Spun", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "Unspun", true);
            }
        }

        #endregion

        #region Constructors

        static Icon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Icon), new FrameworkPropertyMetadata(typeof(Icon)));
        }

        #endregion

        #region Overrides

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            contentPresenter = GetTemplateChild("PART_Content") as ContentPresenter;
            ApplyContent();
        }

        #endregion
    }
}
