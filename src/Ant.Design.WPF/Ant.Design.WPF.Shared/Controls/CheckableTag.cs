using System.Windows;
using System.Windows.Controls.Primitives;

namespace Antd.Controls
{
    /// <summary>
    /// CheckableTag works like Checkbox, click it to toggle checked.
    /// </summary>
    public class CheckableTag : ToggleButton
    {
        static CheckableTag()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CheckableTag), new FrameworkPropertyMetadata(typeof(CheckableTag)));
        }
    }
}
