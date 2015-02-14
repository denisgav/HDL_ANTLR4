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
    public class My_Signal : My_Figure
    {
        public string Type { get; set; }
        public struct_Type Type_inf;
        public string Default_Value { get; set; }
        private Point center_point;
        private Color color_signal;
        public Color Color { get; set; }

        [Serializable]
        public struct struct_Type
        {
            public string range1;
            public string range2;
            public bool to; // to or downto
            public bool avaliable;
        }

        private Point start_point; // используется для перемещения

        public override Point CenterPoint
        {
            get
            {
                Point pt = new Point();
                pt.X = center_point.X;
                pt.Y = center_point.Y;

                return pt;
            }

            set
            {
                center_point = value;
            }
        }

        public override Size MaxSize
        {
            get { return new Size(rect.Width, rect.Height); }
        }

        public virtual Rectangle rect
        {
            get
            {
                Rectangle rt = new Rectangle(center_point.X - 10, center_point.Y - 10, 20, 20);
                return rt;
            }
        }

        public override Constructor_Core Core
        {
            get
            {
                return base.Core;
            }
            set
            {
                base.Core = value;
                label_name.Core = value;
            }
        }

        public My_Label label_name;

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

            dc.FillEllipse(brush, rect);
            dc.DrawEllipse(pen, rect);
            dc.DrawLine(pen, this.center_point, label_name.CenterPoint);
            label_name.draw(sender, e);
        }

        public override void Draw_Bitmap(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            Graphics dc = e.Graphics;
            Brush brush = new SolidBrush(color_signal);

            dc.FillRectangle(brush, rect);

            label_name.Draw_Bitmap(sender, e);
        }

        public My_Signal(string name, string Type, string Default_Value, Point center_point, Schematix.FSM.Constructor_Core core)
        {
            Color = Color.Yellow;
            this.Type = Type;
            this.name = name;
            this.Default_Value = Default_Value;
            this.center_point = center_point;
            this.core = core;

            Type_inf.avaliable = false;
            Type_inf.range1 = "";
            Type_inf.range2 = "";

            mouse_move = MouseMove;
            mouse_up = MouseUp;
            mouse_down = MouseDown;
            draw = Draw;

            UpdateBitmapColors();
            label_name = new My_Label(name, Color, core, this);
        }

        public My_Signal(Schematix.FSM.Constructor_Core core)
        {
            Color = Color.Yellow;
            if(core.Graph.Language == FSM_Language.VHDL)
                this.Type = "Std_Logic";
            if(core.Graph.Language == FSM_Language.Verilog)
                this.Type = "reg";
            this.name = ("Variable" + core.Graph.Signals.Count.ToString());
            this.Default_Value = "'0'";
            this.center_point = new Point(0, 0);
            this.core = core;

            Type_inf.avaliable = false;
            Type_inf.range1 = "";
            Type_inf.range2 = "";

            mouse_move = MouseMoveCreateNew;
            mouse_up = MouseUpCreateNew;
            mouse_down = MouseDownCreateNew;
            draw = Draw;

            UpdateBitmapColors();
            label_name = new My_Label(name, Color, core, this);
        }

        public My_Signal(My_Signal item)
        {
            this.Color = item.Color;
            this.core = item.core;
            this.name = item.name;
            this.Default_Value = item.Default_Value;
            this.center_point = item.center_point;
            this.Type = item.Type;
            this.Type_inf = item.Type_inf;
            this.label_name = new My_Label(item.label_name);
            this.label_name.Owner = this;
            mouse_move = MouseMove;
            mouse_up = MouseUp;
            mouse_down = MouseDown;
            draw = Draw;
            UpdateBitmapColors();
        }

        public void Fix()
        {
            mouse_move = MouseMove;
            mouse_up = MouseUp;
            mouse_down = MouseDown;
            draw = Draw;
        }

        public override void MouseMove(object sender, MouseEventArgs e)
        {

        }

        public override void MouseMoveCreateNew(object sender, MouseEventArgs e)
        {
            center_point = new Point(e.X, e.Y);
            core.form.Invalidate();
        }

        public override void MouseUpCreateNew(object sender, MouseEventArgs e)
        {
            core.Bitmap.UpdateBitmap();
            mouse_up = MouseUp;
            mouse_move = MouseMove;
            mouse_down = MouseDown;
        }

        public override void MouseDownCreateNew(object sender, MouseEventArgs e)
        {
            center_point = new Point(e.X, e.Y);
            core.form.Invalidate();
        }

        private new void MouseMoveResize(object sender, MouseEventArgs e)
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
            if (e.Button == MouseButtons.Right)
            {
                Point p = new Point(Control.MousePosition.X, Control.MousePosition.Y);
                core.form.contextMenuStrip.Show(p);
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
                //core.AddToHistory("Signal " + name + " moved");
            }
        }

        public override void Select(Color point_color)
        {
            if (point_color.ToArgb() == color_signal.ToArgb())
            {
                selected = true;
                return;
            }

            selected = false;
        }

        public override bool Select(Rectangle SelectedRectangle)
        {
            if (SelectedRectangle.Left > rect.Left)
            {
                return false;
            }
            if (SelectedRectangle.Right < rect.Right)
            {
                return false;
            }
            if (SelectedRectangle.Top > rect.Top)
            {
                return false;
            }
            if (SelectedRectangle.Bottom < rect.Bottom)
            {
                return false;
            }
            return true;
        }

        public override bool IsSelected(Color point_color)
        {
            if (point_color.ToArgb() == color_signal.ToArgb())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void UpdateBitmapColors()
        {
            color_signal = GenerateRandomColor();
        }
    }
}
