using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

namespace AntdDemo.ViewModels
{
    [Export(typeof(IScreen))]
    internal class RadioButtonViewModel : Screen
    {
        private bool isEnabled;

        public bool IsEnabled
        {
            get { return isEnabled; }
            set {
                isEnabled = value;
                NotifyOfPropertyChange();
            }
        }


        public RadioButtonViewModel()
        {
            DisplayName = "Radio";
        }

        public void Toggle()
        {
            IsEnabled = !isEnabled;
        }
    }
}
