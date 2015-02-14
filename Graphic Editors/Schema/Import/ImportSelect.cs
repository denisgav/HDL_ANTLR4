using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Drawing.Imaging;
using System.IO;

namespace csx
{
    public partial class ImportSelect : Form
    {
        private Construct @model;
        private SortedList<string, vhdEntity> entities;
        public ImportSelect(SortedList<string, vhdEntity> entities, object @model)
        {
            this.model = (Construct)@model;
            this.entities = entities;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ImportSelect_Load(object sender, EventArgs e)
        {
            int i;
            checkedListBox1.Items.Clear();
            for (i = 0; i < entities.Count; i++)
                checkedListBox1.Items.Add(entities.Values[i].name);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int ElementX = 50;
            int ElementY = 50;
            int countLeft;
            int countRight;
            bool isLeftInOut;
            int rast = 10;
            int d = 10;
            int h;
            int w;
            Graphics gr = CreateGraphics();
            Metafile mf;
            Graphics gr2;
            IntPtr hdc = gr.GetHdc();            
            Pen pn = new Pen(Color.Black, 1);
            Pen pin = new Pen(Color.Blue, 1);
            pin.EndCap = System.Drawing.Drawing2D.LineCap.Square;
            Pen pout = new Pen(Color.Green, 1);
            pout.EndCap = System.Drawing.Drawing2D.LineCap.Square;
            Pen pinout = new Pen(Color.Brown, 1);
            pinout.EndCap = System.Drawing.Drawing2D.LineCap.Square;
            Font f = new Font("Courier New", 12, GraphicsUnit.Pixel); //при высоте 15 ширина 9
            int fontWidht = 8;
            int halfFontHeight = 8;
            float dl;
            float dxl;
            float dr;
            float dxr;
            Element el;
            string str;

            int i, j;
            List<vhdPort> inP = new List<vhdPort>();
            List<vhdPort> outP = new List<vhdPort>();
            List<vhdPort> inoutP = new List<vhdPort>();
            List<string> nameIn = new List<string>();
            List<string> nameOut = new List<string>();
            List<string> nameInout = new List<string>();
            int maxIn = 0;
            int maxOut = 0;
            int maxInout = 0;
            int maxLeft = 0;
            int maxRight = 0;
            vhdEntity ve;
            vhdPort pt;
            int q;

            for (q = 0; q < checkedListBox1.CheckedItems.Count; q++)
            {
                int poz = checkedListBox1.Items.IndexOf(checkedListBox1.CheckedItems[q]);
                ve = entities.Values[poz];
                inP.Clear();
                outP.Clear();
                inoutP.Clear();
                nameIn.Clear();
                nameOut.Clear();
                nameInout.Clear();
                for (j = 0; j < ve.ports.Count; j++)
                {
                    pt = (vhdPort)ve.ports[j];
                    str = pt.name + (pt.bus ? "(" + pt.leftBound.ToString() + ";" + pt.rightBound + ")" : "");
                    switch (pt.inout)
                    {
                        case portInOut.In:
                            inP.Add(pt);                            
                            nameIn.Add(str);
                            if (str.Length > maxIn)
                                maxIn = str.Length;
                            break;
                        case portInOut.Out:
                            outP.Add(pt);
                            nameOut.Add(str);
                            if (str.Length > maxOut)
                                maxOut = str.Length;
                            break;
                        case portInOut.InOut:
                            inoutP.Add(pt);
                            nameInout.Add(str);
                            if (str.Length > maxInout)
                                maxInout = str.Length;
                            break;
                    }
                }
                isLeftInOut = inP.Count < outP.Count;
                if (isLeftInOut)
                {
                    countLeft = inP.Count + inoutP.Count;
                    countRight = outP.Count;
                    maxLeft = Math.Max(maxIn, maxInout);
                    maxRight = maxOut;
                }
                else
                {
                    countLeft = inP.Count;
                    countRight = outP.Count + inoutP.Count;
                    maxLeft = maxIn;
                    maxRight = Math.Max(maxOut, maxInout);
                }

                h = countLeft > countRight ? rast * (countLeft + 1) : rast * (countRight + 1);

                w = (maxLeft + maxRight + 1) * fontWidht;

                mf = new Metafile(new MemoryStream(), hdc, new Rectangle(-d, 0, w + 2 * d + 1, h + 1), MetafileFrameUnit.Pixel);
                
                gr2 = Graphics.FromImage(mf);
                dl = 1;
                dr = 1;
                bool set = false;
                if (countLeft != 0)
                {
                    dl = h / countLeft;
                    dxl = dl / 2;
                    for (i = 0; i < inP.Count; i++)
                    {
                        if (!set && dxl >= h / 2 - 1)
                        {
                            set = true;
                            dxl += 1;
                        }
                        if (inP[i].bus)
                            //if (Math.Abs(dxl - h / 2) < dl - .5)
                            //    pin.Width = (float)3.05;
                            //else
                                pin.Width = (float)3.03;
                        else
                            pin.Width = 1;
                        gr2.DrawLine(pin, 0, (int)dxl, -d, (int)dxl);
                        gr2.DrawString(nameIn[i], f, Brushes.Black, 0, dxl - halfFontHeight);
                        dxl += dl;
                    }
                    if (isLeftInOut)
                        for (i = 0; i < inoutP.Count; i++)
                        {
                            if (!set && dxl >= h / 2 - 1)
                            {
                                set = true;
                                dxl += 1;
                            }
                            if (inoutP[i].bus)
                                //if (Math.Abs(dxl - h / 2) < dl - .5)
                                //    pinout.Width = (float)3.05;
                                //else
                                pinout.Width = (float)3.03;
                            else
                                pinout.Width = 1;
                            gr2.DrawLine(pinout, 0, (int)dxl, -d, (int)dxl);
                            gr2.DrawString(nameInout[i], f, Brushes.Black, 0, dxl - halfFontHeight);
                            dxl += dl;
                        }
                }
                set = false;
                if (countRight != 0)
                {
                    dr = h / countRight;
                    dxr = dr / 2;
                    for (i = 0; i < outP.Count; i++)
                    {
                        if (!set && dxr >= h / 2 - 1)
                        {
                            set = true;
                            dxr += 1;
                        }
                        if (outP[i].bus)
                            //if (Math.Abs(dxr - h / 2) < dr - .5)
                            //    pout.Width = (float)3.05;
                            //else
                            pout.Width = (float)3.03;
                        else
                            pout.Width = 1;
                        gr2.DrawLine(pout, w, (int)dxr, w + d, (int)dxr);
                        gr2.DrawString(nameOut[i], f, Brushes.Black, w - nameOut[i].Length * fontWidht - 2, dxr - halfFontHeight);
                        dxr += dr;
                    }
                    if (!isLeftInOut)
                        for (i = 0; i < inoutP.Count; i++)
                        {
                            if (!set && dxr >= h / 2 - 1)
                            {
                                set = true;
                                dxr += 1;
                            }
                            if (inoutP[i].bus)
                                //if (Math.Abs(dxr - h / 2) < dr - .5)
                                //    pinout.Width = (float)3.05;
                                //else
                                pinout.Width = (float)3.03;
                            else
                                pinout.Width = 1;
                            gr2.DrawLine(pinout, w, (int)dxr, w + d, (int)dxr);
                            gr2.DrawString(nameInout[i], f, Brushes.Black, w - nameInout[i].Length * fontWidht - 2, dxr - halfFontHeight);
                            dxr += dr;
                        }
                }
                gr2.DrawRectangle(pn, new Rectangle(0, 0, w, h));
                el = new Element(mf, model, ve.name, ElementType.Element);
                gr2.Dispose();
                el.border = new RectangleF(ElementX, ElementY, w + 2 * d, h);
                ElementX += w + 2 * d + 5;
                ElementY += h + 5;
                if (countLeft != 0)
                {
                    dxl = dl / 2;
                    for (i = 0; i < inP.Count; i++)
                    {
                        el.ports.Add(new Port(portInOut.In, inP[i].type, new PointF(0, (int)dxl), el, 2, inP[i].name, inP[i].leftBound, inP[i].rightBound, inP[i].bus));
                        dxl += dl;
                    }
                    if (isLeftInOut)
                        for (i = 0; i < inoutP.Count; i++)
                        {
                            el.ports.Add(new Port(portInOut.InOut, inoutP[i].type, new PointF(0, (int)dxl), el, 2, inoutP[i].name, inoutP[i].leftBound, inoutP[i].rightBound, inoutP[i].bus));
                            dxl += dl;
                        }
                }
                if (countRight != 0)
                {
                    dxr = dr / 2;
                    for (i = 0; i < outP.Count; i++)
                    {
                        el.ports.Add(new Port(portInOut.Out, outP[i].type, new PointF(w + 2 * d, (int)dxr), el, 0, outP[i].name, outP[i].leftBound, outP[i].rightBound, outP[i].bus));
                        dxr += dr;
                    }
                    if (!isLeftInOut)
                        for (i = 0; i < inoutP.Count; i++)
                        {
                            el.ports.Add(new Port(portInOut.InOut, inoutP[i].type, new PointF(w + 2 * d, (int)dxr), el, 0, inoutP[i].name, inoutP[i].leftBound, inoutP[i].rightBound, inoutP[i].bus));
                            dxr += dr;
                        }
                }
                model.AddElement(el);
                model.parent.history.Changed();
                this.Close();
            }
            gr.ReleaseHdc(hdc);
            gr.Dispose();
        }
    }
}