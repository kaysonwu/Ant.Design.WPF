using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using ControlBase = System.Windows.Controls.Control;

namespace Antd.Controls
{
    /// <summary>
    /// Avatars can be used to represent people or objects. It supports images, Icons, or letters.
    /// </summary>
    [ContentProperty("Text")]
    [TemplatePart(Name = "PART_Content", Type = typeof(ContentPresenter))]
    public class Avatar : ControlBase
    {
        #region Fields

        private const string PART_Content = "PART_Content";

        private ContentPresenter contentPresenter;

        #endregion

        #region Properties

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(string), typeof(Avatar), new PropertyMetadata(null, OnContentChanged));

        /// <summary>
        /// Gets/sets the icon type for an icon avatar.
        /// </summary>
        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Avatar).SetContent(true);
        }

        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register("Size", typeof(Size?), typeof(Avatar), new PropertyMetadata(null));

        /// <summary>
        /// Gets/sets the size of the avatar.
        /// </summary>
        public Size? Size
        {
            get { return (Size?)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(ImageSource), typeof(Avatar), new PropertyMetadata(null, OnContentChanged));

        /// <summary>
        /// Gets/sets the ImageSource for an image avatar.
        /// </summary>
        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(Avatar), new PropertyMetadata(string.Empty, OnContentChanged));

        /// <summary>
        /// Gets/sets the text for an text avatar.
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty AlternativeProperty =
            DependencyProperty.Register("Alternative", typeof(string), typeof(Avatar), new PropertyMetadata(string.Empty));

        /// <summary>
        /// This attribute defines the alternative text describing the image.
        /// </summary>
        public string Alternative
        {
            get { return (string)GetValue(AlternativeProperty); }
            set { SetValue(AlternativeProperty, value); }
        }

        #endregion

        #region Constructors

        static Avatar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Avatar), new FrameworkPropertyMetadata(typeof(Avatar)));
        }

        #endregion

        #region Overrides

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            contentPresenter = GetTemplateChild(PART_Content) as ContentPresenter;
            SetContent(true);
        }

        #endregion

        #region Private Methods

        private void SetContent(bool imageExist)
        {
            if (contentPresenter == null) return;

            var content = contentPresenter.Content;

            // Clear Event
            if (content is Image)
            {
                ClearValue(BackgroundProperty);
                ((Image)content).ImageFailed -= OnImageFailed;
            }
            else if (content is TextBlock)
            {
                ((TextBlock)content).SizeChanged -= OnTextSizeChanged;
            }

            if (Source != null && imageExist)
            {
                if (!(content is Image))
                {
                    content = new Image();
                }

                SetCurrentValue(BackgroundProperty, Brushes.Transparent);

                var image = (Image)content;
                image.Source = Source;

                image.ImageFailed += OnImageFailed;
                RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.HighQuality);

            }
            else if (Icon != null)
            {
                if (!(content is Icon))
                {
                    content = new Icon();
                }

              ((Icon)content).Type = Icon;

            }
            else
            {
                var text = string.IsNullOrEmpty(Text) ? (imageExist ? string.Empty : Alternative) : Text;

                if (!(content is TextBlock))
                {
                    content = new TextBlock();
                }

                var textblock = (TextBlock)content;
                textblock.Text = text;

                textblock.RenderTransformOrigin = new Point(0.5, 0.5);
                textblock.SizeChanged += OnTextSizeChanged;
            }

            // 引用传递对 Null 无效
            contentPresenter.Content = content;
        }

        private void OnImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            SetContent(false);
        }

        /// <summary>
        /// Autoset Font Size
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTextSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var children = sender as FrameworkElement;
            var childrenWidth = children.ActualWidth;

            var width = ActualWidth - 8;
            var scale = 1d;
            var left = 0d;

            if (width < childrenWidth)
            {
                scale = width / childrenWidth;
                left = ActualWidth / 2 - childrenWidth / 2;
            }

            children.Margin = new Thickness(left, 0d, left, 0d);
            children.RenderTransform = new ScaleTransform(scale, scale);
        }

        #endregion
    }
}
