using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Caliburn.Micro;

namespace AntdDemo.ViewModels
{
    [Export(typeof(IShell))]
    internal class ShellViewModel : Conductor<IScreen>.Collection.OneActive
    {
        private bool useNoneWindowStyle = false;

        public bool UseNoneWindowStyle
        {
            get => useNoneWindowStyle;
            set
            {
                useNoneWindowStyle = value;
                NotifyOfPropertyChange();
            }
        }

        private bool loading = false;

        public bool Loading
        {
            get => loading;
            set
            {
                loading = value;
                NotifyOfPropertyChange();
            }
        }

        public void ClickMe()
        {
            Loading = true;
        }

        public void Change()
        {

            UseNoneWindowStyle = false;
         //   var app = Application.Current;
         //   //var brush = app.FindResource("btn-danger-color") as SolidColorBrush;

         ////   var a = brush.Clone();
         //   var color = Color.FromRgb(0xf4, 0x72, 0xd0);

         //   app.Resources["btn-danger-color"] = new SolidColorBrush(color);
          //  app.Resources.Remove("btn-danger-color");
          //  app.Resources.Add("btn-danger-color", a);
        }
    }
}
