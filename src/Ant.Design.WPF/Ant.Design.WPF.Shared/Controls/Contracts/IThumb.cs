using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Antd.Controls
{
    public interface IThumb : IInputElement
    {
        event DragStartedEventHandler DragStarted;

        event DragDeltaEventHandler DragDelta;

        event DragCompletedEventHandler DragCompleted;

        event MouseButtonEventHandler MouseDoubleClick;
    }
}
