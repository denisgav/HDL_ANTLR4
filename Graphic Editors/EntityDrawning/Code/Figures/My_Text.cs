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
    public class My_Text : My_Figure
    {
        private Point center_point;
        public My_Figure Owner;
        public override Point CenterPoint
        {
            get
            {
                if (Owner != null)
                    return new Point(center_point.X + Owner.CenterPoint.X, center_point.Y + Owner.CenterPoint.Y);
                else
                    return center_point;
            }
            set
            {
                if (Owner != null)
                {
                    center_point.X = value.X - Owner.CenterPoint.X;
                    center_point.Y = value.Y - Owner.CenterPoint.Y;
                }
                else
                    center_point = value;
            }
        }
        private Rectangle rect;
        private Point start_point; // используется для перемещения
        private Point start_location;// используется для перемещения

        public string Text { get; set; }
        public Font Font { get; set; }
        
        public override Point[] Points
        {
            get
            {
                return new Point[1]{CenterPoint};
            }
        }

        public My_Text(EntityDrawningCore core) : base(core)
        {
            Text = "Hello World";
            Font = new Font("Times New Roman", 10);
            brush = new My_SolidBrush(Color.Black);
            pen = new My_Pen(Color.Empty);

            draw = Draw;
            mouse_move = MouseMoveCreateNew;
            mouse_down = MouseDownCreateNew;
            mouse_up = MouseUpCreateNew;
        }

        public My_Text(EntityDrawningCore core, string Text, Point pt)
            : base(core)
        {
            this.core = core;
            this.Text = Text;
            Font = new Font("Times New Roman", 10);
            brush = new My_SolidBrush(Color.Black);
            center_point = pt;
            pen = new My_Pen(Color.Empty);

            draw = Draw;
            mouse_move = MouseMove;
            mouse_down = MouseDown;
            mouse_up = MouseUp;
        }

        public My_Text(My_Text item)
            : base(item)
        {
            this.Text = item.Text;
            this.center_point = item.center_point;
            this.Font = item.Font;

            mouse_move = MouseMove;
            mouse_down = MouseDown;
            mouse_up = MouseUp;
            draw = Draw;
        }

        public override void AddToGraphicsPath(System.Drawing.Drawing2D.GraphicsPath path)
        {
        }

        private new void Draw(object sender, PaintEventArgs e)
        {
            Graphics dc = e.Graphics;
            SizeF size = dc.MeasureString(Text, Font);
            size.Width += Font.Height;
            rect.Location = new Point(CenterPoint.X - (int)(size.Width/2), CenterPoint.Y - (int)(size.Height/2));
            rect.Size = new Size((int) size.Width, (int)size.Height);
            if (selected == true)
                dc.FillRectangle(new SolidBrush(EntityDrawningCore.SelectedColor), rect);
            dc.DrawRectangle(pen, rect);
            System.Drawing.StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            dc.DrawString(Text, Font, brush, rect, format);
        }

        private new void MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point pt = new Point(e.X, e.Y);
                int delta_x = -pt.X + start_point.X;
                int delta_y = -pt.Y + start_point.Y;
                CenterPoint = new Point (start_location.X - delta_x, start_location.Y - delta_y);
                if(core != null)
                    core.Form.Invalidate();
            }
        }

        private new void MouseMoveCreateNew(object sender, MouseEventArgs e)
        {
            CenterPoint = new Point(e.X, e.Y);
            if (core != null)
                core.Form.Invalidate();
        }

        private new void MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                start_point = new Point(e.X, e.Y);
                start_location = CenterPoint;
            }
            if ((e.Button == MouseButtons.Right) && (core != null))
                core.Form.contextMenuStriptext.Show(Control.MousePosition);
        }

        private new void MouseDownCreateNew(object sender, MouseEventArgs e)
        {
            mouse_move = MouseMove;
            mouse_down = MouseDown;
            mouse_up = MouseUp;
        }

        private new void MouseUp(object sender, MouseEventArgs e)
        {
            brush.UpdateBrush(this);
            //core.AddToHistory();
        }

        private new void MouseUpCreateNew(object sende, MouseEventArgs e)
        {
            //core.AddToHistory();
        }

        public override bool IsSelected(Point pt)
        {
            if ((pt.X >= rect.Left) && (pt.X <= rect.Right) && (pt.Y >= rect.Top) && (pt.Y <= rect.Bottom))
                return true;
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