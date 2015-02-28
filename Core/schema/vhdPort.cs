using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace csx
{
    public enum portInOut { In, Out, InOut, Buffer };

    [Serializable]
    public class vhdPort
    {
        public string name;
        public portInOut inout;
        public string type;
        public bool bus;
        public int leftBound; //if bus
        public int rightBound; //if bus

        public vhdPort() { }

        public vhdPort(string name, portInOut inout, string type, bool bus, int leftBound, int rightBound)
        {
            this.name = name;
            this.inout = inout;
            this.type = type;
            this.bus = bus;
            this.leftBound = leftBound;
            this.rightBound = rightBound;
        }

        public vhdPort(string name, portInOut inout, string type)
        {
            this.name = name;
            this.inout = inout;
            this.type = type;
            this.bus = false;
            this.leftBound = 0;
            this.rightBound = 0;
        }
    }
}
