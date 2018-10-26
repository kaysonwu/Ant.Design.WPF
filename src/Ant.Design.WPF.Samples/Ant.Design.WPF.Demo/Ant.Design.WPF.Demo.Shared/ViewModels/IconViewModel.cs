using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace AntdDemo.ViewModels
{
    [Export(typeof(IScreen))]
    public class IconViewModel : Screen
    {
        public IconViewModel()
        {
            DisplayName = "Icon";
        }
    }
}
