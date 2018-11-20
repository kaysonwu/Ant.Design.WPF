using System.ComponentModel;
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

        public static readonly DependencyProperty SeparatorHeightProperty =
            DependencyProperty.Register("SeparatorHeight", typeof(double), typeof(WindowCommands), new PropertyMetadata(double.NaN));

        /// <summary>
        /// Gets/sets the height of the separator.
        /// </summary>
        public double SeparatorHeight
        {
            get { return (double)GetValue(SeparatorHeightProperty); }
            set { SetValue(SeparatorHeightProperty, value); }
        }

        #endregion

        #region Constructors

        static WindowCommands()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowCommands), new FrameworkPropertyMetadata(typeof(WindowCommands)));
        }

        #endregion
    }

    public class WindowCommandsItem : ContentControl
    {
        static WindowCommandsItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowCommandsItem), new FrameworkPropertyMetadata(typeof(WindowCommandsItem)));
        }
    }

}
