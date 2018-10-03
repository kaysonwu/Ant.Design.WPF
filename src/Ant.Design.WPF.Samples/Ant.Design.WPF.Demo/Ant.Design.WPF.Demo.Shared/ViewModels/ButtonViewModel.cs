using System.Windows;
using System.Windows.Media;
using System.ComponentModel.Composition;
using Size = Antd.Controls.Size;
using Caliburn.Micro;

namespace AntdDemo.ViewModels
{
    [Export(typeof(IScreen))]
    internal class ButtonViewModel : Screen
    {
        public Size? Size { get; set; }

        private bool loading1;

        public bool Loading1
        {
            get { return loading1; }
            set
            {
                loading1 = value;
                NotifyOfPropertyChange();
            }
        }

        private bool loading2;

        public bool Loading2
        {
            get { return loading2; }
            set {
                loading2 = value;
                NotifyOfPropertyChange();
            }
        }

        public ButtonViewModel()
        {
            DisplayName = "Button";
        }

        public void Click1()
        {
            Loading1 = true;
            var app = Application.Current;

            app.Resources["PrimaryColor1"] = Color.FromRgb(0, 0, 255);
            //app.Resources["PrimaryColor"] = new SolidColorBrush(Color.FromRgb(255, 0, 0));
        }

        public void Click2()
        {
            Loading2 = true;
        }
    }
}
