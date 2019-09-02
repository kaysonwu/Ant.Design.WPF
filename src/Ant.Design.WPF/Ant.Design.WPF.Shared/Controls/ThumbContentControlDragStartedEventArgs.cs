namespace Antd.Controls
{
    using System.Windows.Controls.Primitives;

    public class ThumbContentControlDragStartedEventArgs : DragStartedEventArgs
    {
        public ThumbContentControlDragStartedEventArgs(double horizontalOffset, double verticalOffset)
            : base(horizontalOffset, verticalOffset)
        {
            this.RoutedEvent = ThumbContentControl.DragStartedEvent;
        }
    }
}
