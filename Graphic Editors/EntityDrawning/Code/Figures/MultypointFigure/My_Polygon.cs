using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;

namespace Schematix.EntityDrawning
{
    [Serializable]
    public class My_Polygon : My_MultypointFigure
    {
        public override void Fix()
        {
            draw = Draw;
            base.Fix();
        }

        public My_Polygon(EntityDrawningCore core) : base(core)
        {
            draw = DrawCreateNew;
        }

        public My_Polygon(My_Polygon item)
            : base(item)
        {
            draw = Draw;
        }

        private new void DrawCreateNew(object sender, PaintEventArgs e)
        {
            Graphics dc = e.Graphics;
            if (Points.Length >= 2)
                dc.DrawLines(pen, Points);

            Brush br = new SolidBrush(Color.Black);
            foreach (Point p in Points)
            {
                dc.FillRectangle(br, new Rectangle(p.X - 2, p.Y - 2, 4, 4));
            }
        }

        public override void AddToGraphicsPath(System.Drawing.Drawing2D.GraphicsPath path)
        {
            path.AddPolygon(Points);
        }

        private new void Draw(object sender, PaintEventArgs e)
        {
            Graphics dc = e.Graphics;

            if (selected == false)
            {
                dc.FillPolygon(brush, Points);
                dc.DrawPolygon(pen, Points);
            }
            else
            {
                Brush brush2 = new SolidBrush(Color.FromArgb(128, EntityDrawningCore.SelectedColor));
                Pen pen2 = new Pen(Color.FromArgb(128, EntityDrawningCore.SelectedColor));

                dc.FillPolygon(brush2, Points);
                dc.DrawPolygon(pen2, Points);

                Brush br = new SolidBrush(Color.Black);
                foreach (Point p in Points)
                {
                    dc.FillRectangle(br, new Rectangle(p.X - 2, p.Y - 2, 4, 4));
                }
            }
        }

        public override bool IsSelected(Point pt)
        {
            if (base.IsSelected(pt) == true)
                return true;

            Point[] arr = Points;
            int count = 0; // колиество пересечений
            for (int i = 0; i < arr.Length; i++)
            {
                Point p1, p2;
                if (i < arr.Length - 1)
                {
                    p1 = new Point(arr[i].X - pt.X, arr[i].Y - pt.Y);
                    p2 = new Point(arr[i + 1].X - pt.X, arr[i + 1].Y - pt.Y);
                }
                else
                {
                    p1 = new Point(arr[0].X - pt.X, arr[0].Y - pt.Y);
                    p2 = new Point(arr[arr.Length - 1].X - pt.X, arr[arr.Length - 1].Y - pt.Y);
                }
                if (((p1.Y > 0) && (p2.Y <= 0)) || ((p1.Y <= 0) && (p2.Y > 0)))
                {
                    double k = ((double)p1.Y - (double)p2.Y) / ((double)p1.X - (double)p2.X);
                    double b = k * (double)p1.X - (double)p1.Y;
                    double res_x = (0 - b) / k;
                    if(res_x>0)
                        count++;
                }
            }
            if ((count % 2) == 0)
                return false;
            else
                return true;
        }
    }
}