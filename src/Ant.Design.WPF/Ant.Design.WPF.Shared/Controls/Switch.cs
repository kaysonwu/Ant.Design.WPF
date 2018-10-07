using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Antd.Controls
{
    public class Switch : ToggleButton
    {
        #region Properties

        public static readonly DependencyProperty LoadingProperty =
            DependencyProperty.Register("Loading", typeof(bool), typeof(Switch), new PropertyMetadata(false));

        /// <summary>
        /// Gets/sets loading state of switch
        /// </summary>
        public bool Loading
        {
            get { return (bool)GetValue(LoadingProperty); }
            set { SetValue(LoadingProperty, value); }
        }

        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register("Size", typeof(Size?), typeof(Switch), new PropertyMetadata(null));

        /// <summary>
        /// Gets/sets the size of the Switch
        /// </summary>
        public Size? Size
        {
            get { return (Size?)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        public static readonly DependencyProperty UnCheckedContentProperty =
            DependencyProperty.Register("UnCheckedContent", typeof(object), typeof(Switch), new PropertyMetadata(null));

        /// <summary>
        /// Gets/sets content to be shown when the state is unchecked
        /// </summary>
        public object UnCheckedContent
        {
            get { return GetValue(UnCheckedContentProperty); }
            set { SetValue(UnCheckedContentProperty, value); }
        }

        #endregion

        #region Switch

        static Switch()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Switch), new FrameworkPropertyMetadata(typeof(Switch)));
        }


        #endregion
    }
}
