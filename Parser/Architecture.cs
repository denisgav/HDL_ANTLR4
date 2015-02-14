using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parser
{
    public class Architecture
    {
        public string name;
        public string entity_name;
        public Architecture(string name, string entity_name)
        {
            this.name = name;
            this.entity_name = entity_name;
            architecture_items = new List<ArchitectureBlockDeclarativeItem>();
        }
        public void AddArchitectureItem(ArchitectureBlockDeclarativeItem it)
        {
            architecture_items.Add(it);
        }
        public List<ArchitectureBlockDeclarativeItem> architecture_items;
    }
}
