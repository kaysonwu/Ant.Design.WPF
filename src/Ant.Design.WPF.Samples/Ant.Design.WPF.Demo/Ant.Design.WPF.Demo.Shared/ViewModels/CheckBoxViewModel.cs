using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace AntdDemo.ViewModels
{
    [Export(typeof(IScreen))]
    internal class CheckBoxViewModel : Screen
    {
        private bool isChecked;

        public bool IsChecked
        {
            get { return isChecked; }
            set {
                isChecked = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange("Check");
                NotifyOfPropertyChange("CheckBox");
            }
        }

        private bool isEnabled;

        public bool IsEnabled
        {
            get { return isEnabled; }
            set {
                isEnabled = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange("Enabled");
                NotifyOfPropertyChange("CheckBox");
            }
        }

        public string Check
        {
            get { return (isChecked ? "Uncheck" : "Check"); }
        }

        public string Enabled
        {
            get { return (isEnabled ? "Disabled" : "Enabled"); }
        }

        public string CheckBox {
            get { return (isChecked ? "Checked" : "Unchecked") + "-" + (isEnabled ? "Enabled" : "Disabled"); }
        }

        private bool apple;

        public bool Apple
        {
            get { return apple; }
            set {
                apple = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(nameof(CheckAll));
            }
        }

        private bool pear;

        public bool Pear
        {
            get { return pear; }
            set {
                pear = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(nameof(CheckAll));
            }
        }


        private bool orange;

        public bool Orange
        {
            get { return orange; }
            set {
                orange = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(nameof(CheckAll));

            }
        }

        public bool? CheckAll {
            get {
                
                if (apple && pear && orange)
                {
                    return true;
                }
 
                if (apple || pear || Orange)
                {
                    return null;
                }

                return false;
            }

            set
            {
                Apple = Pear = Orange = value.Value;
            }
        }

        public CheckBoxViewModel()
        {
            DisplayName = "CheckBox";
            ToggleCheck();
            ToggleEnabled();
        }

        public void ToggleCheck()
        {
            IsChecked = !isChecked;
        }

        public void ToggleEnabled()
        {
            IsEnabled = !isEnabled;
        }
    }
}
