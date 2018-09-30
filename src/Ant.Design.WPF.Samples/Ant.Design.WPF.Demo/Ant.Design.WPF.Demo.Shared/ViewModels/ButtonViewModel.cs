using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace AntdDemo.ViewModels
{
    [Export(typeof(IScreen))]
    internal class ButtonViewModel : Screen
    {
        public ButtonViewModel()
        {
            DisplayName = "Button";
        }
    }
}
