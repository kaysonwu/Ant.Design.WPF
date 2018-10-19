using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace AntdDemo.ViewModels
{
    [Export(typeof(IScreen))]
    internal class TagViewModel : Screen
    {
        public TagViewModel()
        {
            DisplayName = "Tag";
        }
    }
}
