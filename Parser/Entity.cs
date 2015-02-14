using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parser
{
    [Serializable]
    public class Entity
    {
        public List<GenericItem> Generic_items;
        public List<PortItem> Port_items;
        public string name;
        public string entityDeclarativePart;

        public Entity()
            :this(string.Empty)
        { }

        public Entity(string name)
        {
            this.name = name;
            Port_items = new List<PortItem>();
            Generic_items = new List<GenericItem>();
        }
        public void AddGenericItem(GenericItem it)
        {
            Generic_items.Add(it);
        }
        public void AddPortItem(PortItem it)
        {
            Port_items.Add(it);
        }
        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            for (int i = 0; i < entityDeclarativePart.Length; ++i)
            {
                res.Append(entityDeclarativePart[i]);
                if (entityDeclarativePart[i].Equals(';'))
                    res.Append("\r\n\t");
            }
            return res.ToString(); 
        }
    }
}
