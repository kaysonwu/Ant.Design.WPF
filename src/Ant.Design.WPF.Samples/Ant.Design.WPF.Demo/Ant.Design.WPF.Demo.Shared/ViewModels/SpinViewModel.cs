using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace AntdDemo.ViewModels
{
    [Export(typeof(IScreen))]
    internal class SpinViewModel : Screen
    {
        public SpinViewModel()
        {
            DisplayName = "Spin";
        }
    }
}
