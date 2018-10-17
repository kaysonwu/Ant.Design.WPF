using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace AntdDemo.ViewModels
{
    [Export(typeof(IScreen))]
    internal class ToolTipViewModel : Screen
    {
        public ToolTipViewModel()
        {
            DisplayName = "ToolTip";
        }
    }
}
