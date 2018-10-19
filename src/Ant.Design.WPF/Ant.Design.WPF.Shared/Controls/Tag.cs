using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;

namespace Antd.Controls
{
    /// <summary>
    /// Tag for categorizing or markup.
    /// </summary>
    [TemplatePart(Name = PART_Close, Type = typeof(ButtonBase))]
    public class Tag : ButtonBase
    {
        #region Fileds

        private const string PART_Close = "PART_Close";

        private ButtonBase close;

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

            close = GetTemplateChild(PART_Close) as ButtonBase;

            if (close != null)
            {
                close.Click -= OnRaiseClosedEvent;
                close.Click += OnRaiseClosedEvent;
                SetCloseVisibility();
            }
        }

        private void OnRaiseClosedEvent(object sender, RoutedEventArgs e)
        {
            if (CloseStoryboard != null)
            {
                var storyboard = CloseStoryboard.Clone();
                storyboard.Completed += RaiseClosedEvent;
                BeginStoryboard(storyboard);
            }
            else
            {
                RaiseClosedEvent(null, null);
            }
        }

        private void RaiseClosedEvent(object sender, EventArgs e)
        {
            Visibility = Visibility.Collapsed;
            RaiseEvent(new RoutedEventArgs(ClosedEvent, this));
        }

        #endregion
    }
}
