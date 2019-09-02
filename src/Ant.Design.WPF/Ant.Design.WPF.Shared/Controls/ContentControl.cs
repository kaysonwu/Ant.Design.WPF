namespace Antd.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using ContentControlBase = System.Windows.Controls.ContentControl;

    public class ContentControl : ContentControlBase
    {
        public static readonly DependencyProperty ContentCharacterCasingProperty =
            DependencyProperty.Register("ContentCharacterCasing", typeof(CharacterCasing), typeof(ContentControl),
                new FrameworkPropertyMetadata(CharacterCasing.Normal, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsMeasure),
                value => CharacterCasing.Normal <= (CharacterCasing)value && (CharacterCasing)value <= CharacterCasing.Upper);

        /// <summary>
        /// Character casing of the Content
        /// </summary>
        public CharacterCasing ContentCharacterCasing
        {
            get { return (CharacterCasing)GetValue(ContentCharacterCasingProperty); }
            set { SetValue(ContentCharacterCasingProperty, value); }
        }

        public static readonly DependencyProperty RecognizesAccessKeyProperty =
            DependencyProperty.Register("RecognizesAccessKey", typeof(bool), typeof(ContentControl), new FrameworkPropertyMetadata(false));

        /// <summary>
        /// Determine if the inner ContentPresenter should use AccessText in its style
        /// </summary>
        public bool RecognizesAccessKey
        {
            get { return (bool)GetValue(RecognizesAccessKeyProperty); }
            set { SetValue(RecognizesAccessKeyProperty, value); }
        }

        static ContentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentControl), new FrameworkPropertyMetadata(typeof(ContentControl)));
        }
    }
}
