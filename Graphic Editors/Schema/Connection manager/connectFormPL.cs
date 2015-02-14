using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace csx
{
    public partial class connectFormPL : Form
    {
        int d;
        decimal llV = 0, lrV = 0, rlV = 0, rrV = 0;
        private cnManager parent;
        public connectFormPL(object port1, object line, object parentF)
        {
            parent = (cnManager)parentF;
            parent.resultOk = false;
            parent.p1 = (Port)port1;
            parent.ln = (Line)line;
            InitializeComponent();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint,
                true);
            this.UpdateStyles();
            parent.assign = new SortedList<int, int>();
            portName1.Text = parent.p1.name + " (" + parent.p1.LeftBusBound + " : " + parent.p1.RightBusBound + " )";
            portName2.Text = parent.ln.parentBegin.name + " --> " + parent.ln.parentEnd.name + " (" + parent.ln.LeftBusBound + " : " + parent.ln.RightBusBound + " )";
            if (parent.p1.LeftBusBound < parent.p1.RightBusBound)
            {
                parent.llB = parent.p1.LeftBusBound;
                parent.lrB = parent.p1.RightBusBound;
                parent.lDownTo = false;
                llV = parent.p1.LeftBusBound;
                lrV = parent.p1.LeftBusBound;
            }
            else
            {
                parent.llB = parent.p1.RightBusBound;
                parent.lrB = parent.p1.LeftBusBound;
                parent.lDownTo = true;
                llV = parent.p1.RightBusBound;
                lrV = parent.p1.RightBusBound;
            }
            if (parent.ln.LeftBusBound < parent.ln.RightBusBound)
            {
                parent.rlB = parent.ln.LeftBusBound;
                parent.rrB = parent.ln.RightBusBound;
                parent.rDownTo = false;
                rlV = parent.p1.LeftBusBound;
                rrV = parent.ln.LeftBusBound;
            }
            else
            {
                parent.rlB = parent.ln.RightBusBound;
                parent.rrB = parent.ln.LeftBusBound;
                parent.rDownTo = true;
                rlV = parent.ln.RightBusBound;
                rrV = parent.ln.RightBusBound;
            }

            if (Math.Abs(parent.p1.RightBusBound - parent.p1.LeftBusBound) > Math.Abs(parent.ln.RightBusBound - parent.ln.LeftBusBound))
            {
                d = Math.Abs(parent.ln.RightBusBound - parent.ln.LeftBusBound);

                rlV = parent.ln.LeftBusBound;
                rrV = parent.ln.RightBusBound;
                llV = parent.p1.LeftBusBound;
                lrV = parent.p1.LeftBusBound + Math.Abs(parent.ln.RightBusBound - parent.ln.LeftBusBound) * Math.Sign(parent.p1.RightBusBound - parent.p1.LeftBusBound);
            }
            else
            {
                d = Math.Abs(parent.p1.RightBusBound - parent.p1.LeftBusBound);
                llV = parent.p1.LeftBusBound;
                lrV = parent.p1.RightBusBound;
                rlV = parent.ln.LeftBusBound;
                rrV = parent.ln.LeftBusBound + Math.Abs(parent.p1.RightBusBound - parent.p1.LeftBusBound) * Math.Sign(parent.ln.RightBusBound - parent.ln.LeftBusBound);
            }
            ll.Maximum = parent.lrB;
            lr.Maximum = parent.lrB;
            rl.Maximum = parent.rrB;
            rr.Maximum = parent.rrB;
            ll.Value = llV;
            lr.Value = lrV;
            rl.Value = rlV;
            rr.Value = rrV;
            ll.Minimum = parent.llB;
            lr.Minimum = parent.llB;
            rl.Minimum = parent.rlB;
            rr.Minimum = parent.rlB;
            checkScroll();
        }
        private void checkScroll()
        {
            sl.Maximum = (int)(parent.lrB - parent.llB - Math.Abs(lrV - llV));
            sl.Value = (int)Math.Min(lrV, llV) - parent.llB;
            sr.Maximum = (int)(parent.rrB - parent.rlB - Math.Abs(rrV - rlV));
            sr.Value = (int)Math.Min(rrV, rlV) - parent.rlB;
        }
        private void addDiapazone(int llB, int lrB, int rlB, int rrB)
        {
            int dl = Math.Sign(lrB - llB);
            int dr = Math.Sign(rrB - rlB);
            for (int l = llB, r = rlB; l != lrB + dl; l += dl, r += dr)
                addPoint(l, r);
            if (dl == 0)
                addPoint(llB, rlB);
        }
        private void addPoint(int x, int y)
        {
            if (parent.assign.ContainsKey(x))
                parent.assign.RemoveAt(parent.assign.IndexOfKey(x));
            if (parent.assign.ContainsValue(y))
                parent.assign.RemoveAt(parent.assign.IndexOfValue(y));
            parent.assign.Add(x, y);
        }
        public void viewList()
        {
            listB.Items.Clear();
            KeyValuePair<int, int> oldValue = new KeyValuePair<int, int>();
            KeyValuePair<int, int> startValue = new KeyValuePair<int, int>();
            bool old = false;
            foreach (KeyValuePair<int, int> ind in parent.assign)
            {
                if (!old)
                {
                    startValue = ind;
                    oldValue = ind;
                    old = true;
                }
                else
                    if (Math.Abs(oldValue.Value - ind.Value) > 1 || Math.Abs(oldValue.Key - ind.Key) > 1)
                    {
                        listB.Items.Add(string.Format("({0,4}; {1,4})    -->    ({2,4}; {3,4})", startValue.Key, oldValue.Key, startValue.Value, oldValue.Value));
                        startValue = ind;
                        oldValue = ind;
                    }
                    else
                        oldValue = ind;
            }
            if (old)
                listB.Items.Add(string.Format("({0,4}; {1,4})    -->    ({2,4}; {3,4})", startValue.Key, oldValue.Key, startValue.Value, oldValue.Value));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            addDiapazone((int)ll.Value, (int)lr.Value, (int)rl.Value, (int)rr.Value);
            viewList();
        }

        private void rr_ValueChanged(object sender, EventArgs e)
        {
            if (rrV == rr.Value)
                return;
            decimal buf;
            if (Math.Abs(rr.Value - rl.Value) > d)
                rlV = rr.Value - Math.Sign(rr.Value - rrV) * d;
            if (Math.Abs(lrV - llV) != Math.Abs(rr.Value - rlV))
            {
                buf = lrV + (rr.Value - rrV) * (Math.Sign(lrV - llV) == 0 ? Math.Sign(rrV - rr.Value) : Math.Sign(lrV - llV)) *
                    (Math.Sign(rrV - rlV) == 0 ? Math.Sign(rrV - rr.Value) : Math.Sign(rrV - rlV));
                if (buf > parent.lrB)
                {
                    lrV = parent.lrB;
                    llV = lrV - Math.Abs(rr.Value - rlV);
                }
                else
                    if (buf < parent.llB)
                    {
                        lrV = parent.llB;
                        llV = lrV + Math.Abs(rr.Value - rlV);
                    }
                    else
                    {
                        llV = buf - Math.Abs(rr.Value - rlV) * (Math.Sign(lrV - llV) == 0 ? Math.Sign(rr.Value - rrV) : Math.Sign(lrV - llV));
                        lrV = buf;
                    }
            }
            rrV = rr.Value;
            rl.Value = rlV;
            lr.Value = lrV;
            ll.Value = llV;
            checkScroll();
        }

        private void lr_ValueChanged(object sender, EventArgs e)
        {
            if (lrV == lr.Value)
                return;
            decimal buf;
            if (Math.Abs(lr.Value - ll.Value) > d)
                llV = lr.Value - Math.Sign(lr.Value - lrV) * d;
            if (Math.Abs(rrV - rlV) != Math.Abs(lr.Value - llV))
            {
                buf = rrV + (lr.Value - lrV) * (Math.Sign(rrV - rlV) == 0 ? Math.Sign(lrV - lr.Value) : Math.Sign(rrV - rlV)) *
                    (Math.Sign(lrV - llV) == 0 ? Math.Sign(lrV - lr.Value) : Math.Sign(lrV - llV));
                if (buf > parent.rrB)
                {
                    rrV = parent.rrB;
                    rlV = rrV - Math.Abs(lr.Value - llV);
                }
                else
                    if (buf < parent.rlB)
                    {
                        rrV = parent.rlB;
                        rlV = rrV + Math.Abs(lr.Value - llV);
                    }
                    else
                    {
                        rlV = buf - Math.Abs(lr.Value - llV) * (Math.Sign(rrV - rlV) == 0 ? Math.Sign(lr.Value - lrV) : Math.Sign(rrV - rlV));
                        rrV = buf;
                    }
            }
            lrV = lr.Value;
            ll.Value = llV;
            rr.Value = rrV;
            rl.Value = rlV;
            checkScroll();
        }

        private void rl_ValueChanged(object sender, EventArgs e)
        {
            if (rlV == rl.Value)
                return;
            decimal buf;
            if (Math.Abs(rl.Value - rr.Value) > d)
                rrV = rl.Value - Math.Sign(rl.Value - rlV) * d;
            if (Math.Abs(llV - lrV) != Math.Abs(rl.Value - rrV))
            {
                buf = llV + (rl.Value - rlV) * (Math.Sign(llV - lrV) == 0 ? Math.Sign(rlV - rl.Value) : Math.Sign(llV - lrV)) *
                    (Math.Sign(rlV - rrV) == 0 ? Math.Sign(rlV - rl.Value) : Math.Sign(rlV - rrV));
                if (buf > parent.lrB)
                {
                    llV = parent.lrB;
                    lrV = llV - Math.Abs(rl.Value - rrV);
                }
                else
                    if (buf < parent.llB)
                    {
                        llV = parent.llB;
                        lrV = llV + Math.Abs(rl.Value - rrV);
                    }
                    else
                    {
                        lrV = buf - Math.Abs(rl.Value - rrV) * (Math.Sign(llV - lrV) == 0 ? Math.Sign(rl.Value - rlV) : Math.Sign(llV - lrV));
                        llV = buf;
                    }
            }
            rlV = rl.Value;
            rr.Value = rrV;
            ll.Value = llV;
            lr.Value = lrV;
            checkScroll();
        }

        private void ll_ValueChanged(object sender, EventArgs e)
        {
            if (llV == ll.Value)
                return;
            decimal buf;
            if (Math.Abs(ll.Value - lr.Value) > d)
                lrV = ll.Value - Math.Sign(ll.Value - llV) * d;
            if (Math.Abs(rlV - rrV) != Math.Abs(ll.Value - lrV))
            {
                buf = rlV + (ll.Value - llV) * (Math.Sign(rlV - rrV) == 0 ? Math.Sign(llV - ll.Value) : Math.Sign(rlV - rrV)) *
                    (Math.Sign(llV - lrV) == 0 ? Math.Sign(llV - ll.Value) : Math.Sign(llV - lrV));
                if (buf > parent.rrB)
                {
                    rlV = parent.rrB;
                    rrV = rlV - Math.Abs(ll.Value - lrV);
                }
                else
                    if (buf < parent.rlB)
                    {
                        rlV = parent.rlB;
                        rrV = rlV + Math.Abs(ll.Value - lrV);
                    }
                    else
                    {
                        rrV = buf - Math.Abs(ll.Value - lrV) * (Math.Sign(rlV - rrV) == 0 ? Math.Sign(ll.Value - llV) : Math.Sign(rlV - rrV));
                        rlV = buf;
                    }
            }
            llV = ll.Value;
            lr.Value = lrV;
            rl.Value = rlV;
            rr.Value = rrV;
            checkScroll();
        }

        private void sr_Scroll(object sender, EventArgs e)
        {
            if (rrV > rlV)
            {
                rrV = parent.rlB + sr.Value + rrV - rlV;
                rlV = parent.rlB + sr.Value;
            }
            else
            {
                rlV = parent.rlB + sr.Value + rlV - rrV;
                rrV = parent.rlB + sr.Value;
            }
            rr.Value = rrV;
            rl.Value = rlV;
        }

        private void sl_Scroll(object sender, EventArgs e)
        {
            if (lrV > llV)
            {
                lrV = parent.llB + sl.Value + lrV - llV;
                llV = parent.llB + sl.Value;
            }
            else
            {
                llV = parent.llB + sl.Value + llV - lrV;
                lrV = parent.llB + sl.Value;
            }
            lr.Value = lrV;
            ll.Value = llV;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            decimal buf;
            buf = rrV;
            rrV = rlV;
            rlV = buf;
            rl.Value = rlV;
            rr.Value = rrV;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            decimal buf;
            buf = lrV;
            lrV = llV;
            llV = buf;
            ll.Value = llV;
            lr.Value = lrV;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string p1Name = parent.ln.parentBegin.name + (parent.ln.parentBegin.bus ? "(" + parent.ln.parentBegin.LeftBusBound + "; " + parent.ln.parentBegin.RightBusBound + ")" : "");
            string p2Name = parent.ln.parentEnd.name + (parent.ln.parentEnd.bus ? "(" + parent.ln.parentEnd.LeftBusBound + "; " + parent.ln.parentEnd.RightBusBound + ")" : "");
            redactorPL rpl = new redactorPL(this, parent, portName1.Text, p1Name, p2Name);
            rpl.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            parent.resultOk = true;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int min = Math.Min((int)ll.Value, (int)lr.Value);
            int max = Math.Max((int)ll.Value, (int)lr.Value);
            for (int i = min; i <= max; i++)
                if (parent.assign.ContainsKey(i))
                    parent.assign.Remove(i);
            min = Math.Min((int)rl.Value, (int)rr.Value);
            max = Math.Max((int)rl.Value, (int)rr.Value);
            for (int i = min; i <= max; i++)
                if (parent.assign.ContainsValue(i))
                    parent.assign.RemoveAt(parent.assign.IndexOfValue(i));
            viewList();
        }
    }
}