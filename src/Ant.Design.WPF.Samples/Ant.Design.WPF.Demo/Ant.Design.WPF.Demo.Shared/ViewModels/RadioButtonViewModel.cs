using System.Collections.Generic;
using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace AntdDemo.ViewModels
{
    [Export(typeof(IScreen))]
    public class RadioButtonViewModel : Screen
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


        public IEnumerable<RadioItem> Items { get; set; }

        public RadioButtonViewModel()
        {
            DisplayName = "Radio";

          
            Items = new List<RadioItem>
            {
                new RadioItem { Label = "Apple", Value = "Apple" },
                new RadioItem { Label = "Pear", Value = "Pear" },
                new RadioItem { Label = "Orange", Value = "Orange" }
            };
        }

        public void Toggle()
        {
            IsEnabled = !isEnabled;
        }
    }

    public struct RadioItem
    {
        public string Label;

        public string Value;
    }
}
