using System.Windows;
using System.Windows.Interactivity;

namespace Antd.Behaviors
{
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
