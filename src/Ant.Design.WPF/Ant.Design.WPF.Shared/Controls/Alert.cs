using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Antd.Controls
{
    /// <summary>
    /// Alert component for feedback.
    /// </summary>
    [TemplatePart(Name = PART_Icon, Type = typeof(ContentPresenter))]
    public class Alert : Control
    {
        #region Fields

        private const string PART_Icon = "PART_Icon";

        private ContentPresenter icon;

        #endregion

        #region Properties

        public static readonly DependencyProperty BannerProperty =
            DependencyProperty.Register("Banner", typeof(bool), typeof(Alert), new PropertyMetadata(false));

        /// <summary>
        /// Gets/sets Whether to show as banner
        /// </summary>
        public bool Banner
        {
            get { return (bool)GetValue(BannerProperty); }
            set { SetValue(BannerProperty, value); }
        }

        public static readonly DependencyProperty ClosableProperty =
            DependencyProperty.Register("Closable", typeof(bool), typeof(Alert), new PropertyMetadata(false));

        /// <summary>
        /// Gets/sets whether Alert can be closed
        /// </summary>
        public bool Closable
        {
            get { return (bool)GetValue(ClosableProperty); }
            set { SetValue(ClosableProperty, value); }
        }

        public static readonly DependencyProperty CloseTextProperty =
            DependencyProperty.Register("CloseText", typeof(object), typeof(Alert), new PropertyMetadata(null));

        /// <summary>
        /// Gets/sets close text to show
        /// </summary>
        public object CloseText
        {
            get { return GetValue(CloseTextProperty); }
            set { SetValue(CloseTextProperty, value); }
        }

        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(object), typeof(Alert), new PropertyMetadata(null, OnTypeChanged));

        /// <summary>
        /// Gets/sets additional content of Alert
        /// </summary>
        public object Description
        {
            get { return GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(object), typeof(Alert), new PropertyMetadata(null, OnIconChanged));

        /// <summary>
        /// Gets/sets custom icon, effective when showIcon is true
        /// </summary>
        public object Icon
        {
            get { return GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Alert).ApplyIcon(true);
        }

        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(object), typeof(Alert), new PropertyMetadata(null));

        /// <summary>
        /// Gets/sets content of Alert
        /// </summary>
        public object Message
        {
            get { return GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public static readonly DependencyProperty ShowIconProperty =
            DependencyProperty.Register("ShowIcon", typeof(bool), typeof(Alert), new PropertyMetadata(false, OnShowIcon));

        /// <summary>
        /// Gets/sets whether to show icon
        /// </summary>
        public bool ShowIcon
        {
            get { return (bool)GetValue(ShowIconProperty); }
            set { SetValue(ShowIconProperty, value); }
        }

        private static void OnShowIcon(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Alert).SetIconVisibility();
        }

        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(AlertType), typeof(Alert), new PropertyMetadata(AlertType.Info, OnTypeChanged));

        /// <summary>
        /// Gets/sets type of Alert styles
        /// </summary>
        public AlertType Type
        {
            get { return (AlertType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        private static void OnTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Alert).ApplyIcon();
        }

        public static readonly DependencyProperty IconBrushProperty =
            DependencyProperty.Register("IconBrush", typeof(Brush), typeof(Alert), new PropertyMetadata(null));

        /// <summary>
        /// Gets/sets icon brush
        /// </summary>
        public Brush IconBrush
        {
            get { return (Brush)GetValue(IconBrushProperty); }
            set { SetValue(IconBrushProperty, value); }
        }

        #endregion

        #region Constructors

        static Alert()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Alert), new FrameworkPropertyMetadata(typeof(Alert)));
        }

        #endregion

        #region Overrides

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            icon = GetTemplateChild(PART_Icon) as ContentPresenter;

            SetIconVisibility();
            ApplyIcon(true);
        }

        private void ApplyIcon(bool force = false)
        {
            if (icon == null || !force && Icon != null) return;

            if (Icon is UIElement)
            {
                icon.Content = Icon;
                return;
            }

            string type;

            if (Icon is string)
            {
                type = Icon as string;

            } else
            {
                switch (Type)
                {
                    case AlertType.Success:
                        type = "check-circle";
                        break;
                    case AlertType.Info:
                        type = "info-circle";
                        break;
                    case AlertType.Warning:
                        type = "close-circle";
                        break;
                    case AlertType.Error:
                        type = "exclamation-circle";
                        break;
                    default:
                        type = "default";
                        break;
                }

                if (Description != null)
                {
                    type += "-o";
                }
            }

            icon.Content = new Icon() { Type = type };
        }

        private void SetIconVisibility()
        {
            if (icon == null) return;

            icon.Visibility = ShowIcon ? Visibility.Visible : Visibility.Collapsed;
        }

        #endregion

    }

    public enum AlertType : byte
    {
        Success, Info, Warning, Error 
    }
}
