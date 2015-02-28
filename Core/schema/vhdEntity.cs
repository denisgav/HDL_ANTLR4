using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace csx
{
    [Serializable]
    public class vhdEntity
    {
        public string name;
        public List<vhdPort> ports;
        public List<generic> gen_cont;
        public vhdEntity()
        {
            ports = new List<vhdPort>();
            gen_cont = new List<generic>();
        }
    }
}
