using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parser
{
    [Serializable]
    public class PortItem : InterfaceItem
    {
        public PortItem() 
            : this(new List<string>())
        { }
        public PortItem(List<string> interface_item_string)
            : base(interface_item_string)
        {
        }
    }

}
