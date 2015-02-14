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
    public class My_Curve : My_MultypointFigure
    {        
        private Bitmap bitmap;

        public override void Fix()
        {
            draw = Draw;
            UpdateBitmap();
            mouse_up = MouseUp;
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

        public My_Curve(EntityDrawningCore core)
            : base(core)
        {
            draw = DrawCreateNew;
            mouse_up = MouseUpCreateNew;
        }

        public My_Curve(My_Curve item)
            : base(item)
        {
            mouse_up = MouseUp;
            draw = Draw;
            UpdateBitmap();
        }

        private new void DrawCreateNew(object sender, PaintEventArgs e)
        {
            Graphics dc = e.Graphics;
            Point[] points = Points;
            if(points.Length > 1)
                dc.DrawCurve(pen, points);

            Brush br = new SolidBrush(Color.Black);
            foreach (Point p in points)
            {
                dc.FillRectangle(br, new Rectangle(p.X - 2, p.Y - 2, 4, 4));
            }
        }

        public override void AddToGraphicsPath(System.Drawing.Drawing2D.GraphicsPath path)
        {
            path.AddCurve(Points);
        }

        private new void Draw(object sender, PaintEventArgs e)
        {
            Graphics dc = e.Graphics;

            if (selected == false)
            {
                if (Points.Length >= 2)
                    dc.DrawCurve(pen, Points);
            }
            else
            {
                Brush brush2 = new SolidBrush(Color.FromArgb(128, EntityDrawningCore.SelectedColor));
                Pen pen2 = new Pen(Color.FromArgb(128, EntityDrawningCore.SelectedColor));

                //   dc.FillPolygon(brush2, Points);
                if (Points.Length >= 2)
                    dc.DrawCurve(pen2, Points);

                Brush br = new SolidBrush(Color.Black);
                foreach (Point p in Points)
                {
                    dc.FillRectangle(br, new Rectangle(p.X - 2, p.Y - 2, 4, 4));
                }
            }
        }

        protected override void MouseUp(object sender, MouseEventArgs e)
        {
            core.Lock = false;
            //core.AddToHistory();
            UpdateBitmap();
        }

        private void UpdateBitmap()
        {
            if (bitmap == null)
                bitmap = new Bitmap(core.Picture.MaxSize.Width, core.Picture.MaxSize.Height);
            else
            {
                bitmap.Dispose();
                bitmap = new Bitmap(core.Picture.MaxSize.Width, core.Picture.MaxSize.Height);
            }
            Graphics dc = Graphics.FromImage(bitmap);
            if(Points.Length > 1)
                dc.DrawCurve(new Pen(Color.Black, 4.0f), Points);
        }

        public override bool IsSelected(Point pt)
        {
            if (base.IsSelected(pt) == true)
                return true;

            if ((bitmap != null) && (pt.X < bitmap.Width) && (pt.Y < bitmap.Height) && (pt.X > 0) && (pt.Y > 0))
            {
                Color color = bitmap.GetPixel(pt.X, pt.Y);
                if (color.ToArgb().Equals(Color.Black.ToArgb()))
                    return true;
            }
            return false;
        }
    }
}
