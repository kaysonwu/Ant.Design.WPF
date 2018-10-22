using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Antd.Controls
{
    /// <summary>
    /// Alert component for feedback.
    /// </summary>
    [ContentProperty("Message")]
    [TemplatePart(Name = PART_Icon, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = PART_Close, Type = typeof(ButtonBase))]
    public class Alert : Control
    {
        #region Fields

        private const string PART_Icon = "PART_Icon";

        private const string PART_Close = "PART_Close";

        private ContentPresenter icon;

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
        /// Gets/sets whether to show as banner
        /// </summary>
        public bool Banner
        {
            get { return (bool)GetValue(BannerProperty); }
            set { SetValue(BannerProperty, value); }
        }

        public static readonly DependencyProperty ClosableProperty =
            DependencyProperty.Register("Closable", typeof(bool?), typeof(Alert), new PropertyMetadata(null, OnClosableChanged));

        /// <summary>
        /// Gets/sets whether alert can be closed
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
        /// Gets/sets close text to show
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
            DependencyProperty.Register("Description", typeof(object), typeof(Alert), new PropertyMetadata(null, OnDefaultIconChanged));

        /// <summary>
        /// Gets/sets additional content of alert
        /// </summary>
        public object Description
        {
            get { return GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(object), typeof(Alert), new PropertyMetadata(null, OnIconChanged));

        /// <summary>
        /// Gets/sets custom icon, effective when showIcon is true
        /// </summary>
        public object Icon
        {
            get { return GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Alert).SetIcon(true);
        }

        private void SetIcon(bool force = false)
        {
            if (icon == null || (!force && Icon != null)) return;

            string type;

            if (Icon == null)
            {
                switch (Type)
                {
                    case AlertType.Success:
                        type = "check-circle";
                        break;
                    case AlertType.Warning:
                        type = "close-circle";
                        break;
                    case AlertType.Error:
                        type = "exclamation-circle";
                        break;
                    default:
                        type = "info-circle";
                        break;
                }

                if (Description != null)
                {
                    type += "-o";
                }

            } else if (Icon is string)
            {
                type = Icon as string;
            }
            else
            {
                icon.Content = Icon;
                return;
            }

            icon.Content = new Icon() { Type = type };
        }

        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(object), typeof(Alert), new PropertyMetadata(null));

        /// <summary>
        /// Gets/sets content of alert
        /// </summary>
        public object Message
        {
            get { return GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public static readonly DependencyProperty ShowIconProperty =
            DependencyProperty.Register("ShowIcon", typeof(bool), typeof(Alert), new PropertyMetadata(false, OnShowIconChanged));

        /// <summary>
        /// Gets/sets whether to show icon
        /// </summary>
        public bool ShowIcon
        {
            get { return (bool)GetValue(ShowIconProperty); }
            set { SetValue(ShowIconProperty, value); }
        }

        private static void OnShowIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Alert).SetIconVisibility();
        }

        private void SetIconVisibility()
        {
            if (icon != null)
            {
                icon.Visibility = ShowIcon ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(AlertType), typeof(Alert), new PropertyMetadata(AlertType.Info, OnDefaultIconChanged));

        /// <summary>
        /// Gets/sets the type of alert
        /// </summary>
        public AlertType Type
        {
            get { return (AlertType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        private static void OnDefaultIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Alert).SetIcon();
        }

        public static readonly DependencyProperty IconBrushProperty =
            DependencyProperty.Register("IconBrush", typeof(Brush), typeof(Alert), new PropertyMetadata(null));

        /// <summary>
        /// Gets/sets the alert icon brush
        /// </summary>
        public Brush IconBrush
        {
            get { return (Brush)GetValue(IconBrushProperty); }
            set { SetValue(IconBrushProperty, value); }
        }

        public static readonly DependencyProperty CloseStoryboardProperty =
            DependencyProperty.Register("CloseStoryboard", typeof(Storyboard), typeof(Alert), new PropertyMetadata(null));

        /// <summary>
        /// Gets/sets the closing animation of the alert
        /// </summary>
        public Storyboard CloseStoryboard
        {
            get { return (Storyboard)GetValue(CloseStoryboardProperty); }
            set { SetValue(CloseStoryboardProperty, value); }
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

            icon = GetTemplateChild(PART_Icon) as ContentPresenter;
            close = GetTemplateChild(PART_Close) as ButtonBase;

            if (close != null)
            {
                Loaded -= OnLoaded;
                Loaded += OnLoaded;

                close.Click -= OnRaiseClosingEvent;
                close.Click += OnRaiseClosingEvent;
            }

            SetIconVisibility();
            SetIcon(true);

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
            if (CloseStoryboard != null)
            {
                var storyboard = CloseStoryboard.Clone();
                storyboard.Completed += OnRaiseClosedEvent;
                BeginStoryboard(storyboard);

            }
            else
            {
                OnRaiseClosedEvent(null, null);
            }
        }

        private void OnRaiseClosedEvent(object sender, EventArgs e)
        {
            Visibility = Visibility.Collapsed;
            RaiseEvent(new RoutedEventArgs(ClosedEvent, this));
        }

        #endregion
    }

    public enum AlertType : byte
    {
        Success, Info, Warning, Error 
    }
}
