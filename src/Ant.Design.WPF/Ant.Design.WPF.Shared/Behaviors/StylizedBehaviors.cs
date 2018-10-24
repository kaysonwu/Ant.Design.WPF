using System.Windows;
using System.Windows.Interactivity;

namespace Antd.Behaviors
{
    public class StylizedBehaviors
    {
        #region Properties

        public static readonly DependencyProperty BehaviorsProperty = DependencyProperty.RegisterAttached(
            "Behaviors", 
            typeof(StylizedBehaviorCollection), 
            typeof(StylizedBehaviors), 
            new PropertyMetadata(null, OnBehaviorsChanged));

        public static StylizedBehaviorCollection GetBehaviors(DependencyObject obj)
        {
            return (StylizedBehaviorCollection)obj.GetValue(BehaviorsProperty);
        }

        public static void SetBehaviors(DependencyObject obj, StylizedBehaviorCollection value)
        {
            obj.SetValue(BehaviorsProperty, value);
        }

        public static readonly DependencyProperty OriginalBehaviorProperty = DependencyProperty.RegisterAttached(
            "OriginalBehavior", 
            typeof(Behavior), 
            typeof(StylizedBehaviors), 
            new UIPropertyMetadata(null));

        public static Behavior GetOriginalBehavior(DependencyObject obj)
        {
            return (Behavior)obj.GetValue(OriginalBehaviorProperty);
        }

        public static void SetOriginalBehavior(DependencyObject obj, Behavior value)
        {
            obj.SetValue(OriginalBehaviorProperty, value);
        }

        #endregion

        #region Private Methods

        private static void OnBehaviorsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fe = d as FrameworkElement;
            if (fe == null) return;

            var newVal = e.NewValue as StylizedBehaviorCollection;
            var oldVal = e.OldValue as StylizedBehaviorCollection;

            if (newVal == oldVal) return;

            var behaviors = Interaction.GetBehaviors(fe);
            fe.Unloaded  -= OnUnloaded;


            if (oldVal != null)
            {
                foreach (var behavior in oldVal)
                {
                    var index = GetIndexOf(behaviors, behavior);
                    if (index >= 0)
                    {
                        behaviors.RemoveAt(index);
                    }
                }
            }

            if (newVal != null)
            {
                foreach (var behavior in newVal)
                {
                    var index = GetIndexOf(behaviors, behavior);
                    if (index < 0)
                    {
                        var clone = (Behavior)behavior.Clone();
                        SetOriginalBehavior(clone, behavior);
                        behaviors.Add(clone);
                    }
                }
            }

            if (behaviors.Count > 0)
            {
                fe.Unloaded += OnUnloaded;
            }
        }

        private static void OnLoaded(object sender, RoutedEventArgs e)
        {
            var fe = sender as FrameworkElement;
            if (fe == null) return;

            fe.Loaded    -= OnLoaded;
            var behaviors = Interaction.GetBehaviors(fe);

            foreach (var behavior in behaviors)
            {
                behavior.Attach(fe);
            }
        }

        private static void OnUnloaded(object sender, RoutedEventArgs e)
        {
            var fe = sender as FrameworkElement;
            if (fe == null) return;

            var behaviors = Interaction.GetBehaviors(fe);
            foreach (var behavior in behaviors)
            {
                behavior.Detach();
            }

            fe.Loaded += OnLoaded;
        }

        private static int GetIndexOf(BehaviorCollection items, Behavior behavior)
        {
            int index = -1;
            var orignalBehavior = GetOriginalBehavior(behavior);

            for (int i = 0; i < items.Count; i++)
            {
                var currentBehavior = items[i];
                if (currentBehavior == behavior || currentBehavior == orignalBehavior)
                {
                    index = i;
                    break;
                }

                var currentOrignalBehavior = GetOriginalBehavior(currentBehavior);
                if (currentOrignalBehavior == behavior || currentOrignalBehavior == orignalBehavior)
                {
                    index = i;
                    break;
                }
            }

            return index;
        }

        #endregion
    }
}
