using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parser
{
    public class TypeInfo { }
    
    public class Module_Type
    {
        private KeyValuePair<string, TypeInfo> type;
        public string get_Type_name()
        {
            return type.Key;
        }
        public Module_Type(string type_name, TypeInfo type_info)
        {
            type = new KeyValuePair<string, TypeInfo>(type_name, type_info);
        }
    }

}
