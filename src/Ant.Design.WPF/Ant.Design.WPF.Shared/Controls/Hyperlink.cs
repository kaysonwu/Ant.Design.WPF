namespace Antd.Controls
{
    using System.Windows;
    using System.Windows.Controls.Primitives;

    /// <summary>
    /// A hyperlink button.
    /// </summary>
    public class Hyperlink : ButtonBase
    {

        #region Properties

        public static readonly DependencyProperty UriProperty =
            DependencyProperty.Register("Uri", typeof(string), typeof(Hyperlink), new PropertyMetadata(string.Empty));

        /// <summary>
        /// Gets or sets the uri of hyperlinks.
        /// </summary>
        public string Uri
        {
            get { return (string)GetValue(UriProperty); }
            set { SetValue(UriProperty, value); }
        }

        #endregion

        #region Constructors

        static Hyperlink()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Hyperlink), new FrameworkPropertyMetadata(typeof(Hyperlink)));
        }
        #endregion
    }
}  
