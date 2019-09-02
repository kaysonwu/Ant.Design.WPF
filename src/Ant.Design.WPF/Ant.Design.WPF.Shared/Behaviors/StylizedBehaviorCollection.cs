namespace Antd.Behaviors
{
    using System.Windows;
    using System.Windows.Interactivity;

    /// <summary>
    /// From https://github.com/MahApps/MahApps.Metro/tree/1.6.1/src/MahApps.Metro/Behaviours/StylizedBehaviorCollection.cs
    /// </summary>
    public class StylizedBehaviorCollection : FreezableCollection<Behavior>
    {
        protected override Freezable CreateInstanceCore()
        {
            return new StylizedBehaviorCollection();
        }
    }
}
