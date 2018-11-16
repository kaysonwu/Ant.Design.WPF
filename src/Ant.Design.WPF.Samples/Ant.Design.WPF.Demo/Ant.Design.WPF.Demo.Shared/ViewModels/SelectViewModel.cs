using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace AntdDemo.ViewModels
{
    [Export(typeof(IScreen))]
    internal class SelectViewModel : Screen
    {
        public SelectViewModel()
        {
            DisplayName = "Select";
        }
    }
}
