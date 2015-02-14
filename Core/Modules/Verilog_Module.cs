using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Schematix.Core
{
    public enum VerilogPortDirection
    {
        In,
        Out,
        InOut,
    }

    [Serializable]
    public class Verilog_Port
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public VerilogPortDirection Direction { get; set; }
        public bool isBus { get; set; }
        public int LeftIndex { get; set; }
        public int RightIndex { get; set; }

        public Verilog_Port() { }
        public Verilog_Port(string Name, string Type, VerilogPortDirection Direction)
        {
            this.Name = Name;
            this.Type = Type;
            this.Direction = Direction;
            isBus = false;
        }
        public Verilog_Port(string Name, string Type, VerilogPortDirection Direction, int LeftIndex, int RightIndex)
        {
            this.Name = Name;
            this.Type = Type;
            this.Direction = Direction;
            this.LeftIndex = LeftIndex;
            this.RightIndex = RightIndex;
            isBus = true;
        }
    }

    [Serializable]
    public class Verilog_Module
    {
        public string ModuleName { get; set; }
        public string Timescale { get; set; }
        public List<Verilog_Port> PortList { get; set; }
    }
}
