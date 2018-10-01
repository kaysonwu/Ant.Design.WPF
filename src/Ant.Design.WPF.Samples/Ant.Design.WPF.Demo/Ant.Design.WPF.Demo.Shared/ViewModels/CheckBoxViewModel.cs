using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace AntdDemo.ViewModels
{
    [Export(typeof(IScreen))]
    internal class CheckBoxViewModel : Screen
    {
        public CheckBoxViewModel()
        {
            DisplayName = "CheckBox";
        }
    }
}
