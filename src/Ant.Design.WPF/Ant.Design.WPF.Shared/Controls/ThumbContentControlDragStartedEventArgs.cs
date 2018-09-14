using System.Windows.Controls.Primitives;

namespace Antd.Controls
{
    public class ThumbContentControlDragStartedEventArgs : DragStartedEventArgs
    {
        public ThumbContentControlDragStartedEventArgs(double horizontalOffset, double verticalOffset)
            : base(horizontalOffset, verticalOffset)
        {
            this.RoutedEvent = ThumbContentControl.DragStartedEvent;
        }
    }
}
