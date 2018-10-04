using System.Windows;
using ContentControlBase = System.Windows.Controls.ContentControl;

namespace Antd.Controls
{
    public class Badge : ContentControlBase
    {
        #region Properties

        public static readonly DependencyProperty CountProperty =
            DependencyProperty.Register("Count", typeof(int), typeof(Badge), new PropertyMetadata(0));

        /// <summary>
        /// Gets/sets number to show in badge
        /// </summary>
        public int Count
        {
            get { return (int)GetValue(CountProperty); }
            set { SetValue(CountProperty, value); }
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
            DependencyProperty.Register("OverflowCount", typeof(int), typeof(Badge), new PropertyMetadata(99));

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

        #endregion

        #region Constructors

        static Badge()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Badge), new FrameworkPropertyMetadata(typeof(Badge)));  
        }

        #endregion
    }

    public enum BadgeStatus : byte
    {
        Success, Processing, Default, Error, Warning
    }
}
