using System.Windows;
using System.Windows.Controls;

namespace Antd.Controls
{
    /// <summary>
    /// A heading of the a page.
    /// </summary>
    public class Heading : TextBlock
    {
        #region Properties

        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register("Size", typeof(HeadingSizes), typeof(Heading), new PropertyMetadata(HeadingSizes.Normal));

        /// <summary>
        /// Gets/sets the size of the heading.
        /// </summary>
        public HeadingSizes Size
        {
            get { return (HeadingSizes)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        #endregion

        #region Constructors

        static Heading()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Heading), new FrameworkPropertyMetadata(typeof(Heading)));
        }

        #endregion
    }

    public enum HeadingSizes : byte
    {
       ExtraLarge, Large, Medium, Normal, Small, Tiny
    }
}
