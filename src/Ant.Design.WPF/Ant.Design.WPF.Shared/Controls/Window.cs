using System;
using System.Collections;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using Antd.Win32;
using Standard;
using WindowBase = System.Windows.Window;
using ThumbBase = System.Windows.Controls.Primitives.Thumb;
using SystemCommands = Microsoft.Windows.Shell.SystemCommands;
using Microsoft.Windows.Shell;
using System.Windows.Data;

namespace Antd.Controls
{
    [TemplatePart(Name = PART_TitleBarThumb, Type = typeof(ThumbBase))]
    [TemplatePart(Name = PART_TitleBar, Type = typeof(UIElement))]
    [TemplatePart(Name = PART_Icon, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = PART_LeftWindowCommands, Type = typeof(WindowCommands))]
    [TemplatePart(Name = PART_Title, Type = typeof(UIElement))]
    [TemplatePart(Name = PART_RightWindowCommands, Type = typeof(WindowCommands))]
    [TemplatePart(Name = PART_WindowButtons, Type = typeof(WindowButtons))]
    public class Window : WindowBase
    {
        #region Fields

        private const string PART_TitleBarThumb = "PART_TitleBarThumb";

        private const string PART_TitleBar = "PART_TitleBar";

        private const string PART_Icon = "PART_Icon";

        private const string PART_LeftWindowCommands = "PART_LeftWindowCommands";

        private const string PART_Title = "PART_Title";

        private const string PART_RightWindowCommands = "PART_RightWindowCommands";

        private const string PART_WindowButtons = "PART_WindowButtons";

        private ThumbBase titleBarThumb;

        private UIElement titleBar;

        private FrameworkElement icon;

        internal ContentPresenter leftWindowCommands;

        private UIElement title;

        internal ContentPresenter rightWindowCommands;

        internal ContentPresenter windowButtons;

        #endregion

        #region Properties

        public static readonly DependencyProperty IgnoreTaskbarProperty =
            DependencyProperty.Register("IgnoreTaskbar", typeof(bool), typeof(Window), new PropertyMetadata(false));

        /// <summary>
        /// Gets/sets whether the window will ignore (and overlap) the taskbar when maximized.
        /// </summary>
        public bool IgnoreTaskbar
        {
            get { return (bool)GetValue(IgnoreTaskbarProperty); }
            set { SetValue(IgnoreTaskbarProperty, value); }
        }

        public static readonly DependencyProperty ShowSystemMenuProperty =
            DependencyProperty.Register("ShowSystemMenu", typeof(bool), typeof(Window), new PropertyMetadata(false));

        /// <summary>
        /// Gets/sets if the the system menu should popup on right click.
        /// </summary>
        public bool ShowSystemMenu
        {
            get { return (bool)GetValue(ShowSystemMenuProperty); }
            set { SetValue(ShowSystemMenuProperty, value); }
        }

        public static readonly DependencyProperty IsDraggableProperty =
            DependencyProperty.Register("IsDraggable", typeof(bool), typeof(Window), new PropertyMetadata(true));

        /// <summary>
        /// Gets/sets if the the allow drag window
        /// </summary>
        public bool IsDraggable
        {
            get { return (bool)GetValue(IsDraggableProperty); }
            set { SetValue(IsDraggableProperty, value); }
        }

        public static readonly DependencyProperty UseNoneWindowStyleProperty =
            DependencyProperty.Register("UseNoneWindowStyle", typeof(bool), typeof(Window), new PropertyMetadata(false, OnUseNoneWindowStyleChanged));

        /// <summary>
        /// Gets/sets whether the WindowStyle is None or not.
        /// </summary>
        public bool UseNoneWindowStyle
        {
            get { return (bool)GetValue(UseNoneWindowStyleProperty); }
            set { SetValue(UseNoneWindowStyleProperty, value); }
        }

        private static void OnUseNoneWindowStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                (d as Window).ToggleNoneWindowStyle((bool)e.NewValue);
            }
        }

        private void ToggleNoneWindowStyle(bool useNoneWindowStyle)
        {
            SetVisibiltyForTitleBarElements(!useNoneWindowStyle && TitleBarHeight > 0);
        }

        protected IntPtr CriticalHandle
        {
            get
            {
                var value = typeof(WindowBase).GetProperty("CriticalHandle", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this, new object[0]);
                return (IntPtr)value;
            }
        }

        public static readonly DependencyProperty TitleBarHeightProperty =
            DependencyProperty.Register("TitleBarHeight", typeof(double), typeof(Window), new PropertyMetadata(30d, OnTitleBarHeightChanged));
 
        /// <summary>
        /// Gets/sets the TitleBar height.
        /// </summary>
        public double TitleBarHeight
        {
            get { return (double)GetValue(TitleBarHeightProperty); }
            set { SetValue(TitleBarHeightProperty, value); }
        }

        private static void OnTitleBarHeightChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var window = (Window)dependencyObject;
            if (e.NewValue != e.OldValue)
            {
                window.SetVisibiltyForTitleBarElements((int)e.NewValue > 0);
            }
        }

        public static readonly DependencyProperty TitleBarBrushProperty =
            DependencyProperty.Register("TitleBarBrush", typeof(Brush), typeof(Window), new PropertyMetadata(Brushes.Transparent));

        /// <summary>
        /// Gets/sets the brush used for the title bar.
        /// </summary>
        public Brush TitleBarBrush
        {
            get { return (Brush)GetValue(TitleBarBrushProperty); }
            set { SetValue(TitleBarBrushProperty, value); }
        }

        public static readonly DependencyProperty ShowIconProperty =
            DependencyProperty.Register("ShowIcon", typeof(bool), typeof(Window), new PropertyMetadata(true, OnShowIconChanged));

        /// <summary>
        /// Get/sets whether the titlebar icon is visible or not.
        /// </summary>
        public bool ShowIcon
        {
            get { return (bool)GetValue(ShowIconProperty); }
            set { SetValue(ShowIconProperty, value); }
        }

        private static void OnShowIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                (d as Window).SetVisibiltyForIcon();
            }
        }

        public static readonly DependencyProperty IconTemplateProperty =
            DependencyProperty.Register("IconTemplate", typeof(DataTemplate), typeof(Window), new PropertyMetadata(null));

        /// <summary>
        /// Gets/sets the icon content template to show a custom icon.
        /// </summary>
        public DataTemplate IconTemplate
        {
            get { return (DataTemplate)GetValue(IconTemplateProperty); }
            set { SetValue(IconTemplateProperty, value); }
        }

        public static readonly DependencyProperty TitleCharacterCasingProperty =
            DependencyProperty.Register("TitleCharacterCasing", typeof(CharacterCasing), typeof(Window), 
                new FrameworkPropertyMetadata(CharacterCasing.Normal, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsMeasure),
                value => CharacterCasing.Normal <= (CharacterCasing)value && (CharacterCasing)value <= CharacterCasing.Upper);

        /// <summary>
        /// Character casing of the title
        /// </summary>
        public CharacterCasing TitleCharacterCasing
        {
            get { return (CharacterCasing)GetValue(TitleCharacterCasingProperty); }
            set { SetValue(TitleCharacterCasingProperty, value); }
        }

        public static readonly DependencyProperty TitleForegroundProperty =
            DependencyProperty.Register("TitleForeground", typeof(Brush), typeof(Window));

        /// <summary>
        /// Gets/sets the brush used for the titlebar's foreground.
        /// </summary>
        public Brush TitleForeground
        {
            get { return (Brush)GetValue(TitleForegroundProperty); }
            set { SetValue(TitleForegroundProperty, value); }
        }

        public static readonly DependencyProperty TitleAlignmentProperty =
            DependencyProperty.Register("TitleAlignment", typeof(HorizontalAlignment), typeof(Window), new PropertyMetadata(HorizontalAlignment.Stretch, OnTitleAlignmentChanged));

        public HorizontalAlignment TitleAlignment
        {
            get { return (HorizontalAlignment)GetValue(TitleAlignmentProperty); }
            set { SetValue(TitleAlignmentProperty, value); }
        }

        private static void OnTitleAlignmentChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is Window window)
            {
                window.SizeChanged -= window.OnSizeChanged;

                if ((HorizontalAlignment)e.NewValue == HorizontalAlignment.Center)
                {
                    window.SizeChanged += window.OnSizeChanged;
                }
            }
        }

        public static readonly DependencyProperty TitleTemplateProperty =
            DependencyProperty.Register("TitleTemplate", typeof(DataTemplate), typeof(Window), new PropertyMetadata(null));

        /// <summary>
        /// Gets/sets the title content template to show a custom title.
        /// </summary>
        public DataTemplate TitleTemplate
        {
            get { return (DataTemplate)GetValue(TitleTemplateProperty); }
            set { SetValue(TitleTemplateProperty, value); }
        }

        public static readonly DependencyProperty LeftWindowCommandsProperty =
            DependencyProperty.Register("LeftWindowCommands", typeof(WindowCommands), typeof(Window), new PropertyMetadata(null, UpdateLogicalChilds));

        /// <summary>
        /// Gets/sets the left window commands that hosts the user commands.
        /// </summary>
        public WindowCommands LeftWindowCommands
        {
            get { return (WindowCommands)GetValue(LeftWindowCommandsProperty); }
            set { SetValue(LeftWindowCommandsProperty, value); }
        }

        public static readonly DependencyProperty RightWindowCommandsProperty =
            DependencyProperty.Register("RightWindowCommands", typeof(WindowCommands), typeof(Window), new PropertyMetadata(null, UpdateLogicalChilds));

        /// <summary>
        /// Gets/sets the right window commands that hosts the user commands.
        /// </summary>
        public WindowCommands RightWindowCommands
        {
            get { return (WindowCommands)GetValue(RightWindowCommandsProperty); }
            set { SetValue(RightWindowCommandsProperty, value); }
        }

        public static readonly DependencyProperty WindowButtonsProperty =
            DependencyProperty.Register("WindowButtons", typeof(WindowButtons), typeof(Window), new PropertyMetadata(null, UpdateLogicalChilds));

        /// <summary>
        /// Gets/sets the window button commands that hosts the min/max/close commands.
        /// </summary>
        public WindowButtons WindowButtons
        {
            get { return (WindowButtons)GetValue(WindowButtonsProperty); }
            set { SetValue(WindowButtonsProperty, value); }
        }

        private static void UpdateLogicalChilds(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (!(dependencyObject is Window window))
            {
                return;
            }

            if (e.OldValue is FrameworkElement oldChild)
            {
                window.RemoveLogicalChild(oldChild);
            }

            if (e.NewValue is FrameworkElement newChild)
            {
                window.AddLogicalChild(newChild);
                // Yes, that's crazy. But we must do this to enable all possible scenarios for setting DataContext
                // in a Window. Without set the DataContext at this point it can happen that e.g. a Flyout
                // doesn't get the same DataContext.
                // So now we can type
                //
                // InitializeComponent();
                // DataContext = new MainViewModel();
                //
                // or
                //
                // DataContext = new MainViewModel();
                // InitializeComponent();
                //
                newChild.DataContext = window.DataContext;
            }
        }

        #endregion

        #region Constructors

        static Window()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(typeof(Window)));
        }

        public Window()
        {
            DataContextChanged += OnDataContextChanged;
        }

        #endregion

        #region LogicalTree

        protected override IEnumerator LogicalChildren
        {
            get
            {
                // cheat, make a list with all logical content and return the enumerator
                ArrayList children = new ArrayList { Content };

                if (LeftWindowCommands != null)
                {
                    children.Add(LeftWindowCommands);
                }

                if (RightWindowCommands != null)
                {
                    children.Add(RightWindowCommands);
                }

                if (WindowButtons != null)
                {
                    children.Add(WindowButtons);
                }

                return children.GetEnumerator();
            }
        }

        #endregion

        #region Overrides

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            titleBarThumb = GetTemplateChild(PART_TitleBarThumb) as ThumbBase;
            titleBar = GetTemplateChild(PART_TitleBar) as UIElement;
            icon = GetTemplateChild(PART_Icon) as FrameworkElement;
            title = GetTemplateChild(PART_Title) as UIElement;

            leftWindowCommands = GetTemplateChild(PART_LeftWindowCommands) as ContentPresenter;
            rightWindowCommands = GetTemplateChild(PART_RightWindowCommands) as ContentPresenter;
            windowButtons = GetTemplateChild(PART_WindowButtons) as ContentPresenter;

            if (LeftWindowCommands == null)
                LeftWindowCommands = new WindowCommands();
            if (RightWindowCommands == null)
                RightWindowCommands = new WindowCommands();
            if (WindowButtons == null)
                WindowButtons = new WindowButtons();

            LeftWindowCommands.ParentWindow = this;
            RightWindowCommands.ParentWindow = this;
            WindowButtons.ParentWindow = this;

            var windowChrome = GetValue(WindowChrome.WindowChromeProperty) as WindowChrome;

            if (windowChrome != null)
            {
                BindingOperations.SetBinding(
                    windowChrome,
                    WindowChrome.IgnoreTaskbarProperty,
                    new Binding { Path = new PropertyPath(IgnoreTaskbarProperty), Source = this, Mode = BindingMode.OneWay }
                );
            }

            ToggleNoneWindowStyle(UseNoneWindowStyle);
        }

        #endregion
   
        #region Private Methods
   
        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // add these controls to the window with AddLogicalChild method.
            // This has the side effect that the DataContext doesn't update, so do this now here.
            if (LeftWindowCommands != null) LeftWindowCommands.DataContext = DataContext;
            if (RightWindowCommands != null) RightWindowCommands.DataContext = DataContext;
            if (WindowButtons != null) WindowButtons.DataContext = DataContext;
        }

        private void OnSizeChanged(object sender, RoutedEventArgs e)
        {
            // this all works only for centered title
            if (TitleAlignment != HorizontalAlignment.Center) return;

            // Half of this Window
            var halfDistance = ActualWidth / 2;
            // Distance between center and left/right
            var distanceToCenter = title.DesiredSize.Width / 2;
            // Distance between right edge from LeftWindowCommands to left window side
            var distanceFromLeft = icon.ActualWidth + LeftWindowCommands.ActualWidth;
            // Distance between left edge from RightWindowCommands to right window side
            var distanceFromRight = WindowButtons.ActualWidth + RightWindowCommands.ActualWidth;
            // Margin
            const double horizontalMargin = 5.0;

            var dLeft = distanceFromLeft + distanceToCenter + horizontalMargin;
            var dRight = distanceFromRight + distanceToCenter + horizontalMargin;

            // TODO They should also change when the layout changes.
            if ((dLeft < halfDistance) && (dRight < halfDistance))
            {
                Grid.SetColumn(title, 0);
                Grid.SetColumnSpan(title, 5);
            }
            else
            {
                Grid.SetColumn(title, 2);
                Grid.SetColumnSpan(title, 1);
            }
        }

        private void SetVisibiltyForTitleBarElements(bool visible)
        {
            var value = visible ? Visibility.Visible : Visibility.Collapsed;
            titleBar?.SetCurrentValue(VisibilityProperty, value);

            title?.SetCurrentValue(VisibilityProperty, value);
            SetVisibiltyForIcon();

            leftWindowCommands?.SetValue(VisibilityProperty, value);
            rightWindowCommands?.SetValue(VisibilityProperty, value);

            windowButtons?.SetCurrentValue(VisibilityProperty, value);
            SetWindowEvents();
        }

        private void SetVisibiltyForIcon()
        {
            if (icon == null) return;
            icon.Visibility = ShowIcon && !UseNoneWindowStyle && TitleBarHeight > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void SetWindowEvents()
        {
            // clear all event handlers first
            ClearWindowEvents();

            // set mouse down/up for icon
            if (icon != null && icon.Visibility == Visibility.Visible)
            {
                icon.MouseLeftButtonDown += OnIconMouseLeftButtonDown;
                icon.MouseRightButtonUp += OnThumbMouseRightButtonUp;
            }

            if (titleBarThumb != null)
            {
                titleBarThumb.PreviewMouseLeftButtonUp += OnThumbPreviewMouseLeftButtonUp;
                titleBarThumb.DragDelta += OnThumbDragDelta;
                titleBarThumb.MouseDoubleClick += OnThumbMouseDoubleClick;
                titleBarThumb.MouseRightButtonUp += OnThumbMouseRightButtonUp;
            }

            var thumbContentControl = title as IThumb;

            if (thumbContentControl != null)
            {
                thumbContentControl.PreviewMouseLeftButtonUp += OnThumbPreviewMouseLeftButtonUp;
                thumbContentControl.DragDelta += OnThumbDragDelta;
                thumbContentControl.MouseDoubleClick += OnThumbMouseDoubleClick;
                thumbContentControl.MouseRightButtonUp += OnThumbMouseRightButtonUp;
            }

            // handle size if we have a Grid for the title (e.g. clean window have a centered title)
            if (title != null && TitleAlignment == HorizontalAlignment.Center)
            {
                SizeChanged += OnSizeChanged;
            }
        }

        private void ClearWindowEvents()
        {
            // clear all event handlers first:
            if (titleBarThumb != null)
            {
                titleBarThumb.PreviewMouseLeftButtonUp -= OnThumbPreviewMouseLeftButtonUp;
                titleBarThumb.DragDelta -= OnThumbDragDelta;
                titleBarThumb.MouseDoubleClick -= OnThumbMouseDoubleClick;
                titleBarThumb.MouseRightButtonUp -= OnThumbMouseRightButtonUp;
            }

            var thumbContentControl = title as IThumb;

            if (thumbContentControl != null)
            {
                thumbContentControl.PreviewMouseLeftButtonUp -= OnThumbPreviewMouseLeftButtonUp;
                thumbContentControl.DragDelta -= OnThumbDragDelta;
                thumbContentControl.MouseDoubleClick -= OnThumbMouseDoubleClick;
                thumbContentControl.MouseRightButtonUp -= OnThumbMouseRightButtonUp;
            }

            if (icon != null)
            {
                icon.MouseLeftButtonDown -= OnIconMouseLeftButtonDown;
                icon.MouseRightButtonUp -= OnThumbMouseRightButtonUp;
            }

            SizeChanged -= OnSizeChanged;
        }
        
        private void OnIconMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!ShowSystemMenu) return;

            if (e.ClickCount == 2)
            {
                Close();
            } else
            {
#pragma warning disable 618
                SystemCommands.ShowSystemMenu(this, PointToScreen(new Point(0, TitleBarHeight)));
#pragma warning restore 618
            }
        }

        private void OnThumbPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DoThumbPreviewMouseLeftButtonUp(this, e);
        }

        private void OnThumbDragDelta(object sender, DragDeltaEventArgs e)
        {
            DoThumbDragDelta(sender as IThumb, this, e);
        }

        private void OnThumbMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DoThumbMouseDoubleClick(this, e);
        }

        private void OnThumbMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            DoThumbMouseRightButtonUp(this, e);
        }

        internal static void DoThumbPreviewMouseLeftButtonUp(Window window, MouseButtonEventArgs e)
        {
            if (e.Source == e.OriginalSource)
            {
                Mouse.Capture(null);
            }
        }

        internal static void DoThumbDragDelta(IThumb thumb, Window window, DragDeltaEventArgs e)
        {
            if (thumb == null)
            {
                throw new ArgumentNullException(nameof(thumb));
            }

            if (window == null)
            {
                throw new ArgumentNullException(nameof(window));
            }

            // drag only if IsDraggable is set to true
            if (!window.IsDraggable ||
                (!(Math.Abs(e.HorizontalChange) > 2) && !(Math.Abs(e.VerticalChange) > 2)))
            {
                return;
            }

            // tage from DragMove internal code
            window.VerifyAccess();

            //var cursorPos = WinApiHelper.GetPhysicalCursorPos();

            // if the window is maximized dragging is only allowed on title bar (also if not visible)
            var maximized = window.WindowState == WindowState.Maximized;
            var isMouseOnTitlebar = Mouse.GetPosition(thumb).Y <= window.TitleBarHeight && window.TitleBarHeight > 0;

            if (!isMouseOnTitlebar && maximized) return;

#pragma warning disable 618
            // for the touch usage
            UnsafeNativeMethods.ReleaseCapture();
#pragma warning restore 618

            if (maximized)
            {
                //var cursorXPos = cursorPos.x;
                EventHandler windowOnStateChanged = null;
                windowOnStateChanged = (sender, args) =>
                {
                    //window.Top = 2;
                    //window.Left = Math.Max(cursorXPos - window.RestoreBounds.Width / 2, 0);

                    window.StateChanged -= windowOnStateChanged;
                    if (window.WindowState == WindowState.Normal)
                    {
                        Mouse.Capture(thumb, CaptureMode.Element);
                    }
                };

                window.StateChanged += windowOnStateChanged;
            }

            var criticalHandle = window.CriticalHandle;
            // DragMove works too
            // window.DragMove();
            // instead this 2 lines
#pragma warning disable 618
            NativeMethods.SendMessage(criticalHandle, WM.SYSCOMMAND, (IntPtr)SC.MOUSEMOVE, IntPtr.Zero);
            NativeMethods.SendMessage(criticalHandle, WM.LBUTTONUP, IntPtr.Zero, IntPtr.Zero);
#pragma warning restore 618
        }

        /// <summary>
        /// Change window state on MouseDoubleClick
        /// </summary>
        /// <param name="window"></param>
        /// <param name="e"></param>
        internal static void DoThumbMouseDoubleClick(Window window, MouseButtonEventArgs e)
        {
            // restore/maximize only with left button
            if (e.ChangedButton == MouseButton.Left)
            {
                // we can maximize or restore the window if the title bar height is set (also if title bar is hidden)
                var canResize = window.ResizeMode == ResizeMode.CanResizeWithGrip || window.ResizeMode == ResizeMode.CanResize;
                var mousePos = Mouse.GetPosition(window);
                var isMouseOnTitlebar = mousePos.Y <= window.TitleBarHeight && window.TitleBarHeight > 0;
                if (canResize && isMouseOnTitlebar)
                {
#pragma warning disable 618
                    if (window.WindowState == WindowState.Normal)
                    {
                        SystemCommands.MaximizeWindow(window);
                    }
                    else
                    {
                       SystemCommands.RestoreWindow(window);
                    }
#pragma warning restore 618
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// Show system menu on MouseRightButtonUp
        /// </summary>
        /// <param name="window"></param>
        /// <param name="e"></param>
        internal static void DoThumbMouseRightButtonUp(Window window, MouseButtonEventArgs e)
        {
            if (window.ShowSystemMenu)
            {
                // show menu only if mouse pos is on title bar or if we have a window with none style and no title bar
                var mousePos = e.GetPosition(window);
                if ((mousePos.Y <= window.TitleBarHeight && window.TitleBarHeight > 0) || (window.UseNoneWindowStyle && window.TitleBarHeight <= 0))
                {
#pragma warning disable 618
                    SystemCommands.ShowSystemMenu(window, window.PointToScreen(mousePos));
#pragma warning restore 618
                }
            }
        }

        /// <summary>
        /// Gets the template child with the given name.
        /// </summary>
        /// <typeparam name="T">The interface type inheirted from DependencyObject.</typeparam>
        /// <param name="name">The name of the template child.</param>
        internal T GetPart<T>(string name) where T : class
        {
            return GetTemplateChild(name) as T;
        }

        /// <summary>
        /// Gets the template child with the given name.
        /// </summary>
        /// <param name="name">The name of the template child.</param>
        internal DependencyObject GetPart(string name)
        {
            return GetTemplateChild(name);
        }

        #endregion
    }
}
