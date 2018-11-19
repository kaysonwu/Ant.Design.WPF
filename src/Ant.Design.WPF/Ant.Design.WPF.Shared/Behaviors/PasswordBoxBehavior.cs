using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Interactivity;
using Antd.Controls;

namespace Antd.Behaviors
{
    [TemplatePart(Name = PART_TextBox, Type = typeof(TextBox))]
    [TemplatePart(Name = PART_Eye, Type = typeof(ToggleButton))]
    public class PasswordBoxBehavior : Behavior<PasswordBox>
    {
        #region Fields

        private const string PART_TextBox = "PART_TextBox";

        private const string PART_Eye = "PART_Eye";

        private static MethodInfo select;

        private static PropertyInfo selection;

        #endregion

        #region Private Attached Properties

        private static readonly DependencyProperty TextBoxProperty = 
            DependencyProperty.RegisterAttached("TextBox", typeof(TextBox), typeof(PasswordBoxBehavior), new PropertyMetadata(null));

        private static TextBox GetTextBox(DependencyObject obj)
        {
            return (TextBox)obj.GetValue(TextBoxProperty);
        }

        private static void SetTextBox(DependencyObject obj, TextBox value)
        {
            obj.SetValue(TextBoxProperty, value);
        }

        #endregion

        #region Constructors

        static PasswordBoxBehavior()
        {
            select = typeof(PasswordBox).GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic);
            selection = typeof(PasswordBox).GetProperty("Selection", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        #endregion

        #region Overrides

        protected override void OnAttached()
        {
            AssociatedObject.Loaded += OnLoaded;
            AssociatedObject.GotFocus += OnGotFocus;
            AssociatedObject.PasswordChanged += OnPasswordChanged;

            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= OnLoaded;
            AssociatedObject.GotFocus -= OnGotFocus;
            AssociatedObject.PasswordChanged -= OnPasswordChanged;

            if (Input.GetEyeable(AssociatedObject))
            {
                AssociatedObject.RemoveHandler(ButtonBase.ClickEvent, new RoutedEventHandler(OnEyeClick));
            }

            base.OnDetaching();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Get the insertion position index of the caret.
        /// </summary>
        private static int GetCaretIndex(TextSelection selection)
        {
            if (selection == null) return 0;

            var tTextRange = selection.GetType().GetInterfaces().FirstOrDefault(i => i.Name == "ITextRange");
            var oStart = tTextRange?.GetProperty("Start")?.GetGetMethod()?.Invoke(selection, null);
            var value = oStart?.GetType().GetProperty("Offset", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(oStart, null) as int?;

            return value.GetValueOrDefault(0);
        }

        /// <summary>
        /// Set the insertion position index of the caret.
        /// </summary>
        private static void SetCaretIndex(object obj, int index)
        {
            select.Invoke(obj, new object[] { index, 0 });
        }

        private static void OnLoaded(object sender, RoutedEventArgs e)
        {
            var passwordBox = (PasswordBox)sender;
            var password = passwordBox.Password;

            if (!string.IsNullOrEmpty(password))
            {
                Input.SetPassword(passwordBox, password);
                SetCaretIndex(passwordBox, password.Length);
            }

            if (Input.GetEyeable(passwordBox))
            {
                SetTextBox(passwordBox, passwordBox.FindChild<TextBox>(PART_TextBox));
                passwordBox.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(OnEyeClick));
            }
        }

        private static void OnEyeClick(object sender, RoutedEventArgs e)
        {
            var passwordBox = (PasswordBox)sender;
            TextBox textBox;

            if (!(e.OriginalSource is ToggleButton button) || button.Name != PART_Eye ||
                (textBox = GetTextBox(passwordBox)) == null)
            {
                return;
            }

            e.Handled = true;

            if (button.IsChecked.GetValueOrDefault())
            {
                textBox.CaretIndex = GetCaretIndex(selection?.GetValue(passwordBox, null) as TextSelection);
                textBox.Focus();
            } else
            {
                SetCaretIndex(passwordBox, textBox.CaretIndex);
                passwordBox.Focus();
            }
        }

        private static void OnGotFocus(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is PasswordBox passwordBox)
            {
                TextBox textBox;

                if (Input.GetEyeable(passwordBox) && 
                    (textBox = GetTextBox(passwordBox)) != null &&
                    textBox.Visibility == Visibility.Visible)
                {
                    textBox.Focus();
                }
            }
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
