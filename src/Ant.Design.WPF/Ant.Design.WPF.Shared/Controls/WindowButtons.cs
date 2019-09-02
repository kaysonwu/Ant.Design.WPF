namespace Antd.Controls
{
    using Antd.Win32;
    using System;
    using System.ComponentModel;
    using System.Text;
    using System.Windows;
    using System.Windows.Threading;
    using System.Windows.Controls.Primitives;
    using ContentControlBase = System.Windows.Controls.ContentControl;
    using SystemCommands = Microsoft.Windows.Shell.SystemCommands;

    [TemplatePart(Name = PART_Min, Type = typeof(ButtonBase))]
    [TemplatePart(Name = PART_Max, Type = typeof(ButtonBase))]
    [TemplatePart(Name = PART_Close, Type = typeof(ButtonBase))]
    public class WindowButtons : ContentControlBase, INotifyPropertyChanged
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event ClosingWindowEventHandler ClosingWindow;

        public delegate void ClosingWindowEventHandler(object sender, ClosingWindowEventHandlerArgs args);

        #endregion

        #region Fields

        private const string PART_Min = "PART_Min";

        private const string PART_Max = "PART_Max";

        private const string PART_Close = "PART_Close";

        private ButtonBase min;

        private ButtonBase max;

        private ButtonBase close;

#pragma warning disable 618
        private SafeLibraryHandle user32;
#pragma warning restore 618

        #endregion

        #region Properties

        public static readonly DependencyProperty MinimizeProperty =
            DependencyProperty.Register("Minimize", typeof(string), typeof(WindowButtons), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the minimize button tooltip.
        /// </summary>
        public string Minimize
        {
            get { return (string)GetValue(MinimizeProperty); }
            set { SetValue(MinimizeProperty, value); }
        }

        public static readonly DependencyProperty MaximizeProperty =
            DependencyProperty.Register("Maximize", typeof(string), typeof(WindowButtons), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the maximize button tooltip.
        /// </summary>
        public string Maximize
        {
            get { return (string)GetValue(MaximizeProperty); }
            set { SetValue(MaximizeProperty, value); }
        }

        public static readonly DependencyProperty RestoreProperty =
            DependencyProperty.Register("Restore", typeof(string), typeof(WindowButtons), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the restore button tooltip.
        /// </summary>
        public string Restore
        {
            get { return (string)GetValue(RestoreProperty); }
            set { SetValue(RestoreProperty, value); }
        }

        public static readonly DependencyProperty CloseProperty =
            DependencyProperty.Register("Close", typeof(string), typeof(WindowButtons), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the close button tooltip.
        /// </summary>
        public string Close
        {
            get { return (string)GetValue(CloseProperty); }
            set { SetValue(CloseProperty, value); }
        }

        public static readonly DependencyProperty MinimizeStyleProperty =
            DependencyProperty.Register("MinimizeStyle", typeof(Style), typeof(WindowButtons), new PropertyMetadata(null, OnStyleChanged));

        /// <summary>
        /// Gets or sets the style for the minimize button.
        /// </summary>
        public Style MinimizeStyle
        {
            get { return (Style)GetValue(MinimizeStyleProperty); }
            set { SetValue(MinimizeStyleProperty, value); }
        }

        public static readonly DependencyProperty MaximizeStyleProperty =
            DependencyProperty.Register("MaximizeStyle", typeof(Style), typeof(WindowButtons), new PropertyMetadata(null, OnStyleChanged));

        /// <summary>
        /// Gets or sets the style for the maximize button.
        /// </summary>
        public Style MaximizeStyle
        {
            get { return (Style)GetValue(MaximizeStyleProperty); }
            set { SetValue(MaximizeStyleProperty, value); }
        }

        public static readonly DependencyProperty CloseStyleProperty =
            DependencyProperty.Register("CloseStyle", typeof(Style), typeof(WindowButtons), new PropertyMetadata(null, OnStyleChanged));

        /// <summary>
        /// Gets or sets the style for the close button.
        /// </summary>
        public Style CloseStyle
        {
            get { return (Style)GetValue(CloseStyleProperty); }
            set { SetValue(CloseStyleProperty, value); }
        }

        private static void OnStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue) return;
            (d as WindowButtons).ApplyStyle();
        }

        private Window _parentWindow;

        public Window ParentWindow
        {
            get { return _parentWindow; }
            set
            {
                if (Equals(_parentWindow, value))
                {
                    return;
                }

                _parentWindow = value;
                RaisePropertyChanged("ParentWindow");
            }
        }

        #endregion

        #region Constructors

        static WindowButtons()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowButtons), new FrameworkPropertyMetadata(typeof(WindowButtons)));
        }

        public WindowButtons()
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Loaded,
                                        new Action(() => {
                                            if (string.IsNullOrWhiteSpace(Minimize))
                                            {
                                                SetCurrentValue(MinimizeProperty, GetCaption(900));
                                            }
                                            if (string.IsNullOrWhiteSpace(Maximize))
                                            {
                                                SetCurrentValue(MaximizeProperty, GetCaption(901));
                                            }
                                            if (string.IsNullOrWhiteSpace(Close))
                                            {
                                                SetCurrentValue(CloseProperty, GetCaption(905));
                                            }
                                            if (string.IsNullOrWhiteSpace(Restore))
                                            {
                                                SetCurrentValue(RestoreProperty, GetCaption(903));
                                            }
                                        }));
        }

#pragma warning disable 618
        private string GetCaption(int id)
        {
            if (user32 == null)
            {
                user32 = UnsafeNativeMethods.LoadLibrary(Environment.SystemDirectory + "\\User32.dll");
            }

            var sb = new StringBuilder(256);
            UnsafeNativeMethods.LoadString(user32, (uint)id, sb, sb.Capacity);
            return sb.ToString().Replace("&", "");
        }
#pragma warning restore 618

        #endregion

        #region Overrides

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
  
            close = Template.FindName(PART_Close, this) as ButtonBase;

            if (close != null)
            {
                close.Click += OnClose;
            }

            max = Template.FindName(PART_Max, this) as ButtonBase;

            if (max != null)
            {
                max.Click += OnMaximize;
            }

            min = Template.FindName(PART_Min, this) as ButtonBase;

            if (min != null)
            {
                min.Click += OnMinimize;
            }

            ApplyStyle();
        }

        #endregion

        #region Methods

        private void ApplyStyle()
        {
            if (min != null)
            {
                min.Style = MinimizeStyle;
            }

            if (max != null)
            {
                max.Style = MaximizeStyle;
            }

            if (close != null)
            {
                close.Style = CloseStyle;
            }
        }

#pragma warning disable 618

        private void OnMinimize(object sender, RoutedEventArgs e)
        {
            if (ParentWindow != null)
            {
                SystemCommands.MinimizeWindow(ParentWindow);
            }
        }

        private void OnMaximize(object sender, RoutedEventArgs e)
        {
            if (null == ParentWindow) return;
            if (ParentWindow.WindowState == WindowState.Maximized)
            {
                SystemCommands.RestoreWindow(ParentWindow);
            }
            else
            {
                SystemCommands.MaximizeWindow(ParentWindow);
            }
        }
#pragma warning restore 618

        private void OnClose(object sender, RoutedEventArgs e)
        {
            var closingWindowEventHandlerArgs = new ClosingWindowEventHandlerArgs();
            OnClosingWindow(closingWindowEventHandlerArgs);

            if (closingWindowEventHandlerArgs.Cancelled) return;
            ParentWindow?.Close();
        }

        protected void OnClosingWindow(ClosingWindowEventHandlerArgs args)
        {
            ClosingWindow?.Invoke(this, args);
        }

        #endregion
    }
}
