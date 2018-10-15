using System.Windows;
using System.Windows.Controls;

namespace Antd.Controls
{

    /// <summary>
    /// Semantic vector graphics.
    /// </summary>
    [TemplateVisualState(Name = "Spun", GroupName = "SpinStates")]
    [TemplateVisualState(Name = "Unspun", GroupName = "SpinStates")]
    [TemplatePart(Name = "PART_Content", Type = typeof(ContentPresenter))]
    public class Icon : Control
    {
        #region Fields

        private ContentPresenter contentPresenter;

        #endregion

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
            (d as Icon).SetContent();
        }

        private void SetContent()
        {
            if(contentPresenter != null)
            {
                contentPresenter.Content = GetContent();
            }

            GoToSpinState();
        }

        private object GetContent()
        {
            if (string.IsNullOrEmpty(Type))
            {
                return Type;
            }

            var key      = (TryFindResource("IconFontPrefix") as string) + Type;
            var resource = TryFindResource(key);

            if (resource != null)
            {
                return resource;
            }

            // Forgive your misrepresentation
            return TryFindResource(key.ToLower()) ?? Type;
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
            (d as Icon).GoToSpinState();
        }

        private void GoToSpinState()
        {
            var spun = Spin.HasValue ? Spin.Value : (!string.IsNullOrEmpty(Type) && Type.ToLower() == "loading");
            VisualStateManager.GoToState(this, spun ? "Spun" : "Unspun", true);
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
            SetContent();
        }

        #endregion
    }
}
