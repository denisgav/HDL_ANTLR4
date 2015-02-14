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
    public class My_Reset : My_Figure
    {
        public enum Reset_Type
        {
            Asynchonous, Synchonous
        };

        public void Fix()
        {
            mouse_down = MouseDown;
            mouse_move = MouseMove;
            mouse_up = MouseUp;
            draw = Draw;
        }

        public Reset_Type res_type {get; set;}
        public My_State state;
        public string condition;
        public string signal;

        private Color figure_color;

        private Point center_point;
        public new Point CenterPoint
        {
            get
            {
                if (state != null)
                {
                    Point pt = new Point(center_point.X + state.CenterPoint.X, center_point.Y + state.CenterPoint.Y);
                    return pt;
                }
                else
                {
                    Point pt = new Point(center_point.X, center_point.Y);
                    return pt;
                }
            }
            set
            {
                if (state != null)
                {
                    center_point.X = value.X - state.CenterPoint.X;
                    center_point.Y = value.Y - state.CenterPoint.Y;
                }
                else
                    center_point = value;
            }
        }

        public Rectangle rect
        {
            get
            {                
                Rectangle rt = new Rectangle(CenterPoint.X - 20, CenterPoint.Y - 20, 40, 40);
                return rt;
            }
        }

        public override Size MaxSize
        {
            get
            {
                return new Size(rect.Width, rect.Height);
            }
        }

        private bool cretion_flag = false; // используется при создании обьекта
        private Point Creation_Point;      // используется при создании обьекта

        public My_Reset(Schematix.FSM.Constructor_Core core)
        {
            this.core = core;
            this.res_type = Reset_Type.Synchonous;

            mouse_down = MouseDownCreateNew;
            mouse_move = MouseMoveCreateNew;
            mouse_up = MouseUpCreateNew;
            draw = Draw;

            UpdateBitmapColors();
        }

        public My_Reset(My_Reset item)
        {
            this.core = item.core;
            this.center_point = item.center_point;
            this.condition = item.condition;
            this.signal = item.signal;
            this.res_type = item.res_type;

            mouse_down = MouseDown;
            mouse_move = MouseMove;
            mouse_up = MouseUp;
            draw = Draw;

            UpdateBitmapColors();
        }

        public override void MouseDownCreateNew(object sender, MouseEventArgs e)
        {
            center_point.X = e.X;
            center_point.Y = e.Y;
            cretion_flag = true;
        }
        public override void MouseMoveCreateNew(object sender, MouseEventArgs e)
        {
            if (cretion_flag == true)
            {
                Creation_Point.X = e.X;
                Creation_Point.Y = e.Y;
            }
            else
            {
                center_point.X = e.X;
                center_point.Y = e.Y;
            }
            core.form.Invalidate();
        }
        public override void MouseUpCreateNew(object sender, MouseEventArgs e)
        {
            cretion_flag = false;
            mouse_down = MouseDown;
            mouse_move = MouseMove;
            mouse_up = MouseUp;

            Creation_Point.X = e.X;
            Creation_Point.Y = e.Y;

            //ищем ближайшее состояние
            int? min_distance = null;
            foreach (My_State s in core.Graph.States)
            {
                int distance = (e.X - s.CenterPoint.X) * (e.X - s.CenterPoint.X) + (e.Y - s.CenterPoint.Y) * (e.Y - s.CenterPoint.Y);
                if ((min_distance == null) || (distance < min_distance))
                {
                    min_distance = distance;
                    state = s;
                }
            }
            center_point.X -= state.CenterPoint.X;
            center_point.Y -= state.CenterPoint.Y;

            core.Bitmap.UpdateBitmap();
            core.form.Invalidate();
        }

        public override void Draw(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            Graphics dc = e.Graphics;
            Brush brush;
            Pen pen = new Pen(Color.Black);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            if (selected == false)
            {
                brush = new SolidBrush(Color.Yellow);
            }
            else
            {
                brush = new SolidBrush(core.Selected_Color);
            }

            /*Point[] triangle = new Point[3];
            triangle[0].X = rect.Left;
            triangle[0].Y = rect.Bottom;

            triangle[1].X = rect.Left + rect.Width/2;
            triangle[1].Y = rect.Top;

            triangle[2].X = rect.Right;
            triangle[2].Y = rect.Bottom;

            dc.FillPolygon(brush, triangle);
            dc.DrawLines(pen, triangle);*/

            if (state == null)
            {
                if (cretion_flag != false)
                    dc.DrawLine(pen, CenterPoint, Creation_Point);
            }
            else
            {
                dc.DrawLine(pen, CenterPoint, state.CenterPoint);
            }

            if (selected == false)
            {
                if (res_type == Reset_Type.Asynchonous)
                {
                    Bitmap icon = global::FSM.Resources.reset1;
                    dc.DrawImage(icon, rect);
                }
                else
                {
                    Bitmap icon = global::FSM.Resources.reset2;
                    dc.DrawImage(icon, rect);
                }
            }
            else
            {
                if (res_type == Reset_Type.Asynchonous)
                {
                    Bitmap icon = global::FSM.Resources.reset1_;
                    dc.DrawImage(icon, rect);
                }
                else
                {
                    Bitmap icon = global::FSM.Resources.reset2_;
                    dc.DrawImage(icon, rect);
                }
            }
        }

        public override void Draw_Bitmap(object sender, PaintEventArgs e)
        {
            Brush brush = new SolidBrush(figure_color);
            Point[] triangle = new Point[3];
            triangle[0].X = rect.Left;
            triangle[0].Y = rect.Bottom;

            triangle[1].X = rect.Left + rect.Width / 2;
            triangle[1].Y = rect.Top;

            triangle[2].X = rect.Right;
            triangle[2].Y = rect.Bottom;

            e.Graphics.FillPolygon(brush, triangle);
        }

        public override void UpdateBitmapColors()
        {
            figure_color = GenerateRandomColor();
        }

        private new void MouseMove(object sender, MouseEventArgs e)
        {
        }

        private new void MouseMoveResize(object sender, MouseEventArgs e)
        {
            Point pt = new Point(e.X, e.Y);
            CenterPoint = pt;

            if (CenterPoint.X < 0)
                CenterPoint = new Point(0, CenterPoint.Y);
            if (CenterPoint.Y < 0)
                CenterPoint = new Point(CenterPoint.X, 0);

            core.form.Invalidate();
        }

        private new void MouseDown(object sender, MouseEventArgs e)
        {
            if (selected == false)
                return;
            Point pt = new Point(e.X, e.Y);
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

        private new void MouseUp(object sender, MouseEventArgs e)
        {
            if (selected == false)
                return;

            if (e.Button == MouseButtons.Left)
            {
                mouse_move = MouseMove;
                core.Bitmap.UpdateBitmap();
                //core.AddToHistory("Reset moved");
            }
        }

        public override void Select(Color point_color)
        {
            if (point_color.ToArgb() == figure_color.ToArgb())
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
            if (point_color.ToArgb() == figure_color.ToArgb())
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