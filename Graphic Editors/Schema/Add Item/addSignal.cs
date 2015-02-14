using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;

namespace csx
{    
    public partial class addSignal : Form
    {
        private Construct @model;
        public addSignal(object @model)
        {
            this.model = (Construct)@model;
            InitializeComponent();
        }

        private void AddSignal()
        {
            if (signalName.Text != "")
            {
                bool bus = Math.Abs(signalLeftBound.Value - signalRightBound.Value) > 0;
                string portType = bus ? "std_logic_vector" : "std_logic";
                Graphics gr = CreateGraphics();
                IntPtr hdc = gr.GetHdc();
                Metafile mf = new Metafile(new MemoryStream(), hdc, new Rectangle(0, 0, 16, 16), MetafileFrameUnit.Pixel);
                gr.ReleaseHdc(hdc);
                Pen pn = new Pen(Color.Black, bus ? 3 : 1);
                gr = Graphics.FromImage(mf);
                gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                gr.DrawRectangle(pn, new Rectangle(1, 1, 13, 13));
                Element el = new Element(mf, model, "Terminator", ElementType.Terminator);
                gr.Dispose();
                el.border = new RectangleF(100, 100, 16, 16);
                el.ports.Add(new Port(portInOut.InOut, portType, new PointF(7, 14), el, 3, "Terminator", (int)signalLeftBound.Value, (int)signalRightBound.Value, bus));
                Element el2 = new Element(mf, model, "Terminator", ElementType.Terminator);
                el2.border = new RectangleF(100, 200, 16, 16);
                el2.ports.Add(new Port(portInOut.InOut, portType, new PointF(7, 1), el2, 1, "Terminator", (int)signalLeftBound.Value, (int)signalRightBound.Value, bus));
                Line ln = new Line(el.ports[0], el2.ports[0], bus, (int)signalLeftBound.Value, (int)signalRightBound.Value, signalName.Text);
                ln.isSignal = true;
                el.ports[0].line = ln;
                el2.ports[0].line = ln;
                if (!bus)
                    ln.assign.Add(0, 0);
                else
                {
                    decimal di = Math.Sign(signalRightBound.Value - signalLeftBound.Value);
                    for (decimal i = signalLeftBound.Value; i != signalRightBound.Value + di; i += di)
                        ln.assign.Add((int)i, (int)i);
                }
                model.AddElement(el);
                model.AddElement(el2);
                ln.PassFinding();
                model.AddLine(ln);
                model.parent.history.Changed();
                model.parent.Invalidate();
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            AddSignal();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void signalName_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = signalName.Text != "";
            button3.Enabled = button1.Enabled;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddSignal();
            signalName.Text = "";
        }
    }
}