using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;


namespace Schematix.FSM
{
    public class My_Paper
    {
        public Color BGColor;
        public Color LineColor;
        private Schematix.FSM.Constructor_Core core;
        private Schematix.FSM.Constructor_UserControl form;
        public Double scale;

        public bool DrawGrig;
        public bool DrawBorder;

        private bool showLinePriority = true;
        public bool ShowLinePriority
        {
            get { return showLinePriority; }
            set { showLinePriority = value; }
        }
        

        public Point ClientStartPoint
        {
            get
            {
                Point pt = new Point(0, 0);
                pt.X = form.ClientRectangle.X;
                pt.Y = form.ClientRectangle.Y; //+ form.menuStrip.Height + form.toolStrip1.Height;
                return pt;
            }
        }

        public My_Paper(Schematix.FSM.Constructor_Core core)
        {
            this.core = core;
            this.form = core.form;
            if(form != null)
                this.BGColor = form.BackColor;
            this.scale = 1;
            DrawBorder = true;
            DrawGrig = true;
            LineColor = Color.Green;
        }

        public void Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(LineColor);
            pen.DashStyle = DashStyle.Dash;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            Graphics graphics = e.Graphics;
            Rectangle rect = new Rectangle(new Point(0, 0), core.Graph.MaxSize);
            graphics.FillRectangle(new SolidBrush(BGColor), rect);
            if (DrawGrig == true)
            {
                for (int x = 0; x < rect.Width; x += 50)
                {
                    graphics.DrawLine(pen, new Point(x, 0), new Point(x, rect.Height));
                }
                for (int y = 0; y < rect.Height; y += 50)
                {
                    graphics.DrawLine(pen, new Point(0, y), new Point(rect.Width, y));
                }
            }
            if (DrawBorder == true)
            {
                pen.Width = 5;
                graphics.DrawRectangle(pen, rect);
            }
        }

        public Point ConvertToBitmapCoordinate(MouseEventArgs e)
        {
            int hScrollValue = form.hScrollBar1.Value;
            int vScrollValue = form.vScrollBar1.Value;
            Point res = new Point(0, 0);
            res.X = (int)((e.X - ClientStartPoint.X) * scale + hScrollValue * scale);
            res.Y = (int)((e.Y - ClientStartPoint.Y) * scale + vScrollValue * scale);
            return res;
        }

        public Point ConvertToBitmapCoordinate(Point pt)
        {
            int hScrollValue = form.hScrollBar1.Value;
            int vScrollValue = form.vScrollBar1.Value;
            Point res = new Point(0, 0);
            res.X = (int)((pt.X - ClientStartPoint.X) * scale + hScrollValue * scale);
            res.Y = (int)((pt.Y - ClientStartPoint.Y) * scale + vScrollValue * scale);
            return res;
        }

        public void ChangeScroll()
        {
            Rectangle RealSquare = form.ClientRectangle; // реальная рабочая поверхность
            RealSquare.Height -= (form.hScrollBar1.Height);
            RealSquare.Width -= (form.vScrollBar1.Width);

            Rectangle FullSquare = new Rectangle(new Point(0, 0), core.Graph.MaxSize); // Размер картинки с учетом масштаба
            FullSquare.Width = (int)((double)FullSquare.Width / scale);
            FullSquare.Height = (int)((double)FullSquare.Height / scale);

            // устанавливаем границы для скролбара
            if (RealSquare.Width > FullSquare.Width)
            {
                form.hScrollBar1.Visible = false;
                form.hScrollBar1.Maximum = 0;
            }
            else
            {
                form.hScrollBar1.Visible = true;
                form.hScrollBar1.Maximum = Math.Abs(FullSquare.Width - RealSquare.Width) + form.vScrollBar1.Width;
            }

            if (RealSquare.Height > FullSquare.Height)
            {
                form.vScrollBar1.Visible = false;
                form.vScrollBar1.Maximum = 0;
            }
            else
            {
                form.vScrollBar1.Visible = true;
                form.vScrollBar1.Maximum = Math.Abs(FullSquare.Height - RealSquare.Height) + form.hScrollBar1.Height;
            }


            if (core.Bitmap != null)
                core.Bitmap.UpdateBitmap();
        }
    }
}