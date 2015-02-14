using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Schematix.EntityDrawning
{
    public class GroupSelector
    {
        public Rectangle rect;

        EntityDrawningCore core;

        Point start_point;
        Point end_point;

        public bool active = false;

        public GroupSelector(EntityDrawningCore core)
        {
            this.core = core;
            rect = new Rectangle(new Point(0, 0), new Size(0, 0));
        }

        public void Draw(object sender, PaintEventArgs e)
        {
            if ((rect.Width < 10) || (rect.Height < 10))
                return;

            Pen pen = new Pen(Color.Black, 1);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            e.Graphics.DrawRectangle(pen, rect);
        }

        public void MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
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

            core.Form.Invalidate();
        }

        public void MouseDown(object sender, MouseEventArgs e)
        {
            rect.Size = new Size(1, 1);
            rect.Location = new Point(e.X, e.Y);
            start_point = new Point(e.X, e.Y);
        }

        public void MouseUp(object sender, MouseEventArgs e)
        {
            active = false;
            core.Form.Invalidate();
        }

        private void SelectFigures()
        {
            core.UnselectAll();
            List<My_Figure> lst = core.Picture.GetSelectedFigure(rect);
            foreach (My_Figure fig in lst)
            {
                fig.selected = true;
                core.SelectedFigures.Add(fig);
            }
        }
    }
}