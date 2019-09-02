namespace Antd.Controls
{
    using System.Windows;
    using TabControlBase = System.Windows.Controls.TabControl;

    public class TabControl : TabControlBase
    {
        #region Constructors

        static TabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TabControl), new FrameworkPropertyMetadata(typeof(TabControl)));
        }

        #endregion
    }
}
