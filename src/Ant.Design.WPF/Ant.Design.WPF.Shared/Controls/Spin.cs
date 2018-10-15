using System.Windows;
using ContentControlBase = System.Windows.Controls.ContentControl;

namespace Antd.Controls
{
    /// <summary>
    /// A spinner for displaying loading state of a page or a section.
    /// </summary>
    [TemplateVisualState(Name = "Spun", GroupName = "SpinStates")]
    [TemplateVisualState(Name = "Unspun", GroupName = "SpinStates")]
    public class Spin : ContentControlBase
    {
        #region Properties

        public static readonly DependencyProperty IndicatorProperty =
            DependencyProperty.Register("Indicator", typeof(UIElement), typeof(Spin), new PropertyMetadata(null, OnSpinningChanged));

        /// <summary>
        /// Gets/sets element of the spinning indicator
        /// </summary>
        public UIElement Indicator
        {
            get { return (UIElement)GetValue(IndicatorProperty); }
            set { SetValue(IndicatorProperty, value); }
        }

        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register("Size", typeof(Size?), typeof(Spin), new PropertyMetadata(null));

        /// <summary>
        /// Gets/sets size of Spin
        /// </summary>
        public Size? Size
        {
            get { return (Size?)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        public static readonly DependencyProperty SpinningProperty =
            DependencyProperty.Register("Spinning", typeof(bool), typeof(Spin), new PropertyMetadata(true, OnSpinningChanged));

        /// <summary>
        /// Gets/sets whether Spin is spinning
        /// </summary>
        public bool Spinning
        {
            get { return (bool)GetValue(SpinningProperty); }
            set { SetValue(SpinningProperty, value); }
        }

        private static void OnSpinningChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Spin).GoToSpinState();
        }

        private void GoToSpinState()
        {
            VisualStateManager.GoToState(this, Spinning && null == Indicator ? "Spun" : "Unspun", true);
        }

        public static readonly DependencyProperty TipProperty =
            DependencyProperty.Register("Tip", typeof(string), typeof(Spin), new PropertyMetadata(string.Empty));

        /// <summary>
        /// Gets/sets customize description content when Spin has children
        /// </summary>
        public string Tip
        {
            get { return (string)GetValue(TipProperty); }
            set { SetValue(TipProperty, value); }
        }

        #endregion

        #region Constructors

        static Spin()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Spin), new FrameworkPropertyMetadata(typeof(Spin)));
        }

        #endregion

        #region Overrides

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            GoToSpinState();
        }

        #endregion
    }
}
