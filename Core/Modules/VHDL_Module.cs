using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Schematix.Core
{
    public enum VHDLPortDirection
    {
        In,
        Out,
        InOut,
        Buffer
    }

    [Serializable]
    public class VHDL_Port
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public VHDLPortDirection Direction { get; set; }
        public bool isBus
        {
            get
            {
                return (LeftIndex != 0) && (RightIndex != 0);
            }
        }
        public int LeftIndex { get; set; }
        public int RightIndex { get; set; }

        public VHDL_Port() { }
        public VHDL_Port(string Name, string Type, VHDLPortDirection Direction)
        {
            this.Name = Name;
            this.Type = Type;
            this.Direction = Direction;
        }
        public VHDL_Port(string Name, string Type, VHDLPortDirection Direction, int LeftIndex, int RightIndex)
        {
            this.Name = Name;
            this.Type = Type;
            this.Direction = Direction;
            this.LeftIndex = LeftIndex;
            this.RightIndex = RightIndex;
        }
    }

    [Serializable]
    public class VHDL_Module
    {
        public string ArchitectureName { get; set; }
        public string EntityName { get; set; }
        public List<VHDL_Port> PortList { get; set; }
    }
}
