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
    public class My_Polyline : My_MultypointFigure
    {        
        public My_Polyline(EntityDrawningCore core)
            : base(core)
        {
            draw = DrawCreateNew;
        }

        public My_Polyline(My_Polyline item)
            : base(item)
        {
            draw = Draw;
        }

        public override void Fix()
        {
            draw = Draw;
            base.Fix();
        }

        public override Point CenterPoint
        {
            get
            {
                return Points[0];
            }
            set
            {
                base.CenterPoint = value;
            }
        }

        private new void DrawCreateNew(object sender, PaintEventArgs e)
        {
            if (Points.Length <= 1)
                return;
            Graphics dc = e.Graphics;
            dc.DrawLines(pen, Points);

            Brush br = new SolidBrush(Color.Black);
            foreach (Point p in Points)
            {
                dc.FillRectangle(br, new Rectangle(p.X - 2, p.Y - 2, 4, 4));
            }
        }

        public override void AddToGraphicsPath(System.Drawing.Drawing2D.GraphicsPath path)
        {
            path.AddLines(Points);
        }

        private new void Draw(object sender, PaintEventArgs e)
        {
            Graphics dc = e.Graphics;

            if (selected == false)
            {
                //  dc.FillPolygon(brush, Points);
                if (Points.Length >= 2)
                    dc.DrawLines(pen, Points);
            }
            else
            {
                Brush brush2 = new SolidBrush(Color.FromArgb(128, EntityDrawningCore.SelectedColor));
                Pen pen2 = new Pen(Color.FromArgb(128, EntityDrawningCore.SelectedColor));

                //   dc.FillPolygon(brush2, Points);
                dc.DrawLines(pen2, Points);

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
            for (int i = 0; i < arr.Length - 1; i++)
            {
                Point point1 = arr[i];
                Point point2 = arr[i + 1];
                if (Math.Abs(point2.X - point1.X) < 4)
                {
                    if (Math.Abs(point1.X - pt.X) < 2)
                        return true;
                }
                double k = ((double)point1.Y - (double)point2.Y) / ((double)point1.X - (double)point2.X);
                double b = (double)point2.Y - k * (double)point2.X;

                double Y2 = k * pt.X + b;
                if (Math.Abs(Y2 - (double)pt.Y) < 5)
                {
                    Point Location = new Point(
                                                Math.Min(point1.X, point2.X),
                                                Math.Min(point1.Y, point2.Y));
                    Location.X -= 2;
                    Location.Y -= 2;
                    Size size = new Size(
                                        Math.Max(point1.X - Location.X, point2.X - Location.X),
                                        Math.Max(point1.Y - Location.Y, point2.Y - Location.Y));
                    size.Width += 4;
                    size.Height += 4;
                    Rectangle rect = new Rectangle(Location, size);
                    return rect.Contains(pt);
                }
            }
            return false;
        }
    }
}
