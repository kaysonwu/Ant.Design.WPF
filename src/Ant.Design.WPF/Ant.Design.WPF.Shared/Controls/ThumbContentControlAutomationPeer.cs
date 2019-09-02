namespace Antd.Controls
{
    using System.Windows;
    using System.Windows.Automation.Peers;

    public class ThumbContentControlAutomationPeer : FrameworkElementAutomationPeer
    {
        public ThumbContentControlAutomationPeer(FrameworkElement owner)
            : base(owner)
        {
        }

        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Custom;
        }

        protected override string GetClassNameCore()
        {
            return "ThumbContentControl";
        }
    }
}
