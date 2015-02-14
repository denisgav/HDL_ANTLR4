using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing.Drawing2D;

namespace Schematix.EntityDrawning
{
    [Serializable]
    public class My_Arc : My_EllipsePart
    {
        public My_Arc(EntityDrawningCore core)
            : base(core)
        {
            draw = Draw;
        }

        public My_Arc(My_Arc item)
            : base(item)
        {
            draw = Draw;
        }

        public My_Arc(EntityDrawningCore core, Rectangle rect)
            :base(core, rect)
        {
            draw = Draw;
        }

        public override void AddToGraphicsPath(System.Drawing.Drawing2D.GraphicsPath path)
        {
            path.AddArc(rect, startAngle, sweepAngle);
        }

        private new void Draw(object sender, PaintEventArgs e)
        {
            Graphics dc = e.Graphics;
            if ((rect.Width < 20) || (rect.Height < 20))
                return;
            if (selected == false)
            {
                dc.DrawArc(pen, rect, startAngle, sweepAngle);
            }
            else
            {
                Pen pen2 = new Pen(Color.FromArgb(128, EntityDrawningCore.SelectedColor));

                AdjustableArrowCap My_Cap = new AdjustableArrowCap(5, 10);
                My_Cap.BaseCap = LineCap.Triangle;
                pen2.CustomEndCap = My_Cap;
                pen2.CustomStartCap = My_Cap;

                System.Drawing.StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;
                dc.DrawString("startAngle = " + startAngle.ToString() + "\n sweepAngle = " + sweepAngle.ToString(), new Font("Times New Roman", 10, FontStyle.Bold), new SolidBrush(Color.Black), rect, format);

                dc.DrawArc(pen2, rect, startAngle, sweepAngle);

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

            if ((pt.X < rect.Left-2) || (pt.X > rect.Right+2) || (pt.Y < rect.Top-2) || (pt.Y > rect.Bottom+2))
                return false;

            //данные для еллипса
            double a = (double)(rect.Width / 2);
            double b = (double)(rect.Height / 2);
            //x^2/a^2 + y^2/b^2 = 1;

            double x = pt.X - (rect.Left + rect.Width / 2);
            double y = pt.Y - (rect.Top + rect.Height / 2);
            double y1 = Math.Sqrt(((b * b) / (a * a)) * (a * a - x * x));
            double y2 = -y1;

            if ((Math.Abs(y - y1) < 5) || (Math.Abs(y - y2) < 5))
            {
                return true;
            }
            else
                return false;
        }

        public override bool IsSelected(Rectangle rt)
        {
            if ((rt.Left <= rect.Left) && (rt.Right >= rect.Right) && (rt.Top <= rect.Top) && (rt.Bottom >= rect.Bottom))
                return true;
            else
                return false;
        }
    }
}