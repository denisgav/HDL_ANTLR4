using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace csx
{
    [Serializable]
    public class generic
    {
        public int number;
        public string type_of_var;
        public string var_name;
        public int var_value;

        public generic() { }
        public generic(generic g)
        {
            this.number = g.number;
            this.type_of_var = g.type_of_var;
            this.var_name = g.var_name;
            this.var_value = g.var_value;
        }
    }
}
