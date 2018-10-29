using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Media;
using ControlBase = System.Windows.Controls.Control;

namespace Antd.Controls
{
    /// <summary>
    /// Alert component for feedback.
    /// </summary>
    [ContentProperty("Message")]
    [TemplatePart(Name = PART_Close, Type = typeof(ButtonBase))]
    public class Alert : ControlBase
    {
        #region Fields

        private const string PART_Close = "PART_Close";

        private ButtonBase close;

        #endregion

        #region Events

        public static readonly RoutedEvent ClosingEvent =
            EventManager.RegisterRoutedEvent("Closing", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Alert));

        /// <summary>
        /// Occurs when closing the tag.
        /// </summary>
        public event RoutedEventHandler Closing
        {
            add { AddHandler(ClosingEvent, value); }
            remove { RemoveHandler(ClosingEvent, value); }
        }

        public static readonly RoutedEvent ClosedEvent =
            EventManager.RegisterRoutedEvent("Closed", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Alert));

        /// <summary>
        /// Occurs when a Tag is closed and is no longer visible.
        /// </summary>
        public event RoutedEventHandler Closed
        {
            add { AddHandler(ClosedEvent, value); }
            remove { RemoveHandler(ClosedEvent, value); }
        }

        #endregion

        #region Properties

        public static readonly DependencyProperty BannerProperty =
            DependencyProperty.Register("Banner", typeof(bool), typeof(Alert), new PropertyMetadata(false));

        /// <summary>
        /// Gets/sets whether to show as banner.
        /// </summary>
        public bool Banner
        {
            get { return (bool)GetValue(BannerProperty); }
            set { SetValue(BannerProperty, value); }
        }

        public static readonly DependencyProperty ClosableProperty =
            DependencyProperty.Register("Closable", typeof(bool?), typeof(Alert), new PropertyMetadata(null, OnClosableChanged));

        /// <summary>
        /// Gets/sets whether alert can be closed.
        /// </summary>
        public bool? Closable
        {
            get { return (bool?)GetValue(ClosableProperty); }
            set { SetValue(ClosableProperty, value); }
        }

        private static void OnClosableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Alert).SetCloseButtonVisibility();
        }

        private void SetCloseButtonVisibility()
        {
            if (close != null)
            {
                var visible      = Closable.HasValue ? Closable.Value : CloseText != null;
                close.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public static readonly DependencyProperty CloseTextProperty =
            DependencyProperty.Register("CloseText", typeof(object), typeof(Alert), new PropertyMetadata(null, OnCloseTextChanged));

        /// <summary>
        /// Gets/sets close text to show.
        /// </summary>
        public object CloseText
        {
            get { return GetValue(CloseTextProperty); }
            set { SetValue(CloseTextProperty, value); }
        }

        private static void OnCloseTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Alert).SetCloseButton();
        }

        private void SetCloseButton()
        {
            if (close != null)
            {
                close.Content = CloseText != null ? CloseText : new Icon() { Type = "close" };
            }

            SetCloseButtonVisibility();
        }

        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(object), typeof(Alert), new PropertyMetadata(null));

        /// <summary>
        /// Gets/sets additional content of alert.
        /// </summary>
        public object Description
        {
            get { return GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(string), typeof(Alert), new PropertyMetadata(string.Empty));

        /// <summary>
        /// Gets/sets the icon type of the alert.
        /// </summary>
        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(object), typeof(Alert), new PropertyMetadata(null));

        /// <summary>
        /// Gets/sets content of alert.
        /// </summary>
        public object Message
        {
            get { return GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public static readonly DependencyProperty ShowIconProperty =
            DependencyProperty.Register("ShowIcon", typeof(bool), typeof(Alert), new PropertyMetadata(false));

        /// <summary>
        /// Gets/sets whether to show icon.
        /// </summary>
        public bool ShowIcon
        {
            get { return (bool)GetValue(ShowIconProperty); }
            set { SetValue(ShowIconProperty, value); }
        }

        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(AlertType), typeof(Alert), new PropertyMetadata(AlertType.Info));

        /// <summary>
        /// Gets/sets the type of alert.
        /// </summary>
        public AlertType Type
        {
            get { return (AlertType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        public static readonly DependencyProperty IconBrushProperty =
            DependencyProperty.Register("IconBrush", typeof(Brush), typeof(Alert), new PropertyMetadata(null));

        /// <summary>
        /// Gets/sets the alert icon brush.
        /// </summary>
        public Brush IconBrush
        {
            get { return (Brush)GetValue(IconBrushProperty); }
            set { SetValue(IconBrushProperty, value); }
        }

        #endregion

        #region Constructors

        static Alert()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Alert), new FrameworkPropertyMetadata(typeof(Alert)));
        }

        #endregion

        #region Overrides

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            close = GetTemplateChild(PART_Close) as ButtonBase;

            if (close != null)
            {
                Loaded -= OnLoaded;
                Loaded += OnLoaded;

                close.Click -= OnRaiseClosingEvent;
                close.Click += OnRaiseClosingEvent;
            }

            SetCloseButton();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Closing -= OnClosing;
            Closing += OnClosing;
        }

        private void OnRaiseClosingEvent(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            RaiseEvent(new RoutedEventArgs(ClosingEvent, this));
        }

        private void OnClosing(object sender, RoutedEventArgs e)
        {
            SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
            RaiseEvent(new RoutedEventArgs(ClosedEvent, this));
        }

        #endregion
    }

    public enum AlertType : byte
    {
        Success, Info, Warning, Error 
    }
}
