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
    public class My_Pie : My_EllipsePart
    {       
        public My_Pie(EntityDrawningCore core)
            : base(core)
        {
            draw = Draw;
        }

        public My_Pie(My_Pie item)
            : base(item)
        {
            draw = Draw;
        }

        public override void AddToGraphicsPath(System.Drawing.Drawing2D.GraphicsPath path)
        {
            path.AddPie(rect, startAngle, sweepAngle);
        }

        private new void Draw(object sender, PaintEventArgs e)
        {
            Graphics dc = e.Graphics;
            if ((rect.Width < 20) || (rect.Height < 20))
                return;
            if (selected == false)
            {
                dc.FillPie(brush, rect, startAngle, sweepAngle);
                dc.DrawPie(pen, rect, startAngle, sweepAngle);
            }
            else
            {
                Pen pen2 = new Pen(Color.FromArgb(128, EntityDrawningCore.SelectedColor));
                Brush brush2 = new SolidBrush(Color.FromArgb(128, EntityDrawningCore.SelectedColor));

                AdjustableArrowCap My_Cap = new AdjustableArrowCap(5, 10);
                My_Cap.BaseCap = LineCap.Triangle;
                pen2.CustomEndCap = My_Cap;
                pen2.CustomStartCap = My_Cap;

                System.Drawing.StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;
                dc.DrawString("startAngle = " + startAngle.ToString() + "\n sweepAngle = " + sweepAngle.ToString(), new Font("Times New Roman", 10, FontStyle.Bold), new SolidBrush(Color.Black), rect, format);

                dc.DrawPie(pen2, rect, startAngle, sweepAngle);
                dc.FillPie(brush2, rect, startAngle, sweepAngle);

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

            if ((pt.X < rect.Left - 2) || (pt.X > rect.Right + 2) || (pt.Y < rect.Top - 2) || (pt.Y > rect.Bottom + 2))
                return false;

            //данные для еллипса
            double a = (double)(rect.Width / 2);
            double b = (double)(rect.Height / 2);
            //x^2/a^2 + y^2/b^2 = 1;

            double x = pt.X - (rect.Left + rect.Width / 2);
            double y = pt.Y - (rect.Top + rect.Height / 2);
            double y1 = Math.Sqrt(((b * b) / (a * a)) * (a * a - x * x));
            double y2 = -y1;

            if ((y > y2) && (y < y1))
                return true;
            else
                return false;
        }
    }
}