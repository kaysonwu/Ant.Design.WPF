using System.ComponentModel.Composition;
using System.Windows;
using Caliburn.Micro;

namespace AntdDemo.ViewModels
{
    [Export(typeof(IScreen))]
    internal class TagViewModel : Screen
    {
        private Visibility visibility;

        public Visibility Visibility
        {
            get { return visibility; }
            set {
                visibility = value;
                NotifyOfPropertyChange();
            }
        }


        public TagViewModel()
        {
            DisplayName = "Tag";
        }

        public void Closing(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
        }

        public void Toggle()
        {
            Visibility = visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
