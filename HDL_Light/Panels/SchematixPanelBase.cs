using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvalonDock.Layout;

namespace Schematix.Panels
{
    [Serializable]
    public class SchematixPanelBase : LayoutAnchorable
    {
        public virtual void OnActivateChild(Schematix.Windows.SchematixBaseWindow form)
        { }

        public virtual void OnClosePanel()
        { }
    }
}
