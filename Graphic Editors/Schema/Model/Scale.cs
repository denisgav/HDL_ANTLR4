using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace csx
{
    public class Scale
    {
        public Construct parent;
        public Schematix_all.SchemaUserControl form;
        public Double scale;

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

        public Scale(Schematix_all.SchemaUserControl form, Construct parent)
        {
            this.form = form;
            this.parent = parent;
            scale = 1.0;
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

        public void UpdateScale(int zoom)
        {
            this.scale = (double)100 / (double)zoom;
            UpdateScale();
        }

        public void UpdateScale()
        {
            Rectangle RealSquare = form.ClientRectangle; // реальная рабочая поверхность
            RealSquare.Height -= (form.hScrollBar1.Height);
            RealSquare.Width -= (form.vScrollBar1.Width);

            Rectangle FullSquare = new Rectangle(new Point(0, 0), GetMaxSize()); // Размер картинки с учетом масштаба
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
                form.hScrollBar1.Maximum = Math.Abs(FullSquare.Width - RealSquare.Width) + 2 * form.vScrollBar1.Width;
            }

            if (RealSquare.Height > FullSquare.Height)
            {
                form.vScrollBar1.Visible = false;
                form.vScrollBar1.Maximum = 0;
            }
            else
            {
                form.vScrollBar1.Visible = true;
                form.vScrollBar1.Maximum = Math.Abs(FullSquare.Height - RealSquare.Height) + 2 * form.hScrollBar1.Height;
            }
        }

        public Size GetMaxSize()
        {
            Size res = new Size(0, 0);
            foreach (Element el in parent.elements)
            {
                if (el.border.Right > res.Width)
                    res.Width = (int)el.border.Right;
                if (el.border.Bottom > res.Height)
                    res.Height = (int)el.border.Bottom;
            }
            //Size res = form.ModelBorder.Size;
            return res;
        }
    }
}