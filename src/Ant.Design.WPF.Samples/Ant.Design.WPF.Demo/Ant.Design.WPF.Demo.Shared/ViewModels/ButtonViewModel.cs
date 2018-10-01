using System.ComponentModel.Composition;
using Antd.Controls;
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
        }

        public void Click2()
        {
            Loading2 = true;
        }
    }
}
