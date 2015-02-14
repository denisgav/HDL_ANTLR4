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
    public partial class addExternPort : Form
    {
        private Construct @model;
        private List<PointF> ports;
        private const float StartLeftXPorts = 120;
        private const float StartRigthXPorts = 400;
        private const float StartYPorts = 100;
        private const float DYPorts = 30;
        
        public addExternPort(object @model)
        {
            this.model = (Construct)@model;
            ports = new List<PointF>();
            Element el;
            for (int i = 0; i < this.model.elements.Count; i++)
            {
                el = this.model.elements[i];
                if (el.elementType == ElementType.ExternPort)
                {
                    if (el.ports[0].napr == 0)
                        ports.Add(new PointF(el.border.Right, el.border.Top));
                    else
                        ports.Add(el.border.Location);
                }
            }
            InitializeComponent();
        }

        public void AddPort()
        {
            if (portName.Text != "")
            {
                bool bus = Math.Abs(leftBound.Value - rightBound.Value) > 0;
                string portType = bus ? "std_logic_vector" : "std_logic";

                Graphics gr = CreateGraphics();

                StringFormat strFormat = new StringFormat();
                strFormat.Alignment = StringAlignment.Near;
                strFormat.LineAlignment = StringAlignment.Center;
                Font fnt;
                //if (bus)
                //    fnt = new Font(Font.SystemFontName, 13, FontStyle.Bold, GraphicsUnit.Pixel);
                //else
                    fnt = new Font(Font.SystemFontName, 13, GraphicsUnit.Pixel);

                
                SizeF size = gr.MeasureString(portName.Text, fnt);

                Port pt = null;
                Pen pn;
                if (In.Checked)
                {
                    pn = new Pen(Color.Green, bus ? 3 : 1);
                    if (left.Checked)
                        pt = new Port(portInOut.Out, portType, new PointF(29 + size.Width, 6), null, 0, portName.Text, (int)leftBound.Value, (int)rightBound.Value, bus);
                    else
                        pt = new Port(portInOut.Out, portType, new PointF(1, 6), null, 2, portName.Text, (int)leftBound.Value, (int)rightBound.Value, bus);
                }
                else
                    if (Out.Checked)
                    {
                        pn = new Pen(Color.Blue, bus ? 3 : 1);
                        if (left.Checked)
                            pt = new Port(portInOut.In, portType, new PointF(29 + size.Width, 6), null, 0, portName.Text, (int)leftBound.Value, (int)rightBound.Value, bus);
                        else
                            pt = new Port(portInOut.In, portType, new PointF(1, 6), null, 2, portName.Text, (int)leftBound.Value, (int)rightBound.Value, bus);
                    }
                    else
                    {
                        pn = new Pen(Color.Brown, bus ? 3 : 1);
                        if (left.Checked)
                            pt = new Port(portInOut.InOut, portType, new PointF(29 + size.Width, 6), null, 0, portName.Text, (int)leftBound.Value, (int)rightBound.Value, bus);
                        else
                            pt = new Port(portInOut.InOut, portType, new PointF(1, 6), null, 2, portName.Text, (int)leftBound.Value, (int)rightBound.Value, bus);
                    }

                IntPtr hdc = gr.GetHdc();

                Metafile mf = new Metafile(new MemoryStream(), hdc, 
                    new Rectangle(0, 0, 31 + (int)(size.Width+.5), 13), MetafileFrameUnit.Pixel);
                gr.ReleaseHdc(hdc);
                gr = Graphics.FromImage(mf);
                gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                PointF[] p = new PointF[7];
                if (left.Checked)
                {
                    p[0] = new PointF(29 + size.Width, 5.5f);
                    p[1] = new PointF(21 + size.Width, 5.5f);
                    p[2] = new PointF(12 + size.Width, 1);
                    p[3] = new PointF(1 + size.Width, 1);
                    p[4] = new PointF(1 + size.Width, 11);
                    p[5] = new PointF(12 + size.Width, 11);
                    p[6] = new PointF(21 + size.Width, 5.5f);
                }
                else
                {
                    p[0] = new PointF(1, 5.5f);
                    p[1] = new PointF(9, 5.5f);
                    p[2] = new PointF(18, 1);
                    p[3] = new PointF(29, 1);
                    p[4] = new PointF(29, 11);
                    p[5] = new PointF(18, 11);
                    p[6] = new PointF(9, 5.5f);
                }
                gr.DrawLines(pn, p);
                if (pt.napr == 0)
                {
                    gr.DrawString(pt.name, fnt, pn.Brush, new RectangleF(0, 0, size.Width, 13), strFormat);
                }
                else
                {
                    gr.DrawString(pt.name, fnt, pn.Brush, new RectangleF(31, 0, 31 + size.Width, 13), strFormat);
                }
                Element el = new Element(mf, model, portName.Text, ElementType.ExternPort);
                gr.Dispose();
                float curY = StartYPorts;
                float curX;
                if (left.Checked)
                    curX = StartLeftXPorts;
                else
                    curX = StartRigthXPorts;
                while (ports.Contains(new PointF(curX, curY)))
                    curY += DYPorts;

                if (left.Checked)
                    el.border = new RectangleF(curX - size.Width - 31, curY, 31 + size.Width, 13);
                else
                    el.border = new RectangleF(curX, curY, 31 + size.Width, 13);

                ports.Add(new PointF(curX, curY));
                
                pt.parent = el;
                el.ports.Add(pt);
                model.AddElement(el);
                model.parent.history.Changed();
                model.parent.Invalidate();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddPort();
            this.Close();
        }

        private void InOut_CheckedChanged(object sender, EventArgs e)
        {
            groupBox1.Enabled = InOut.Checked;
        }

        private void portName_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = portName.Text != "";
            button3.Enabled = button1.Enabled;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddPort();
            portName.Text = "";
        }

        private void In_CheckedChanged(object sender, EventArgs e)
        {
            if (In.Checked)
                left.Checked = true;
        }

        private void Out_CheckedChanged(object sender, EventArgs e)
        {
            if (Out.Checked)
                right.Checked = true;
        }
    }
}