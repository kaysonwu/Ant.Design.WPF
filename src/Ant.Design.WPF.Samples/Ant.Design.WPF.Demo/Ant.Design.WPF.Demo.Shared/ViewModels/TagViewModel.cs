using System.ComponentModel.Composition;
using System.Windows;
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

        public void Closing(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
        }
    }
}
