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

            Input.SetPassword(AssociatedObject, AssociatedObject.Password);
            AssociatedObject.PasswordChanged += OnPasswordChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.PasswordChanged -= OnPasswordChanged;
        }

        #endregion

        #region Private Methods

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            var box = (PasswordBox)sender;
            var password = box.Password;

            // Sync password
            if (password != Input.GetPassword(box))
            {
                Input.SetPassword(box, password);
            }
        }

        #endregion
    }
}
