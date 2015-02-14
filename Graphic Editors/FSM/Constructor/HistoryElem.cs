using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Schematix.FSM
{
    public class HistoryElem
    {
        private Schematix.FSM.My_Graph graph;
        public Schematix.FSM.My_Graph Graph
        {
            get { return graph; }
        }

        private string name;
        public string Name
        {
            get { return name; }
        }

        private bool isSaved;
        public bool IsSaved
        {
            get { return isSaved; }
            set { isSaved = value; }
        }

        public HistoryElem(Schematix.FSM.My_Graph graph, String name, bool isSaved = false)
        {
            this.graph = graph;
            this.name = name;
            this.isSaved = isSaved;
        }
    }
}
