namespace Antd.Controls
{
    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Automation.Peers;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;

    public class ThumbContentControl : ContentControl, IThumb
    {
        #region Events

        public static readonly RoutedEvent DragStartedEvent
            = EventManager.RegisterRoutedEvent("DragStarted", RoutingStrategy.Bubble, typeof(DragStartedEventHandler), typeof(ThumbContentControl));

        /// <summary>
        /// Adds or remove a DragStartedEvent handler
        /// </summary>
        public event DragStartedEventHandler DragStarted
        {
            add { AddHandler(DragStartedEvent, value); }
            remove { RemoveHandler(DragStartedEvent, value); }
        }

        public static readonly RoutedEvent DragDeltaEvent
            = EventManager.RegisterRoutedEvent("DragDelta", RoutingStrategy.Bubble, typeof(DragDeltaEventHandler), typeof(ThumbContentControl));

        /// <summary>
        /// Adds or remove a DragDeltaEvent handler
        /// </summary>
        public event DragDeltaEventHandler DragDelta
        {
            add { AddHandler(DragDeltaEvent, value); }
            remove { RemoveHandler(DragDeltaEvent, value); }
        }

        public static readonly RoutedEvent DragCompletedEvent
            = EventManager.RegisterRoutedEvent("DragCompleted", RoutingStrategy.Bubble, typeof(DragCompletedEventHandler), typeof(ThumbContentControl));

        /// <summary>
        /// Adds or remove a DragCompletedEvent handler
        /// </summary>
        public event DragCompletedEventHandler DragCompleted
        {
            add { AddHandler(DragCompletedEvent, value); }
            remove { RemoveHandler(DragCompletedEvent, value); }
        }

        #endregion

        #region Fields

        private TouchDevice currentDevice = null;

        private Point startDragPoint;

        private Point startDragScreenPoint;

        private Point oldDragScreenPoint;

        #endregion

        #region Properties

        public static readonly DependencyPropertyKey IsDraggingPropertyKey
            = DependencyProperty.RegisterReadOnly("IsDragging", typeof(bool), typeof(ThumbContentControl), new FrameworkPropertyMetadata(default(bool)));

        /// <summary>
        /// DependencyProperty for the IsDragging property.
        /// </summary>
        public static readonly DependencyProperty IsDraggingProperty = IsDraggingPropertyKey.DependencyProperty;

        /// <summary>
        /// Indicates that the left mouse button is pressed and is over the MetroThumbContentControl.
        /// </summary>
        public bool IsDragging
        {
            get { return (bool)GetValue(IsDraggingProperty); }
            protected set { SetValue(IsDraggingPropertyKey, value); }
        }

        #endregion

        #region Constructors

        static ThumbContentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ThumbContentControl), new FrameworkPropertyMetadata(typeof(ThumbContentControl)));
            FocusableProperty.OverrideMetadata(typeof(ThumbContentControl), new FrameworkPropertyMetadata(default(bool)));
            EventManager.RegisterClassHandler(typeof(ThumbContentControl), Mouse.LostMouseCaptureEvent, new MouseEventHandler(OnLostMouseCapture));
        }

        #endregion

        #region Overrides

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (!IsDragging)
            {
                e.Handled = true;
                try
                {
                    // focus me
                    Focus();
                    // now capture the mouse for the drag action
                    CaptureMouse();
                    // so now we are in dragging mode
                    SetValue(IsDraggingPropertyKey, true);
                    // get the mouse points
                    startDragPoint = e.GetPosition(this);
                    oldDragScreenPoint = startDragScreenPoint = PointToScreen(startDragPoint);

                    RaiseEvent(new ThumbContentControlDragStartedEventArgs(startDragPoint.X, startDragPoint.Y));
                }
                catch (Exception exception)
                {
                    Trace.TraceError($"{this}: Something went wrong here: {exception} {Environment.NewLine} {exception.StackTrace}");
                    CancelDragAction();
                }
            }

            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if (IsMouseCaptured && IsDragging)
            {
                e.Handled = true;
                // now we are in normal mode
                ClearValue(IsDraggingPropertyKey);
                // release the captured mouse
                ReleaseMouseCapture();
                // get the current mouse position and call the completed event with the horizontal/vertical change
                Point currentMouseScreenPoint = PointToScreen(e.MouseDevice.GetPosition(this));
                var horizontalChange = currentMouseScreenPoint.X - startDragScreenPoint.X;
                var verticalChange = currentMouseScreenPoint.Y - startDragScreenPoint.Y;
                RaiseEvent(new ThumbContentControlDragCompletedEventArgs(horizontalChange, verticalChange, false));
            }

            base.OnMouseLeftButtonUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (!IsDragging) return;
           
            if (e.MouseDevice.LeftButton == MouseButtonState.Pressed)
            {
                Point currentDragPoint = e.GetPosition(this);
                // Get client point and convert it to screen point
                Point currentDragScreenPoint = PointToScreen(currentDragPoint);
                if (currentDragScreenPoint != oldDragScreenPoint)
                {
                    oldDragScreenPoint = currentDragScreenPoint;
                    e.Handled = true;
                    var horizontalChange = currentDragPoint.X - startDragPoint.X;
                    var verticalChange = currentDragPoint.Y - startDragPoint.Y;
                    RaiseEvent(new DragDeltaEventArgs(horizontalChange, verticalChange) { RoutedEvent = ThumbContentControl.DragDeltaEvent });
                }
            }
            else
            {
                // clear some saved stuff
                if (e.MouseDevice.Captured == this)
                {
                    ReleaseMouseCapture();
                }

                ClearValue(IsDraggingPropertyKey);
                startDragPoint.X = 0;
                startDragPoint.Y = 0;
            }
        }

        protected override void OnPreviewTouchDown(TouchEventArgs e)
        {
            // Release any previous capture
            ReleaseCurrentDevice();
            // Capture the new touch
            CaptureCurrentDevice(e);
        }

        protected override void OnPreviewTouchUp(TouchEventArgs e)
        {
            ReleaseCurrentDevice();
        }

        protected override void OnLostTouchCapture(TouchEventArgs e)
        {
            // Only re-capture if the reference is not null
            // This way we avoid re-capturing after calling ReleaseCurrentDevice()
            if (currentDevice != null)
            {
                CaptureCurrentDevice(e);
            }
        }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new ThumbContentControlAutomationPeer(this);
        }

        #endregion

        #region Methods

        private static void OnLostMouseCapture(object sender, MouseEventArgs e)
        {
            // Cancel the drag action if we lost capture
            var thumb = (ThumbContentControl)sender;
            if (Mouse.Captured != thumb)
            {
                thumb.CancelDragAction();
            }
        }

        private void ReleaseCurrentDevice()
        {
            if (currentDevice != null)
            {
                // Set the reference to null so that we don't re-capture in the OnLostTouchCapture() method
                var temp = currentDevice;
                currentDevice = null;
                ReleaseTouchCapture(temp);
            }
        }

        private void CaptureCurrentDevice(TouchEventArgs e)
        {
            bool gotTouch = CaptureTouch(e.TouchDevice);
            if (gotTouch)
            {
                currentDevice = e.TouchDevice;
            }
        }

        public void CancelDragAction()
        {
            if (!IsDragging) return;

            if (IsMouseCaptured)
            {
                ReleaseMouseCapture();
            }

            ClearValue(IsDraggingPropertyKey);
            var horizontalChange = oldDragScreenPoint.X - startDragScreenPoint.X;
            var verticalChange = oldDragScreenPoint.Y - startDragScreenPoint.Y;
            RaiseEvent(new ThumbContentControlDragCompletedEventArgs(horizontalChange, verticalChange, true));
        }

        #endregion
    }
}
