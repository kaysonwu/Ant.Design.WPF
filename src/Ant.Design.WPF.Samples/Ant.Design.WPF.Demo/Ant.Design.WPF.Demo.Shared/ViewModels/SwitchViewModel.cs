using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

namespace AntdDemo.ViewModels
{
    [Export(typeof(IScreen))]
    internal class SwitchViewModel : Screen
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


        public SwitchViewModel()
        {
            DisplayName = "Switch";
        }

        public void Toggle()
        {
            IsEnabled = !isEnabled;
        }
    }
}
