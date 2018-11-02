using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Antd.Controls
{
    public class RadioButtonGroup : ItemsControl
    {
        #region Properties

        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register("Size", typeof(Sizes?), typeof(RadioButtonGroup), new PropertyMetadata(null));

        /// <summary>
        /// Gets/sets size for radio button style
        /// </summary>
        public Sizes? Size
        {
            get { return (Sizes?)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(RadioButtonGroup), new PropertyMetadata(Orientation.Horizontal));


        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        #endregion

        #region Constructors

        static RadioButtonGroup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RadioButtonGroup), new FrameworkPropertyMetadata(typeof(RadioButtonGroup)));
        }

        #endregion

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

        }
    }

    public class RadioButtonGroupTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return base.SelectTemplate(item, container);
        }
    }
}
