namespace Antd.Behaviors
{
    using System.Windows;
    using System.Windows.Interactivity;
    using System.Windows.Media.Animation;

    /// <summary>
    /// Provides the ability to animate when changing the Visibility property for an element.
    /// </summary>
    public class VisibilityBehavior : Behavior<FrameworkElement>
    {
        #region Fields

        private Visibility visibility;

        #endregion

        #region Properties

        public static readonly DependencyProperty AppearProperty =
            DependencyProperty.Register("Appear", typeof(Storyboard), typeof(VisibilityBehavior), new PropertyMetadata(null));

        /// <summary>
        /// Gets/sets the storyboard when the element is visible.
        /// </summary>
        public Storyboard Appear
        {
            get { return (Storyboard)GetValue(AppearProperty); }
            set { SetValue(AppearProperty, value); }
        }

        public static readonly DependencyProperty LeaveProperty =
            DependencyProperty.Register("Leave", typeof(Storyboard), typeof(VisibilityBehavior), new PropertyMetadata(null));

        /// <summary>
        /// Gets/sets the storyboard when the element is hidden.
        /// </summary>
        public Storyboard Leave
        {
            get { return (Storyboard)GetValue(LeaveProperty); }
            set { SetValue(LeaveProperty, value); }
        }

        #endregion

        #region Overrides

        protected override void OnAttached()
        {
            visibility = AssociatedObject.Visibility;
            AssociatedObject.IsVisibleChanged += OnVisibleChanged;

            base.OnAttached();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            AssociatedObject.IsVisibleChanged += OnVisibleChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.IsVisibleChanged -= OnVisibleChanged;

            base.OnDetaching();
        }

        #endregion

        #region Private Methods

        private void OnVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (visibility == AssociatedObject.Visibility) return;

            Storyboard storyboard;
            
            if (AssociatedObject.Visibility == Visibility.Visible)
            {
                if (Appear == null) return;

                storyboard = Appear;
                visibility = AssociatedObject.Visibility;

            } else
            {
                if (Leave == null) return;

                var cache = AssociatedObject.Visibility;
                AssociatedObject.SetCurrentValue(UIElement.VisibilityProperty, visibility = Visibility.Visible);

                storyboard = Leave.Clone();
                storyboard.Completed += (s, a) => AssociatedObject.SetCurrentValue(UIElement.VisibilityProperty, visibility = cache);
            }

            AssociatedObject.BeginStoryboard(storyboard);
        }

        #endregion
    }
}
