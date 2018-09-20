using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using ButtonBase = System.Windows.Controls.Button;

namespace Antd.Controls
{
    public class Button : ButtonBase
    {
        #region Properties

        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(ButtonType?), typeof(Button), new PropertyMetadata(null));

        /// <summary>
        /// Gets/sets button type
        /// </summary>
        public ButtonType? Type
        {
            get { return (ButtonType?)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }
        
        public static readonly DependencyProperty HrefProperty =
            DependencyProperty.Register("Href", typeof(string), typeof(Button), new PropertyMetadata(null));

        /// <summary>
        /// Gets/sets click the button to jump to the url
        /// </summary>
        public string Href
        {
            get { return (string)GetValue(HrefProperty); }
            set { SetValue(HrefProperty, value); }
        }

        public static readonly DependencyProperty GhostProperty =
            DependencyProperty.Register("Ghost", typeof(bool), typeof(Button), new PropertyMetadata(false));

        /// <summary>
        ///  Gets/sets ghost property to make the button background transparent
        /// </summary>
        public bool Ghost
        {
            get { return (bool)GetValue(GhostProperty); }
            set { SetValue(GhostProperty, value); }
        }

        #endregion

        #region Constructors

        static Button()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Button), new FrameworkPropertyMetadata(typeof(Button)));
        }

        #endregion

        #region Overrides

        protected override void OnClick()
        {
            if (Href != null)
            {// TODO 使用默认浏览器打开地址
                return;
            }

            base.OnClick();
        }

        #endregion
    }

    public enum ButtonType
    {
        Primary, Dashed, Danger
    }
}
