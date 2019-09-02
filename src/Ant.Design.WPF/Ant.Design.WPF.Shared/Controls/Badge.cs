namespace Antd.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using WindowSize = System.Windows.Size;
    using ContentControlBase = System.Windows.Controls.ContentControl;

    [TemplatePart(Name = PART_BadgeContainer, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = PART_Count, Type = typeof(ContentPresenter))]
    public class Badge : ContentControlBase
    {
        #region Fields

        private const string PART_BadgeContainer = "PART_BadgeContainer";

        private const string PART_Count = "PART_Count";

        private FrameworkElement badgeContainer;

        private ContentPresenter count;

        #endregion

        #region Properties

        public static readonly DependencyProperty CountProperty =
            DependencyProperty.Register("Count", typeof(object), typeof(Badge), new PropertyMetadata(null, OnCountChanged));

        /// <summary>
        /// Gets/sets number to show in badge
        /// </summary>
        public object Count
        {
            get { return (object)GetValue(CountProperty); }
            set { SetValue(CountProperty, value); }
        }

        private static void OnCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Badge).ApplyCount();
        }

        private void ApplyCount()
        {
            if (count == null) return;

            var content = Count;

            if (Count is string)
            {
                try
                {
                    var d = int.Parse(Count as string);

                    if (d > OverflowCount)
                    {
                        content = OverflowCount + "+";
                    }
                }
                catch { } // Swallow the error, it may be normal
            }

            count.Content = content;
        }

        public static readonly DependencyProperty DotProperty =
            DependencyProperty.Register("Dot", typeof(bool), typeof(Badge), new PropertyMetadata(false));

        /// <summary>
        /// Gets/sets whether to display a red dot instead of count
        /// </summary>
        public bool Dot
        {
            get { return (bool)GetValue(DotProperty); }
            set { SetValue(DotProperty, value); }
        }

        public static readonly DependencyProperty OffsetProperty =
            DependencyProperty.Register("Offset", typeof(Point?), typeof(Badge), new PropertyMetadata(null));

        public Point? Offset
        {
            get { return (Point?)GetValue(OffsetProperty); }
            set { SetValue(OffsetProperty, value); }
        }

        public static readonly DependencyProperty OverflowCountProperty =
            DependencyProperty.Register("OverflowCount", typeof(int), typeof(Badge), new PropertyMetadata(99, OnCountChanged));

        /// <summary>
        /// Gets/sets max count to show
        /// </summary>
        public int OverflowCount
        {
            get { return (int)GetValue(OverflowCountProperty); }
            set { SetValue(OverflowCountProperty, value); }
        }

        public static readonly DependencyProperty ShowZeroProperty =
            DependencyProperty.Register("ShowZero", typeof(bool), typeof(Badge), new PropertyMetadata(false));

        /// <summary>
        /// Gets/sets whether to show badge when count is zero
        /// </summary>
        public bool ShowZero
        {
            get { return (bool)GetValue(ShowZeroProperty); }
            set { SetValue(ShowZeroProperty, value); }
        }

        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register("Status", typeof(BadgeStatus?), typeof(Badge), new PropertyMetadata(null));

        /// <summary>
        /// Gets/sets badge as a status dot
        /// </summary>
        public BadgeStatus? Status
        {
            get { return (BadgeStatus?)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(Badge), new PropertyMetadata(string.Empty));

        /// <summary>
        /// Gets/sets the text of the status dot. valid with StatusProperty set
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty BadgeHeightProperty =
            DependencyProperty.Register("BadgeHeight", typeof(double), typeof(Badge), new PropertyMetadata(default(double)));

        public double BadgeHeight
        {
            get { return (double)GetValue(BadgeHeightProperty); }
            set { SetValue(BadgeHeightProperty, value); }
        }

        public static readonly DependencyProperty BadgeForegroundProperty =
            DependencyProperty.Register("BadgeForeground", typeof(Brush), typeof(Badge), new PropertyMetadata(default(Brush)));

        public Brush BadgeForeground
        {
            get { return (Brush)GetValue(BadgeForegroundProperty); }
            set { SetValue(BadgeForegroundProperty, value); }
        }


        public static readonly DependencyProperty BadgeBackgroundProperty =
            DependencyProperty.Register("BadgeBackground", typeof(Brush), typeof(Badge), new PropertyMetadata(default(Brush)));

        public Brush BadgeBackground
        {
            get { return (Brush)GetValue(BadgeBackgroundProperty); }
            set { SetValue(BadgeBackgroundProperty, value); }
        }


        #endregion

        #region Constructors

        static Badge()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Badge), new FrameworkPropertyMetadata(typeof(Badge)));  
        }

        #endregion

        #region Overrides

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            badgeContainer = GetTemplateChild(PART_BadgeContainer) as FrameworkElement;
            count = GetTemplateChild(PART_Count) as ContentPresenter;

            ApplyCount();
        }

        protected override WindowSize ArrangeOverride(WindowSize arrangeBounds)
        {
            var result = base.ArrangeOverride(arrangeBounds);

            if (badgeContainer == null) return result;

            var desiredSize = badgeContainer.DesiredSize;

            //   System.Console.WriteLine(desiredSize);
            //    if ((desiredSize.Width <= 0.0 || desiredSize.Height <= 0.0))


            //var containerDesiredSize = _badgeContainer.DesiredSize;
            //if ((containerDesiredSize.Width <= 0.0 || containerDesiredSize.Height <= 0.0)
            //    && !double.IsNaN(_badgeContainer.ActualWidth) && !double.IsInfinity(_badgeContainer.ActualWidth)
            //    && !double.IsNaN(_badgeContainer.ActualHeight) && !double.IsInfinity(_badgeContainer.ActualHeight))
            //{
            //    containerDesiredSize = new Size(_badgeContainer.ActualWidth, _badgeContainer.ActualHeight);
            //}

            var h = 0 - desiredSize.Width / 2;
            var v = 0 - desiredSize.Height / 2;

          //  badgeContainer.Margin = new Thickness(0);
          //  badgeContainer.Margin = new Thickness(h, v, h, v);

            return result;
        }

        #endregion
    }

    public enum BadgeStatus : byte
    {
        Success, Processing, Default, Error, Warning
    }
}
