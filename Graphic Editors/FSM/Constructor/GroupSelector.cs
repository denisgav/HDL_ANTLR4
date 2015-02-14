using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;


namespace Schematix.FSM
{
    public class GroupSelector
    {
        public Rectangle rect; // выделенная область формы
        bool change_rect = false;
        private Schematix.FSM.Constructor_Core core;
        Point start_point;
        Point end_point;

        public bool active = false;

        public GroupSelector(Schematix.FSM.Constructor_Core core)
        {
            this.core = core;
            rect = new Rectangle(new Point(0, 0), new Size(0, 0));
        }

        public void Draw(object sender, PaintEventArgs e)
        {
            if ((rect.Width < 10) || (rect.Height < 10))
                return;

            Brush brush = new SolidBrush(Color.Empty);
            Pen pen = new Pen(Color.Black, 1);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.DrawRectangle(pen, rect);
            e.Graphics.FillRectangle(brush, rect);
        }

        private void SelectFigures()
        {
            core.Graph.UnselectAllFigures();
            List<My_Figure> sel_figures = new List<My_Figure>();

            foreach (My_Figure fig in core.Graph.Figures)
            {
                if (fig.Select(rect) == true)
                {
                    sel_figures.Add(fig);
                }
            }

            if (core.Graph.Reset != null)
            {
                if (core.Graph.Reset.Select(rect) == true)
                {
                    sel_figures.Add(core.Graph.Reset);
                }
            }

            core.Graph.SelectFigures(sel_figures);            
        }

        public void MouseMove(object sender, MouseEventArgs e)
        {
            if (change_rect == false)
                return;

            end_point = new Point(e.X, e.Y);

            rect.Location = new Point(Math.Min(start_point.X, end_point.X), Math.Min(start_point.Y, end_point.Y));
            int width = Math.Abs(start_point.X - end_point.X);
            int height = Math.Abs(start_point.Y - end_point.Y);
            if (width < 1)
                width = 1;
            if (height < 1)
                height = 1;

            rect.Size = new Size(width, height);

            SelectFigures();

            core.form.Invalidate();
        }

        public void MouseDown(object sender, MouseEventArgs e)
        {
            change_rect = true;
            rect.Size = new Size(1, 1);
            rect.Location = new Point(e.X, e.Y);
            start_point = new Point(e.X, e.Y);
        }

        public void MouseUp(object sender, MouseEventArgs e)
        {
            change_rect = false;
            active = false;
            core.Lock = false;
            core.form.Invalidate();
        }
    }
}