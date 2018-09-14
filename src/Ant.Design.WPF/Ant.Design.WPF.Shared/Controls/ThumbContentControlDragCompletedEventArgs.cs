using System.Windows.Controls.Primitives;

namespace Antd.Controls
{
    public class ThumbContentControlDragCompletedEventArgs : DragCompletedEventArgs
    {
        public ThumbContentControlDragCompletedEventArgs(double horizontalOffset, double verticalOffset, bool canceled)
            : base(horizontalOffset, verticalOffset, canceled)
        {
            this.RoutedEvent = ThumbContentControl.DragCompletedEvent;
        }
    }
}
