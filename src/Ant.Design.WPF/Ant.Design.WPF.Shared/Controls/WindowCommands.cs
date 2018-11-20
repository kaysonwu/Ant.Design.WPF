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

        public static readonly DependencyProperty ShowSeparatorsProperty = 
            DependencyProperty.Register("ShowSeparators", typeof(bool), typeof(WindowCommands),
                new FrameworkPropertyMetadata(
                    true, 
                    FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, 
                    OnShowSeparatorsChanged
                ));

        /// <summary>
        /// Gets or sets the value indicating whether to show the separators.
        /// </summary>
        public bool ShowSeparators
        {
            get { return (bool)GetValue(ShowSeparatorsProperty); }
            set { SetValue(ShowSeparatorsProperty, value); }
        }

        private static void OnShowSeparatorsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue)
            {
                return;
            }
            // ((WindowCommands)d).ResetSeparators();
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
