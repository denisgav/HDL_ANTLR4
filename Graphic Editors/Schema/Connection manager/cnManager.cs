using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace csx
{
    class cnManager
    {
        System.Windows.Forms.UserControl parent;
        public SortedList<int, int> assign;
        public int llB, lrB, rlB, rrB;
        public bool lDownTo, rDownTo;
        public Port p1, p2;
        public Line ln;
        public bool resultOk = false;

        public cnManager(Port port1, Port port2, System.Windows.Forms.UserControl parent)
        {
            this.parent = parent;
            assign = new SortedList<int,int>();
            if (port1.bus)
                if (port2.bus)
                {
                    connectFormPP cf = new connectFormPP(port1, port2, this);
                    cf.ShowDialog();
                    if (assign.Count == 0)
                        resultOk = false;
                }
                else
                {
                    connectForm1 cf = new connectForm1(port1.LeftBusBound, port1.RightBusBound, this, false);
                    cf.ShowDialog();
                    if (assign.Count == 0)
                        resultOk = false;
                }
            else
                if (port2.bus)
                {
                    connectForm1 cf = new connectForm1(port2.LeftBusBound, port2.RightBusBound, this, true);
                    cf.ShowDialog();
                    if (assign.Count == 0)
                        resultOk = false;
                }
                else
                {
                    assign.Add(0, 0);
                    resultOk = true;
                    if (assign.Count == 0)
                        resultOk = false;
                }
        }

        public cnManager(Port port1, Line line, System.Windows.Forms.UserControl parent)
        {
            this.parent = parent;
            assign = new SortedList<int, int>();
            if (port1.bus)
                if (line.bus)
                {
                    connectFormPL cf = new connectFormPL(port1, line, this);
                    cf.ShowDialog();
                    if (assign.Count == 0)
                        resultOk = false;
                }
                else
                {
                    connectForm1 cf = new connectForm1(port1.LeftBusBound, port1.RightBusBound, this, false);
                    cf.ShowDialog();
                    if (assign.Count == 0)
                        resultOk = false;
                }
            else
                if (line.bus)
                {
                    connectForm1 cf = new connectForm1(line.LeftBusBound, line.RightBusBound, this, true);
                    cf.ShowDialog();
                    if (assign.Count == 0)
                        resultOk = false;
                }
                else
                {
                    assign.Add(0, 0);
                    resultOk = true;
                    if (assign.Count == 0)
                        resultOk = false;
                }
        }
    }
}
