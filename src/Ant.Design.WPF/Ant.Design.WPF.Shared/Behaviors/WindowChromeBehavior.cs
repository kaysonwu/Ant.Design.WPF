using System;
using System.Linq;
using System.Management;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;
using System.Windows.Interop;
using System.Windows.Threading;
using Microsoft.Windows.Shell;
using Standard;
using Antd.Win32;
using Antd.Controls;
using WindowBase = System.Windows.Window;
using AntdWindow = Antd.Controls.Window;

namespace Antd.Behaviors
{
    /// <summary>
    /// With this class we can make custom window styles.
    /// </summary>
    public class WindowChromeBehavior : Behavior<WindowBase>
    {
        #region Fields

        private IntPtr handle;

        private bool isCleanedUp;

        private IntPtr taskbarHandle;

        private HwndSource hwndSource;

        private WindowChrome windowChrome;

        private Thickness? savedBorderThickness;

        private Thickness? savedResizeBorderThickness;

        private PropertyChangeNotifier topMostChangeNotifier;

        private PropertyChangeNotifier borderThicknessChangeNotifier;

        private PropertyChangeNotifier resizeBorderThicknessChangeNotifier;

        private bool savedTopMost;

        private bool isWindwos10OrHigher;

        #endregion

        #region Properties

        public static readonly DependencyProperty ResizeBorderThicknessProperty = DependencyProperty.Register(
            "ResizeBorderThickness",
            typeof(Thickness), 
            typeof(WindowChromeBehavior), 
            new PropertyMetadata(
#if NET45 || NET462
                SystemParameters.WindowResizeBorderThickness
#else
                SystemParameters2.Current.WindowResizeBorderThickness
#endif
            ));

        /// <summary>
        /// Mirror property for <see cref="WindowChrome.ResizeBorderThickness"/>.
        /// </summary>
        public Thickness ResizeBorderThickness
        {
            get { return (Thickness)GetValue(ResizeBorderThicknessProperty); }
            set { SetValue(ResizeBorderThicknessProperty, value); }
        }

        public static readonly DependencyProperty GlassFrameThicknessProperty = DependencyProperty.Register(
            "GlassFrameThickness", 
            typeof(Thickness), 
            typeof(WindowChromeBehavior), 
            new PropertyMetadata(default(Thickness), OnGlassFrameThicknessChanged));

        /// <summary>
        /// Mirror property for <see cref="WindowChrome.GlassFrameThickness"/>.
        /// </summary>
        public Thickness GlassFrameThickness
        {
            get { return (Thickness)GetValue(GlassFrameThicknessProperty); }
            set { SetValue(GlassFrameThicknessProperty, value); }
        }

        private static void OnGlassFrameThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        public static readonly DependencyProperty IgnoreTaskbarProperty = DependencyProperty.Register(
            "IgnoreTaskbar", 
            typeof(bool), 
            typeof(WindowChromeBehavior), 
            new PropertyMetadata(false, OnIgnoreTaskbarChanged));

        /// <summary>
        /// Mirror property for <see cref="WindowChrome.IgnoreTaskbar"/>.
        /// </summary>
        public bool IgnoreTaskbar
        {
            get { return (bool)GetValue(IgnoreTaskbarProperty); }
            set { SetValue(IgnoreTaskbarProperty, value); }
        }

        private static void OnIgnoreTaskbarChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (WindowChromeBehavior)d;
            if (behavior.windowChrome != null)
            {
                if (!Equals(behavior.windowChrome.IgnoreTaskbar, behavior.IgnoreTaskbar))
                {
                    // another special hack to avoid nasty resizing
                    // repro
                    // ResizeMode="NoResize"
                    // WindowState="Maximized"
                    // IgnoreTaskbar="True"
                    // this only happens if we change this at runtime
                    behavior.windowChrome.IgnoreTaskbar = behavior.IgnoreTaskbar;

                    if (behavior.AssociatedObject.WindowState == WindowState.Maximized)
                    {
                        behavior.AssociatedObject.WindowState = WindowState.Normal;
                        behavior.AssociatedObject.WindowState = WindowState.Maximized;
                    }
                }
            }
        }

        #endregion

        #region Methods

        private static bool IsWindows10OrHigher()
        {
            var version = NtDll.RtlGetVersion();
            if (default(Version) == version)
            {
                // Snippet from Koopakiller https://dotnet-snippets.de/snippet/os-version-name-mit-wmi/4929
                using (var mos = new ManagementObjectSearcher("SELECT Caption, Version FROM Win32_OperatingSystem"))
                {
                    var attribs = mos.Get().OfType<ManagementObject>();
                    //caption = attribs.FirstOrDefault().GetPropertyValue("Caption").ToString() ?? "Unknown";
                    version = new Version((attribs.FirstOrDefault()?.GetPropertyValue("Version") ?? "0.0.0.0").ToString());
                }
            }
            return version >= new Version(10, 0);
        }

        protected override void OnAttached()
        {
            isWindwos10OrHigher = IsWindows10OrHigher();
            InitializeWindowChrome();

            AssociatedObject.WindowStyle = WindowStyle.None;

            savedBorderThickness = AssociatedObject.BorderThickness;
            borderThicknessChangeNotifier = new PropertyChangeNotifier(AssociatedObject, Control.BorderThicknessProperty);
            borderThicknessChangeNotifier.ValueChanged += OnBorderThicknessChanged;

            savedResizeBorderThickness = windowChrome.ResizeBorderThickness;
            resizeBorderThicknessChangeNotifier = new PropertyChangeNotifier(this, ResizeBorderThicknessProperty);
            resizeBorderThicknessChangeNotifier.ValueChanged += OnResizeBorderThicknessChanged;


            savedTopMost = AssociatedObject.Topmost;
            topMostChangeNotifier = new PropertyChangeNotifier(AssociatedObject, WindowBase.TopmostProperty);
            topMostChangeNotifier.ValueChanged += OnTopMostChanged;

            AssociatedObject.SourceInitialized += OnSourceInitialized;
            AssociatedObject.Loaded += OnLoaded;
            AssociatedObject.Unloaded += OnUnloaded;
            AssociatedObject.Closed += OnClosed;
            AssociatedObject.StateChanged += OnStateChanged;
            AssociatedObject.LostFocus += OnLostFocus;
            AssociatedObject.Deactivated += OnDeactivated;

            base.OnAttached();
        }

        private void OnBorderThicknessChanged(object sender, EventArgs e)
        {
            // It's bad if the window is null at this point, but we check this here to prevent the possible occurred exception
            var window = AssociatedObject;
            if (window != null)
            {
                savedBorderThickness = window.BorderThickness;
            }
        }

        private void OnResizeBorderThicknessChanged(object sender, EventArgs e)
        {
            savedResizeBorderThickness = ResizeBorderThickness;
        }

        private void OnTopMostChanged(object sender, EventArgs e)
        {
            // It's bad if the window is null at this point, but we check this here to prevent the possible occurred exception
            var window = AssociatedObject;
            if (window != null)
            {
                savedTopMost = window.Topmost;
            }
        }

        private void InitializeWindowChrome()
        {
            windowChrome = new WindowChrome();

            BindingOperations.SetBinding(windowChrome, WindowChrome.ResizeBorderThicknessProperty, new Binding { Path = new PropertyPath(ResizeBorderThicknessProperty), Source = this });
            BindingOperations.SetBinding(windowChrome, WindowChrome.GlassFrameThicknessProperty, new Binding { Path = new PropertyPath(GlassFrameThicknessProperty), Source = this });

            windowChrome.CaptionHeight = 0;
            windowChrome.CornerRadius = default(CornerRadius);
            windowChrome.UseAeroCaptionButtons = false;

            AssociatedObject.SetValue(WindowChrome.WindowChromeProperty, windowChrome);
        }

        private void OnSourceInitialized(object sender, EventArgs e)
        {
            handle = new WindowInteropHelper(AssociatedObject).Handle;

            if (IntPtr.Zero == handle)
            {
                throw new Exception("Uups, at this point we really need the Handle from the associated object!");
            }

            if (AssociatedObject.SizeToContent != SizeToContent.Manual && AssociatedObject.WindowState == WindowState.Normal)
            {
                // Another try to fix SizeToContent
                // without this we get nasty glitches at the borders
                Invoke(AssociatedObject, () =>
                {
                    AssociatedObject.InvalidateMeasure();
                    Win32.RECT rect;
                    if (UnsafeNativeMethods.GetWindowRect(handle, out rect))
                    {
                        var flags = SWP.SHOWWINDOW;
                        if (!AssociatedObject.ShowActivated)
                        {
                            flags |= SWP.NOACTIVATE;
                        }
                        NativeMethods.SetWindowPos(handle, new IntPtr(-2), rect.left, rect.top, rect.Width, rect.Height, flags);
                    }
                });
            }

            hwndSource = HwndSource.FromHwnd(handle);
            hwndSource?.AddHook(WindowProc);

            // handle the maximized state here too (to handle the border in a correct way)
            HandleMaximize();
        }

        private static void Invoke(DispatcherObject dispatcherObject, Action invokeAction)
        {
            if (dispatcherObject == null)
            {
                throw new ArgumentNullException(nameof(dispatcherObject));
            }
            if (invokeAction == null)
            {
                throw new ArgumentNullException(nameof(invokeAction));
            }
            if (dispatcherObject.Dispatcher.CheckAccess())
            {
                invokeAction();
            }
            else
            {
                dispatcherObject.Dispatcher.Invoke(invokeAction);
            }
        }

        private IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            var result = IntPtr.Zero;

            switch (msg)
            {
                case (int)WM.NCPAINT:
                    break;
                case (int)WM.WINDOWPOSCHANGING:
                    break;
            }

            return result;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var window = sender as AntdWindow;
            if (window == null)
            {
                return;
            }

            if (window.ResizeMode != ResizeMode.NoResize)
            {
                // TODO 恢复
               // window.SetIsHitTestVisibleInChromeProperty<UIElement>("PART_Icon");
               // window.SetWindowChromeResizeGripDirection("WindowResizeGrip", ResizeGripDirection.BottomRight);
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            Cleanup();
        }

        private void Cleanup()
        {
            if (!isCleanedUp)
            {
                isCleanedUp = true;

                if (taskbarHandle != IntPtr.Zero
                    && isWindwos10OrHigher)
                {
                    DeactivateTaskbarFix(taskbarHandle);
                }

                // clean up events
                AssociatedObject.SourceInitialized -= OnSourceInitialized;
                AssociatedObject.Loaded -= OnLoaded;
                AssociatedObject.Unloaded -= OnUnloaded;
                AssociatedObject.Closed -= OnClosed;
                AssociatedObject.StateChanged -= OnStateChanged;
                AssociatedObject.LostFocus -= OnLostFocus;
                AssociatedObject.Deactivated -= OnDeactivated;

                hwndSource?.RemoveHook(WindowProc);
                windowChrome = null;
            }
        }

        private void OnClosed(object sender, EventArgs e)
        {
            Cleanup();
        }

        private void OnStateChanged(object sender, EventArgs e)
        {
            HandleMaximize();
        }

        private void HandleMaximize()
        {
            var raiseValueChanged = topMostChangeNotifier.RaiseValueChanged;
            topMostChangeNotifier.RaiseValueChanged = false;

            HandleBorderAndResizeBorderThicknessDuringMaximize();

            if (AssociatedObject.WindowState == WindowState.Maximized)
            {
                if (handle != IntPtr.Zero)
                {
                    // WindowChrome handles the size false if the main monitor is lesser the monitor where the window is maximized
                    // so set the window pos/size twice
                    var monitor = UnsafeNativeMethods.MonitorFromWindow(handle, MonitorOptions.MONITOR_DEFAULTTONEAREST);
                    if (monitor != IntPtr.Zero)
                    {
                        var monitorInfo = NativeMethods.GetMonitorInfo(monitor);
                        var monitorRect = IgnoreTaskbar ? monitorInfo.rcMonitor : monitorInfo.rcWork;

                        var x = monitorRect.Left;
                        var y = monitorRect.Top;
                        var cx = monitorRect.Width;
                        var cy = monitorRect.Height;

                        if (IgnoreTaskbar && isWindwos10OrHigher)
                        {
                            ActivateTaskbarFix(monitor);
                        }

                        NativeMethods.SetWindowPos(handle, new IntPtr(-2), x, y, cx, cy, SWP.SHOWWINDOW);
                    }
                }
            }
            else
            {
                // #2694 make sure the window is not on top after restoring window
                // this issue was introduced after fixing the windows 10 bug with the taskbar and a maximized window that ignores the taskbar
                if (taskbarHandle != IntPtr.Zero
                    && isWindwos10OrHigher)
                {
                    DeactivateTaskbarFix(taskbarHandle);
                }
            }

            // fix nasty TopMost bug
            // - set TopMost="True"
            // - start mahapps demo
            // - TopMost works
            // - maximize window and back to normal
            // - TopMost is gone
            //
            // Problem with minimize animation when window is maximized #1528
            // 1. Activate another application (such as Google Chrome).
            // 2. Run the demo and maximize it.
            // 3. Minimize the demo by clicking on the taskbar button.
            // Note that the minimize animation in this case does actually run, but somehow the other
            // application (Google Chrome in this example) is instantly switched to being the top window,
            // and so blocking the animation view.
            AssociatedObject.Topmost = false;
            AssociatedObject.Topmost = AssociatedObject.WindowState == WindowState.Minimized || savedTopMost;

            topMostChangeNotifier.RaiseValueChanged = raiseValueChanged;
        }

        /// <summary>
        /// This fix is needed because style triggers don't work if someone sets the value locally/directly on the window.
        /// </summary>
        private void HandleBorderAndResizeBorderThicknessDuringMaximize()
        {
            borderThicknessChangeNotifier.RaiseValueChanged = false;
            resizeBorderThicknessChangeNotifier.RaiseValueChanged = false;

            if (AssociatedObject.WindowState == WindowState.Maximized)
            {
                var monitor = IntPtr.Zero;

                if (handle != IntPtr.Zero)
                {
                    monitor = UnsafeNativeMethods.MonitorFromWindow(handle, MonitorOptions.MONITOR_DEFAULTTONEAREST);
                }

                if (monitor != IntPtr.Zero)
                {
                    var monitorInfo = NativeMethods.GetMonitorInfo(monitor);
                    var monitorRect = IgnoreTaskbar ? monitorInfo.rcMonitor : monitorInfo.rcWork;

                    var rightBorderThickness = 0D;
                    var bottomBorderThickness = 0D;

                    //if (KeepBorderOnMaximize
                    //    && savedBorderThickness.HasValue)
                    //{
                    //    // If the maximized window will have a width less than the monitor size, show the right border.
                    //    if (AssociatedObject.MaxWidth < monitorRect.Width)
                    //    {
                    //        rightBorderThickness = savedBorderThickness.Value.Right;
                    //    }

                    //    // If the maximized window will have a height less than the monitor size, show the bottom border.
                    //    if (AssociatedObject.MaxHeight < monitorRect.Height)
                    //    {
                    //        bottomBorderThickness = savedBorderThickness.Value.Bottom;
                    //    }
                    //}

                    // set window border, so we can move the window from top monitor position
                    AssociatedObject.BorderThickness = new Thickness(0, 0, rightBorderThickness, bottomBorderThickness);
                }
                else // Can't get monitor info, so just remove all border thickness
                {
                    AssociatedObject.BorderThickness = new Thickness(0);
                }

                windowChrome.ResizeBorderThickness = new Thickness(0);
            }
            else
            {
                AssociatedObject.BorderThickness = savedBorderThickness.GetValueOrDefault(new Thickness(0));

                var resizeBorderThickness = savedResizeBorderThickness.GetValueOrDefault(new Thickness(0));

                if (windowChrome.ResizeBorderThickness != resizeBorderThickness)
                {
                    windowChrome.ResizeBorderThickness = resizeBorderThickness;
                }
            }

            borderThicknessChangeNotifier.RaiseValueChanged = true;
            resizeBorderThicknessChangeNotifier.RaiseValueChanged = true;
        }

        private void ActivateTaskbarFix(IntPtr monitor)
        {
            var trayWndHandle = NativeMethods.GetTaskBarHandleForMonitor(monitor);

            if (trayWndHandle != IntPtr.Zero)
            {
                taskbarHandle = trayWndHandle;
                NativeMethods.SetWindowPos(trayWndHandle, new IntPtr(1), 0, 0, 0, 0, SWP.TOPMOST);
                NativeMethods.SetWindowPos(trayWndHandle, new IntPtr(0), 0, 0, 0, 0, SWP.TOPMOST);
                NativeMethods.SetWindowPos(trayWndHandle, new IntPtr(-2), 0, 0, 0, 0, SWP.TOPMOST);
            }
        }

        private void DeactivateTaskbarFix(IntPtr trayWndHandle)
        {
            if (trayWndHandle != IntPtr.Zero)
            {
                taskbarHandle = IntPtr.Zero;
                NativeMethods.SetWindowPos(trayWndHandle, new IntPtr(1), 0, 0, 0, 0, SWP.TOPMOST);
                NativeMethods.SetWindowPos(trayWndHandle, new IntPtr(0), 0, 0, 0, 0, SWP.TOPMOST);
                NativeMethods.SetWindowPos(trayWndHandle, new IntPtr(-1), 0, 0, 0, 0, SWP.TOPMOST);
            }
        }

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            TopMostHack();
        }

        private void OnDeactivated(object sender, EventArgs e)
        {
            TopMostHack();
        }

        private void TopMostHack()
        {
            if (AssociatedObject.Topmost)
            {
                var raiseValueChanged = topMostChangeNotifier.RaiseValueChanged;
                topMostChangeNotifier.RaiseValueChanged = false;
                AssociatedObject.Topmost = false;
                AssociatedObject.Topmost = true;
                topMostChangeNotifier.RaiseValueChanged = raiseValueChanged;
            }
        }

        #endregion
    }
}