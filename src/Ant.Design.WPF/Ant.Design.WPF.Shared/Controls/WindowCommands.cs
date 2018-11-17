using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Antd.Controls
{
    public class WindowCommands : ItemsControl, INotifyPropertyChanged
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Properties

        private Window _parentWindow;

        public Window ParentWindow
        {
            get { return _parentWindow; }
            set
            {
                if (Equals(_parentWindow, value))
                {
                    return;
                }

                _parentWindow = value;
                RaisePropertyChanged("ParentWindow");
            }
        }

        #endregion

        #region Constructors

        static WindowCommands()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowCommands), new FrameworkPropertyMetadata(typeof(WindowCommands)));
        }

        #endregion

    }
}
