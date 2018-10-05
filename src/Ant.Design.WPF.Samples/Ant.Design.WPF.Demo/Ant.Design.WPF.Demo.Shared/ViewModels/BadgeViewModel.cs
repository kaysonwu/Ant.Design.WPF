using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace AntdDemo.ViewModels
{
    [Export(typeof(IScreen))]
    internal class BadgeViewModel : Screen
    {
        public BadgeViewModel()
        {
            DisplayName = " Badge";
        }
    }
}
