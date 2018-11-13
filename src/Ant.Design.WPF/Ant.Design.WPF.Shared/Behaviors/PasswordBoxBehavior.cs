using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using Antd.Controls;

namespace Antd.Behaviors
{
    public class PasswordBoxBehavior : Behavior<PasswordBox>
    {

        #region Overrides

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Loaded += OnLoaded;
            AssociatedObject.PasswordChanged += OnPasswordChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.Loaded -= OnLoaded;
            AssociatedObject.PasswordChanged -= OnPasswordChanged;
        }

        #endregion

        #region Private Methods

        private static void OnLoaded(object sender, RoutedEventArgs e)
        {
            var passwordBox = (PasswordBox)sender;

            passwordBox.FindChild<TextBox>()



            Input.SetPassword(passwordBox, passwordBox.Password);
        }

        private static void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = (PasswordBox)sender;
            var password = passwordBox.Password;

            // Sync password
            if (password != Input.GetPassword(passwordBox))
            {
                Input.SetPassword(passwordBox, password);
            }
        }

        #endregion
    }
}
