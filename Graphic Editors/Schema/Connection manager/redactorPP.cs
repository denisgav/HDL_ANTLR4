using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace csx
{
    public partial class redactorPP : Form
    {
        private int ncon = 0;
        private float y = 0;
        private float scrollW = 20;
        private connectFormPP parent;
        private cnManager parentManager;
        private float ot = 60;
        private float ot0 = 25;
        private float fontR = 25;
        private float fontH = 7;
        private Pen p = Pens.Black;
        private Font f = new Font("Courier New", 12, GraphicsUnit.Pixel); //при высоте 15 ширина 9

        private float otstyp = 30;
        private bool drawConnect = false;
        private PointF connectLocation;
        private bool drawLine = false;
        private PointF LinePoint1, LinePoint2;
        private float dy = 0;

        public redactorPP(object parentF, object parentM, string namePort1, string namePort2)
        {            
            parentManager = (cnManager)parentM;
            parent = (connectFormPP)parentF;
            InitializeComponent();
            portName1.Text = namePort1;
            portName2.Text = namePort2;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
        }

        private void redactorPP_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            int i;
            float dd = otstyp;
            int di = Math.Sign(parentManager.p1.RightBusBound - parentManager.p1.LeftBusBound);
            for (i = parentManager.p1.LeftBusBound; i != parentManager.p1.RightBusBound + di; i += di)
            {
                g.DrawString(i.ToString(), f, p.Brush, new PointF(0, dd - fontH + y));
                g.DrawLine(p, ot0, dd + y, ot, dd + y);
                g.DrawLine(p, ot, dd + y - 1, ot, dd + y + 1);
                dd += dy;
            }
            dd = otstyp;
            di = Math.Sign(parentManager.p2.RightBusBound - parentManager.p2.LeftBusBound);
            for (i = parentManager.p2.LeftBusBound; i != parentManager.p2.RightBusBound + di; i += di)
            {
                g.DrawString(i.ToString(), f, p.Brush, new PointF(Width - fontR - scrollW, dd - fontH + y));
                g.DrawLine(p, Width - ot0 - scrollW, dd + y, Width - ot - scrollW, dd + y);
                g.DrawLine(p, Width - ot - scrollW, dd + y - 1, Width - ot - scrollW, dd + y + 1);
                dd += dy;
            }
            Pen pn = new Pen(Color.Black);
            Random rnd = new Random(parentManager.lrB);
            int A = 255, R, G, B, nom = 0;
            R = rnd.Next(0, 255);
            G = rnd.Next(0, 255);
            B = rnd.Next(0, 255);
            foreach (KeyValuePair<int, int> ind in parentManager.assign)
            {
                nom++;
                if (nom > 3)
                {
                    nom = 1;
                    if (A > 125)
                        A = rnd.Next(0, 125);
                    else
                        A = rnd.Next(125, 255);
                }
                switch (nom)
                {
                    case 1:
                        R = A;
                        G = 255-A;
                        B = 255-A;
                        break;
                    case 2:
                        R = 255-A;
                        G = A;
                        B = 255-A;
                        break;
                    case 3:
                        R = 255-A;
                        G = 255-A;
                        B = A;
                        break;
                }
                //switch (nom)
                //{
                //    case 1:
                //        if (R > 125)
                //            R = rnd.Next(200, 255);
                //        else
                //            R = rnd.Next(0, 55);
                //        //A = rnd.Next(0, 255);
                //        G = rnd.Next(0, 25) * 10;
                //        B = rnd.Next(0, 25) * 10;
                //        break;
                //    case 2:
                //        R = rnd.Next(0, 25) * 10;
                //        if (G > 125)
                //            G = rnd.Next(200, 255);
                //        else
                //            G = rnd.Next(0, 55);
                //        B = rnd.Next(0, 25) * 10;
                //        break;
                //    case 3:
                //        R = rnd.Next(0, 25) * 10;
                //        G = rnd.Next(0, 25) * 10;
                //        if (B > 125)
                //            B = rnd.Next(0, 55);
                //        else
                //            B = rnd.Next(200, 255);
                //        break;
                //}
                pn.Color = Color.FromArgb(R, G, B);
                g.DrawLine(pn, ot, otstyp + dy * Math.Abs(ind.Key - (parentManager.lDownTo ? parentManager.lrB : parentManager.llB)) + y, Width - ot - scrollW, otstyp + dy * Math.Abs(ind.Value - (parentManager.rDownTo ? parentManager.rrB : parentManager.rlB)) + y);
            }
            if (drawLine)
                g.DrawLine(Pens.Red, LinePoint1.X, LinePoint1.Y, LinePoint2.X, LinePoint2.Y);
            if (drawConnect)
                g.FillEllipse(Brushes.Green, connectLocation.X - 4, connectLocation.Y - 4, 8, 8);
        }

        private void onResize()
        {
            //Dock = DockStyle.Fill;
            dy = (Height - otstyp - 70) / (Math.Max((parentManager.lrB - parentManager.llB), (parentManager.rrB - parentManager.rlB)));
            if (dy < 20)
            {
                //Dock = DockStyle.Top;
                dy = 20;
                vS.Visible = true;
                scrollW = 30;
                vS.Minimum = 0;
                vS.Maximum = 20 * Math.Max((parentManager.lrB - parentManager.llB), (parentManager.rrB - parentManager.rlB)) + (int)otstyp + 95 - Height;
                vS.Value = 0;
                y = 0;
            }
            else
            {
                vS.Visible = false;
                y = 0;
                scrollW = 10;
            }
            drawConnect = false;
            drawLine = false;
        }

        private void redactorPP_Load(object sender, EventArgs e)
        {
            onResize();
        }

        private void redactorPP_MouseMove(object sender, MouseEventArgs e)
        {
            bool dc;
            int n = (int)((e.Y - otstyp - y) / dy + .5);
            if (Math.Abs(e.X - ot) < 9)
                if (n <= parentManager.lrB - parentManager.llB && Math.Abs(n * dy + otstyp - e.Y + y) < 9 &&
                    !(drawLine && LinePoint1.X == ot))
                {
                    dc = true;
                    connectLocation.Y = n * dy + otstyp + y;
                    connectLocation.X = ot;
                }
                else
                    dc = false;
            else
                if (Math.Abs(e.X - Width + ot + scrollW) < 9)
                    if (n <= parentManager.rrB - parentManager.rlB && Math.Abs(n * dy + otstyp - e.Y + y) < 9 &&
                        !(drawLine && LinePoint1.X != ot))
                    {
                        dc = true;
                        connectLocation.Y = n * dy + otstyp + y;
                        connectLocation.X = Width - ot - scrollW;
                    }
                    else
                        dc = false;
                else
                    dc = false;
            if (dc != drawConnect)
            {
                drawConnect = dc;
                Invalidate();
            }
            if (e.Y < 20)
                if (vS.Visible && vS.Value - 10 > vS.Minimum)
                {
                    vS.Value -= 10;
                    LinePoint1.Y += 10;
                    y = -vS.Value;
                    this.Invalidate();
                }  
            if (e.Y > Height - 50)
                if (vS.Visible && vS.Value + 10 < vS.Maximum)
                {
                    vS.Value += 10;
                    LinePoint1.Y -= 10;
                    y = -vS.Value;
                }
            if (drawLine)
            {
                LinePoint2.X = e.X;
                LinePoint2.Y = e.Y;
            }
            this.Invalidate();
        }

        private void redactorPP_Resize(object sender, EventArgs e)
        {
            onResize();
        }

        private void vS_Scroll(object sender, ScrollEventArgs e)
        {
            y = - vS.Value;
            this.Invalidate();
        }

        private void redactorPP_MouseDown(object sender, MouseEventArgs e)
        {
            if (drawConnect)
            {
                int n = (int)((connectLocation.Y - otstyp - y) / dy + .5);
                if (connectLocation.X == ot)
                {
                    if (parentManager.lDownTo)
                        n = parentManager.p1.LeftBusBound - n;
                    else
                        n = parentManager.p1.LeftBusBound + n;
                    if (parentManager.assign.ContainsKey(n))
                        parentManager.assign.RemoveAt(parentManager.assign.IndexOfKey(n));
                    ncon = n;
                    drawLine = true;
                    LinePoint1.X = connectLocation.X;
                    LinePoint1.Y = connectLocation.Y;
                    LinePoint2.X = e.X;
                    LinePoint2.Y = e.Y;
                }
                else
                {
                    if (parentManager.rDownTo)
                        n = parentManager.p2.LeftBusBound - n;
                    else
                        n = parentManager.p2.LeftBusBound + n;
                    if (parentManager.assign.ContainsValue(n))
                        parentManager.assign.RemoveAt(parentManager.assign.IndexOfValue(n));
                    ncon = n;
                    drawLine = true;
                    LinePoint1.X = connectLocation.X;
                    LinePoint1.Y = connectLocation.Y;
                    LinePoint2.X = e.X;
                    LinePoint2.Y = e.Y;
                }
                drawConnect = false;
            }
            Invalidate();
            parent.viewList();
        }

        private void redactorPP_MouseUp(object sender, MouseEventArgs e)
        {
            if (drawConnect)
            {
                int n = (int)((connectLocation.Y - otstyp - y) / dy + .5);
                if (connectLocation.X == ot)
                {
                    if (parentManager.lDownTo)
                        n = parentManager.p1.LeftBusBound - n;
                    else
                        n = parentManager.p1.LeftBusBound + n;
                    if (parentManager.assign.ContainsKey(n))
                        parentManager.assign.RemoveAt(parentManager.assign.IndexOfKey(n));
                    parentManager.assign.Add(n, ncon);
                    drawLine = false;
                }
                else
                {
                    if (parentManager.rDownTo)
                        n = parentManager.p2.LeftBusBound - n;
                    else
                        n = parentManager.p2.LeftBusBound + n;
                    if (parentManager.assign.ContainsValue(n))
                        parentManager.assign.RemoveAt(parentManager.assign.IndexOfValue(n));
                    parentManager.assign.Add(ncon, n);
                    drawLine = false;
                }
            }
            else
                drawLine = false;
            Invalidate();
            parent.viewList();
        }
    }
}