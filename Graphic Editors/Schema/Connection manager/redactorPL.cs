using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace csx
{
    public partial class redactorPL : Form
    {
        private int ncon = 0;
        private float y = 0, x = 0;
        private float scrollW = 20;
        //private float scrollH = 20;
        private connectFormPL parent;
        private cnManager parentManager;
        private float ot = 60;
        private float otX = 30;
        private float ot0 = 25;
        private float ot1 = 35;
        private float fontR = 25;
        private float fontH = 7;
        private Pen p = Pens.Black;
        private Font f = new Font("Courier New", 12, GraphicsUnit.Pixel); //при высоте 15 ширина 9

        private float otstyp = 100;
        private float otstypX = 70;
        private bool drawConnect = false;
        private PointF connectLocation;
        private bool drawLine = false;
        private PointF LinePoint1, LinePoint2;
        private float dy = 0, dx = 0;

        public redactorPL(object parentF, object parentM, string namePort, string nameLine1, string nameLine2)
        {            
            parentManager = (cnManager)parentM;
            parent = (connectFormPL)parentF;
            InitializeComponent();
            portName.Text = namePort;
            lineName1.Text = nameLine1;
            lineName2.Text = nameLine2;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
        }

        private void redactorPL_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            int i;
            float dd = otstypX;
            int di = Math.Sign(parentManager.p1.RightBusBound - parentManager.p1.LeftBusBound);
            g.RotateTransform(90);
            for (i = parentManager.p1.LeftBusBound; i != parentManager.p1.RightBusBound + di; i += di)
            {
                g.DrawString(i.ToString(), f, p.Brush, 0, - dd - fontH - x);
                g.DrawLine(p, ot0, -dd - x, ot, -dd - x);
                g.DrawLine(p, ot, -dd - x - 1, ot, -dd - x + 1);
                dd += dx;
            }
            g.RotateTransform(-90);
            i = 0;
            dd = otstyp;
            foreach (KeyValuePair<int, int> ind in parentManager.ln.assign)           
            {
                g.DrawString("("+i.ToString()+")", f, p.Brush, 0, dd - fontH + y);
                g.DrawString(ind.Key.ToString(), f, p.Brush, ot1, dd - fontH + y);
                g.DrawString(ind.Value.ToString(), f, p.Brush, Width - fontR - scrollW, dd - fontH + y);
                g.DrawLine(p, ot0 + ot1, dd + y, Width - ot0 - scrollW, dd + y);
                g.DrawLine(p, otX + ot1, dd + y - 1, otX + ot1, dd + y + 1);
                g.DrawLine(p, Width - otX - scrollW, dd + y - 1, Width - otX - scrollW, dd + y + 1);
                dd += dy;
                i++;
            }
            foreach (KeyValuePair<int, int> ind in parentManager.assign)
            {
                g.DrawLine(p, otstypX + dx * Math.Abs(ind.Key - (parentManager.lDownTo ? parentManager.lrB : parentManager.llB)) + x, ot + y, otstypX + dx * Math.Abs(ind.Key - (parentManager.lDownTo ? parentManager.lrB : parentManager.llB)) + x, otstyp + dy * ind.Value + y);
                g.FillEllipse(p.Brush, otstypX + dx * Math.Abs(ind.Key - (parentManager.lDownTo ? parentManager.lrB : parentManager.llB)) + x - 3, otstyp + dy * ind.Value + y - 3, 6, 6);
            }
            if (drawLine)
                g.DrawLine(Pens.Red, LinePoint1.X, LinePoint1.Y, LinePoint2.X, LinePoint2.Y);
            if (drawConnect)
                g.FillEllipse(Brushes.Green, connectLocation.X - 4, connectLocation.Y - 4, 8, 8);
        }

        private void onResize()
        {
            lineName2.Left = this.Width - vS.Width - lineName2.Width;
            //Dock = DockStyle.Fill;
            dy = (Height - otstyp - 70) / Math.Abs(parentManager.rrB - parentManager.rlB);
            if (dy < 20)
            {
                //Dock = DockStyle.Top;
                dy = 20;
                vS.Visible = true;
                scrollW = 30;
                vS.Minimum = 0;
                vS.Maximum = 20 * Math.Abs(parentManager.rrB - parentManager.rlB) + (int)otstyp + 95 - Height;
                vS.Value = 0;
                y = 0;
            }
            else
            {
                vS.Visible = false;
                y = 0;
                scrollW = 10;
            }
            dx = (Width - 2 * otstypX) / Math.Abs(parentManager.lrB - parentManager.llB);
            if (dx < 20)
            {
                dx = 20;
                hS.Visible = true;
                //scrollH = 30;
                hS.Minimum = 0;
                hS.Maximum = 20 * Math.Abs(parentManager.lrB - parentManager.llB);
                hS.Value = 0;
                x = 0;
            }
            else
            {
                hS.Visible = false;
                x = 0;
                //scrollH = 10;
            }
            drawConnect = false;
            drawLine = false;
        }

        private void redactorPL_Load(object sender, EventArgs e)
        {
            onResize();
        }

        private void redactorPL_MouseMove(object sender, MouseEventArgs e)
        {
            int n = (int)((e.X - otstypX - x) / dx + .5);
            if (Math.Abs(e.Y - ot) < 9)
                if (n <= parentManager.lrB - parentManager.llB && Math.Abs(n * dx + otstypX - e.X + x) < 9 && !drawLine)
                {
                    drawConnect = true;
                    connectLocation.X = n * dx + otstypX + x;
                    connectLocation.Y = ot;
                }
                else
                    drawConnect = false;
            else
                drawConnect = false;            
            if (e.Y < 20)
                if (vS.Visible && vS.Value - 10 > vS.Minimum)
                {
                    vS.Value -= 10;
                    LinePoint1.Y += 10;
                    y = -vS.Value;
                    this.Invalidate();
                }  
            if (e.Y > Height - 70)
                if (vS.Visible && vS.Value + 10 < vS.Maximum)
                {
                    vS.Value += 10;
                    LinePoint1.Y -= 10;
                    y = -vS.Value;
                }
            if (drawLine)
            {
                LinePoint2.Y = e.Y;
                n = (int)((e.Y - otstyp - y) / dy + .5);
                if (n < parentManager.ln.assign.Count && Math.Abs(n * dy + otstyp - e.Y + y) < 9)
                {
                    drawConnect = true;
                    connectLocation.X = LinePoint1.X;
                    connectLocation.Y = n * dy + otstyp + y;
                }
            }
            this.Invalidate();
        }

        private void redactorPL_Resize(object sender, EventArgs e)
        {
            onResize();
        }

        private void vS_Scroll(object sender, ScrollEventArgs e)
        {
            y = - vS.Value;
            this.Invalidate();
        }

        private void hS_Scroll(object sender, ScrollEventArgs e)
        {
            x = -hS.Value;
            this.Invalidate();
        }

        private void redactorPL_MouseDown(object sender, MouseEventArgs e)
        {
            if (drawConnect)
            {
                int n = (int)((connectLocation.X - otstypX - x) / dx + .5);                
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
                LinePoint2.X = connectLocation.X;
                LinePoint2.Y = e.Y;                
                drawConnect = false;
            }
            Invalidate();
            parent.viewList();
        }

        private void redactorPL_MouseUp(object sender, MouseEventArgs e)
        {
            if (drawConnect)
            {
                int n = (int)((connectLocation.Y - otstyp - y) / dy + .5);
                //if (parentManager.lDownTo)
                //    n = parentManager.p1.LeftBusBound - n;
                //else
                //    n = parentManager.p1.LeftBusBound + n;
                if (parentManager.assign.ContainsValue(n))
                    parentManager.assign.RemoveAt(parentManager.assign.IndexOfValue(n));
                parentManager.assign.Add(ncon, n);
                drawConnect = false;
                drawLine = false;
            }
            else
            {
                drawLine = false;
                drawConnect = false;
            }
            Invalidate();
            parent.viewList();
        }
    }
}