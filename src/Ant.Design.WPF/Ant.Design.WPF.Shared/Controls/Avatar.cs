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

        public static readonly DependencyProperty ShapeProperty =
            DependencyProperty.Register("Shape", typeof(Shapes), typeof(Avatar), new PropertyMetadata(Shapes.Circle));

        /// <summary>
        /// Gets/sets the shape of avatar.
        /// </summary>
        public Shapes Shape
        {
            get { return (Shapes)GetValue(ShapeProperty); }
            set { SetValue(ShapeProperty, value); }
        }

        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register("Size", typeof(Sizes?), typeof(Avatar), new PropertyMetadata(null, OnSizeChanged));

        /// <summary>
        /// Gets/sets the size of the avatar.
        /// </summary>
        public Sizes? Size
        {
            get { return (Sizes?)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        private static void OnSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var avatar = d as Avatar;
            var newValue = (Sizes?)e.NewValue;
            var oldValue = (Sizes?)e.OldValue;

            if (newValue.HasValue && newValue.Value >= 0)
            {
                var size = (double)newValue.Value;
                avatar.SetValue(WidthProperty, size);
                avatar.SetValue(HeightProperty, size);
                avatar.SetValue(FontSizeProperty, size / 2);
            }
            else if (oldValue.HasValue && oldValue.Value >= 0)
            {
                avatar.ClearValue(WidthProperty);
                avatar.ClearValue(HeightProperty);
                avatar.ClearValue(FontSizeProperty);
            }
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

        public static readonly DependencyProperty IsImageProperty =
            DependencyProperty.Register("IsImage", typeof(bool), typeof(Avatar), new PropertyMetadata(false));

        /// <summary>
        /// Get the current avatar type as an image.
        /// </summary>
        public bool IsImage
        {
            get { return (bool)GetValue(IsImageProperty); }
            private set { SetValue(IsImageProperty, value); }
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
                ClearValue(IsImageProperty);
                ((Image)content).ImageFailed -= OnImageFailed;
            }
            else if (content is TextBlock)
            {
                SizeChanged -= OnTextSizeChanged;
                ((TextBlock)content).SizeChanged -= OnTextSizeChanged;
            }

            if (Source != null && imageExist)
            {
                if (!(content is Image))
                {
                    content = new Image();
                }

                SetCurrentValue(IsImageProperty, true);

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

                SizeChanged += OnTextSizeChanged;
                textblock.SizeChanged += OnTextSizeChanged;

                textblock.Text = text;
                textblock.RenderTransformOrigin = new Point(0.5, 0.5);
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
            if (contentPresenter != null && contentPresenter.Content is TextBlock textBlock)
            {
                var childrenWidth = textBlock.ActualWidth;
                var width         = ActualWidth - 8;
                var scale         = 1d;
                var left          = 0d;

                if (width < childrenWidth)
                {
                    scale = width / childrenWidth;
                    left = ActualWidth / 2 - childrenWidth / 2;
                }

                textBlock.Margin = new Thickness(left, 0d, left, 0d);
                textBlock.RenderTransform = new ScaleTransform(scale, scale);
            }
        }

        #endregion
    }
}
