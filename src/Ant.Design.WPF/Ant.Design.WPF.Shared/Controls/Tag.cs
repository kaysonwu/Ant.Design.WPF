using System;
using System.Windows;
using System.Windows.Media.Animation;
using ContentControlBase = System.Windows.Controls.ContentControl;

namespace Antd.Controls
{
    /// <summary>
    /// Tag for categorizing or markup.
    /// </summary>
    [TemplatePart(Name = PART_Close, Type = typeof(UIElement))]
    public class Tag : ContentControlBase
    {
        #region Fileds

        private const string PART_Close = "PART_Close";

        private UIElement close;

        #endregion

        #region Events

        public static readonly RoutedEvent ClosedEvent =
            EventManager.RegisterRoutedEvent("Closed", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Tag));

        /// <summary>
        /// Occurs when a Tag is closed and is no longer visible.
        /// </summary>
        public event RoutedEventHandler Closed
        {
            add { AddHandler(ClosedEvent, value); }
            remove { RemoveHandler(ClosedEvent, value); }
        }

        public static readonly RoutedEvent ClosingEvent =
            EventManager.RegisterRoutedEvent("Closing", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Tag));

        /// <summary>
        /// Occurs when closing the tag.
        /// </summary>
        public event RoutedEventHandler Closing
        {
            add { AddHandler(ClosingEvent, value); }
            remove { RemoveHandler(ClosingEvent, value); }
        }

        #endregion

        #region Properties

        public static readonly DependencyProperty ClosableProperty =
            DependencyProperty.Register("Closable", typeof(bool), typeof(Tag), new PropertyMetadata(false, OnClosableChnaged));

        /// <summary>
        /// Gets/sets whether the Tag can be closed.
        /// </summary>
        public bool Closable
        {
            get { return (bool)GetValue(ClosableProperty); }
            set { SetValue(ClosableProperty, value); }
        }

        private static void OnClosableChnaged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Tag).SetCloseVisibility();
        }

        private void SetCloseVisibility()
        {
            if (close != null)
            {
                close.Visibility = Closable ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public static readonly DependencyProperty ColorfulProperty =
            DependencyProperty.Register("Colorful", typeof(bool), typeof(Tag), new PropertyMetadata(false));

        /// <summary>
        /// Gets/sets whether the tag automatically sets the background and border brush based on the foreground.
        /// </summary>
        public bool Colorful
        {
            get { return (bool)GetValue(ColorfulProperty); }
            set { SetValue(ColorfulProperty, value); }
        }

        public static readonly DependencyProperty CloseStoryboardProperty =
            DependencyProperty.Register("CloseStoryboard", typeof(Storyboard), typeof(Tag), new PropertyMetadata(null));

        /// <summary>
        /// Gets/sets the close animation of the tag.
        /// </summary>
        public Storyboard CloseStoryboard
        {
            get { return (Storyboard)GetValue(CloseStoryboardProperty); }
            set { SetValue(CloseStoryboardProperty, value); }
        }

        #endregion

        #region Constructors

        static Tag()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Tag), new FrameworkPropertyMetadata(typeof(Tag)));
        }

        #endregion

        #region Overrides

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            close = GetTemplateChild(PART_Close) as UIElement;

            if (close != null)
            {
                Loaded -= OnLoaded;
                Loaded += OnLoaded;

                close.MouseLeftButtonUp -= OnRaiseClosingEvent;
                close.MouseLeftButtonUp += OnRaiseClosingEvent;

                SetCloseVisibility();
            }
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
}
