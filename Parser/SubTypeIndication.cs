using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parser
{
    [Serializable]
    public class SubTypeIndication
    {
        public string sub_type_indication_string;
        public bool hasRange;
        public Range range;

        public SubTypeIndication() :
            this(new List<string>())
        { }

        public SubTypeIndication(List<string> interface_item_string)
        {
            this.sub_type_indication_string = interface_item_string.ElementAt(0);
            Globals.DeleteBeforeWord(interface_item_string.ElementAt(0), ref interface_item_string);
            hasRange = interface_item_string.Count != 0 && interface_item_string.First().Equals("(");
            if (hasRange)
            {
                range = new Range(ref interface_item_string);
            }
        }
    }
}
