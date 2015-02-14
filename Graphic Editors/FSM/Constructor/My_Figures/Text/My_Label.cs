using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Schematix.FSM
{
    [Serializable]
    public class My_Label : My_Figure
    {
        public string Text { get; set; }

        public My_Figure Owner;

        private Point start_point; // используется для перемещения

        private Point center_point;
        public new Point CenterPoint
        {
            get
            {
                Point pt = new Point(center_point.X + Owner.CenterPoint.X, center_point.Y + Owner.CenterPoint.Y);
                return pt;
            }
            set
            {
                center_point.X = value.X - Owner.CenterPoint.X;
                center_point.Y = value.Y - Owner.CenterPoint.Y;
            }
        }

        public Color Color { get; set; }
        private Color color_label;
        private Rectangle rt;

        public override Size MaxSize
        {
            get
            {
                return new Size(rt.Width, rt.Height);
            }
        }

        public override void Draw(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            Graphics dc = e.Graphics;
            Brush brush;
            Pen pen = new Pen(Color.Black);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            if (selected == false)
                brush = new SolidBrush(Color);
            else
                brush = new SolidBrush(SelectedColor);

            if (string.IsNullOrEmpty(Text) == false)
            {
                Font font = new Font("Times New Roman", 10, FontStyle.Bold);

                SizeF size = dc.MeasureString(Text, font);
                size.Width = size.Width + font.Height;
                rt.Location = new Point(CenterPoint.X - (int)size.Width / 2, CenterPoint.Y - (int)size.Height / 2);
                rt.Size = new Size((int)size.Width, (int)size.Height);

                dc.DrawRectangle(pen, rt);
                System.Drawing.StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;
                dc.DrawString(Text, font, new SolidBrush(Color.Black), rt, format);
            }
        }

        public override void Draw_Bitmap(object sender, PaintEventArgs e)
        {
            Font font = new Font("Times New Roman", 10, FontStyle.Bold);
            Graphics dc = e.Graphics;

            SizeF size = dc.MeasureString(Text, font);
            size.Width = size.Width + font.Height;
            rt.Location = new Point(CenterPoint.X - (int)size.Width / 2, CenterPoint.Y - (int)size.Height / 2);
            rt.Size = new Size((int)size.Width, (int)size.Height);

            Brush brush = new SolidBrush(color_label);

            dc.FillRectangle(brush, rt);
        }

        public My_Label(string text, Color color, Schematix.FSM.Constructor_Core core, My_Figure Owner)
        {
            this.Owner = Owner;
            this.Color = color;
            this.Text = text;
            this.core = core;

            mouse_down = MouseDown;
            mouse_move = MouseMove;
            mouse_up = MouseUp;
            draw = Draw;

            UpdateBitmapColors();
        }

        public My_Label(My_Label item)
        {
            this.Color = item.Color;
            this.Text = item.Text;
            this.core = item.core;
            this.center_point = item.center_point;

            mouse_down = MouseDown;
            mouse_move = MouseMove;
            mouse_up = MouseUp;
            draw = Draw;

            UpdateBitmapColors();
        }

        public override void UpdateBitmapColors()
        {
            color_label = GenerateRandomColor();
        }

        public override void MouseMove(object sender, MouseEventArgs e)
        {
        }

        public override void MouseMoveResize(object sender, MouseEventArgs e)
        {
            Point pt = new Point(e.X, e.Y);
            pt = new Point(-(this.start_point.X - pt.X), -(this.start_point.Y - pt.Y));
            CenterPoint = pt;

            if (CenterPoint.X < 0)
                CenterPoint = new Point(0, CenterPoint.Y);
            if (CenterPoint.Y < 0)
                CenterPoint = new Point(CenterPoint.X, 0);

            core.form.Invalidate();
        }

        public override void MouseDown(object sender, MouseEventArgs e)
        {
            if (selected == false)
                return;
            Point pt = new Point(e.X - CenterPoint.X, e.Y - CenterPoint.Y);
            start_point = pt;
            if (e.Button == MouseButtons.Left)
            {
                mouse_move = MouseMoveResize;
            }
        }

        public override void MouseUp(object sender, MouseEventArgs e)
        {
            if (selected == false)
                return;

            if (e.Button == MouseButtons.Left)
            {
                mouse_move = MouseMove;
                core.Bitmap.UpdateBitmap();
                //core.AddToHistory("Label moved");
            }
        }

        public override void Select(Color point_color)
        {
            if (point_color.ToArgb() == color_label.ToArgb())
            {
                selected = true;
                return;
            }

            selected = false;
        }

        public override bool Select(Rectangle SelectedRectangle)
        {
            if (SelectedRectangle.Left > rt.Left)
            {
                return false;
            }
            if (SelectedRectangle.Right < rt.Right)
            {
                return false;
            }
            if (SelectedRectangle.Top > rt.Top)
            {
                return false;
            }
            if (SelectedRectangle.Bottom < rt.Bottom)
            {
                return false;
            }
            return true;
        }

        public override bool IsSelected(Color point_color)
        {
            if (point_color.ToArgb() == color_label.ToArgb())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}