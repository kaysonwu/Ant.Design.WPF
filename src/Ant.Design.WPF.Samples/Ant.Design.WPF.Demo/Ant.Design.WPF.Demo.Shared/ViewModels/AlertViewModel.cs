using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace AntdDemo.ViewModels
{
    [Export(typeof(IScreen))]
    internal class AlertViewModel : Screen
    {
        public AlertViewModel()
        {
            DisplayName = "Alert";
        }
    }
}
