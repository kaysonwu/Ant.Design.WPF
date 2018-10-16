using System.ComponentModel.Composition;
using System.Windows.Media;
using Caliburn.Micro;

namespace AntdDemo.ViewModels
{
    [Export(typeof(IScreen))]
    internal class AvatarViewModel : Screen
    {
        private string[] users = { "U", "Lucy", "Tom", "Edward" };

        private string[] colors = { "#f56a00", "#7265e6", "#ffbf00", "#00a2ae" };

        private int index = 0;

        private Brush background;

        public Brush Background
        {
            get { return background; }
            set {
                background = value;
                NotifyOfPropertyChange();
            }
        }

        private string text;

        public string Text
        {
            get { return text; }
            set {
                text = value;
                NotifyOfPropertyChange();
            }
        }

        public AvatarViewModel()
        {
            DisplayName = "Avatar";
            Change();
        }

        public void Change()
        {
            if (index >= users.Length)
            {
                index = 0;
            }
            
            Text = users[index];
            Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colors[index]));

            index++;
        }
    }
}
