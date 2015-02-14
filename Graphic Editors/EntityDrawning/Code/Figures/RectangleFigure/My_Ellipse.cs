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
    public class My_Ellipse : My_RectangleFigure
    {

        public My_Ellipse(EntityDrawningCore core)
            : base(core)
        {
            draw = Draw;
        }

        public My_Ellipse(EntityDrawningCore core, Rectangle rect)
            : base(core, rect)
        {
            draw = Draw;
        }

        public My_Ellipse(My_Ellipse item)
            : base(item)
        {
            this.rect = item.rect;
            draw = Draw;
        }

        public override void AddToGraphicsPath(System.Drawing.Drawing2D.GraphicsPath path)
        {
            path.AddEllipse(rect);
        }

        private new void Draw(object sender, PaintEventArgs e)
        {
            Graphics dc = e.Graphics;
            if (selected == false)
            {
                dc.FillEllipse(brush, rect);
                dc.DrawEllipse(pen, rect);
            }
            else
            {
                Brush brush2 = new SolidBrush(Color.FromArgb(128, EntityDrawningCore.SelectedColor));
                Pen pen2 = new Pen(Color.FromArgb(128, EntityDrawningCore.SelectedColor));

                dc.FillEllipse(brush2, rect);
                dc.DrawEllipse(pen2, rect);

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

            //данные для еллипса
            double a = (double)(rect.Width / 2);
            double b = (double)(rect.Height / 2);
            //x^2/a^2 + y^2/b^2 = 1;

            double x = pt.X - (rect.Left + rect.Width/2);
            double y = pt.Y - (rect.Top + rect.Height/2);
            double y1 = Math.Sqrt(((b * b) / (a * a)) * (a * a - x * x));
            double y2 = -y1;

            if ((y > y2) && (y < y1))
                return true;
            else
                return false;
        }        
    }
}