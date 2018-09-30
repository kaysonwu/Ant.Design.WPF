using System.Collections.Generic;
using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace AntdDemo.ViewModels
{
    [Export(typeof(IShell))]
    internal class ShellViewModel : Conductor<IScreen>.Collection.OneActive
    {
        [ImportingConstructor]
        protected ShellViewModel([ImportMany] IEnumerable<IScreen> screens)
        {
            Items.AddRange(screens);
        }
    }
}
