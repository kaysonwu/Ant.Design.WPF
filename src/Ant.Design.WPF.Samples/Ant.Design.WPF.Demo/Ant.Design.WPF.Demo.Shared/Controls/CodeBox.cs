using System.Windows;
using System.Windows.Controls;

namespace AntdDemo.Controls
{
    internal class CodeBox : ContentControl
    {
        #region Properties

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(CodeBox), new PropertyMetadata(string.Empty));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }


        public static readonly DependencyProperty TitleTemplateProperty =
            DependencyProperty.Register("TitleTemplate", typeof(DataTemplate), typeof(CodeBox), new PropertyMetadata(null));

        public DataTemplate TitleTemplate
        {
            get { return (DataTemplate)GetValue(TitleTemplateProperty); }
            set { SetValue(TitleTemplateProperty, value); }
        }

        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(CodeBox), new PropertyMetadata(string.Empty));

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public static readonly DependencyProperty DescriptionTemplateProperty =
            DependencyProperty.Register("DescriptionTemplate", typeof(DataTemplate), typeof(CodeBox), new PropertyMetadata(null));

        public DataTemplate DescriptionTemplate
        {
            get { return (DataTemplate)GetValue(DescriptionTemplateProperty); }
            set { SetValue(DescriptionTemplateProperty, value); }
        }

        public static readonly DependencyProperty CodeProperty =
            DependencyProperty.Register("Code", typeof(string), typeof(CodeBox), new PropertyMetadata(string.Empty));

        public string Code
        {
            get { return (string)GetValue(CodeProperty); }
            set { SetValue(CodeProperty, value); }
        }

        public static readonly DependencyProperty CodeTemplateProperty =
            DependencyProperty.Register("CodeTemplate", typeof(DataTemplate), typeof(CodeBox), new PropertyMetadata(null));

        public DataTemplate CodeTemplate
        {
            get { return (DataTemplate)GetValue(CodeTemplateProperty); }
            set { SetValue(CodeTemplateProperty, value); }
        }

        #endregion

        #region Constructors

        static CodeBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CodeBox), new FrameworkPropertyMetadata(typeof(CodeBox)));
        }

        #endregion
    }
}
