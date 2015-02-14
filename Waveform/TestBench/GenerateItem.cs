using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Parser;
using Schematix.Waveform;
using DataContainer.Generator;

namespace Schematix.Waveform.TestBench
{

    internal struct StimulatorDialogResult
    {
        public string port_name;
        public Generator gtr;
        public StimulatorDialogResult(string name, Generator g)
        {
            
            this.port_name = name;
            this.gtr = g;
            
        }
    }

    internal class GenerateItem
    {
        private List<PortItem> entity_ports = null;
        private List<ArchitectureBlockDeclarativeItem> architecture_signals = null;
    }
    internal class Generator
    {
        public string type;
        public Generator(string t)
        {
            this.type = t;
        }
    }
    internal class ItemGenerator
    {
        protected BaseGenerator g;
        protected int index;
        public ItemGenerator() { }
        public ItemGenerator(int i,BaseGenerator g)
        { 
            this.g = g;
            this.index = i;
        }
        public BaseGenerator Generator
        {
            get { return g; }
            set { this.g = value; }
        }
        public int Index
        {
            get { return index; }
            set { this.index = value; }
        }
    }
    internal class PortGenerator : ItemGenerator
    {
        protected PortItem pi;
        protected Entity entity;
        public PortGenerator() { } 
        public PortGenerator(Entity en,int index,PortItem i,BaseGenerator g)
        :base(index,g)
        {
            this.pi = i;
            this.entity = en;
        }

        public PortItem Port
        {
            get { return pi; }
            set { this.pi = value; }
        }
        public string Name
        {
            get { return pi.id_list[index]; }
        }
        public Entity Entity
        {
            get { return entity; }
        }

    }
    internal class SignalGenerator : ItemGenerator
    {
        protected ArchitectureBlockDeclarativeItem ai;
        public SignalGenerator() { }
        public SignalGenerator(int index, ArchitectureBlockDeclarativeItem i, BaseGenerator g)
            : base(index,g)
        {
            this.ai = i;
        }

        public ArchitectureBlockDeclarativeItem Port
        {
            get { return ai; }
            set { this.ai = value; }
        }
        public string Name
        {
            get { return ai.id_list[index]; }
        }

    }

}
