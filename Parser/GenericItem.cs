using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parser
{
    [Serializable]
    public class GenericItem : InterfaceItem
    {
        public GenericItem()
            : this(new List<string>())
        { }
        public GenericItem(List<string> interface_item_string)
            : base(interface_item_string)
        {
            AddToGlobalModuleGenerics();
        }
        private void AddToGlobalModuleGenerics()
        {
            foreach (string id in base.id_list)
            {
                if (base.value != 0)
                    Globals.KnownGenerics.Add(new KeyValuePair<string, int>(id, base.value));
            }
        }
    }

}
