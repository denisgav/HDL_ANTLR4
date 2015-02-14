using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace Schematix.FSM
{
    [Serializable]
    public class My_Line : My_Figure
    {
        public My_Label label_condition;
        int selected_mark;
        public string condition { get; set; }
        public string Action { get; set; }
        public My_State state_begin;
        public My_State state_end;
        public Color color { get; set; }
        public int priority { get; set; }
        Color[] color_mark;
        Color color_line;
        public enum DrawningStyle { DrawningBezier, DrawningCurve };
        public DrawningStyle DrawStyle = DrawningStyle.DrawningBezier;

        public Point creation_pt1;
        public Point creation_pt2;
        public bool created_FirstPoint = false;

        private Point angle1;
        private Point angle2;
        public Point Angle1
        {
            get
            {
                Point pt = new Point(angle1.X + state_begin.CenterPoint.X, angle1.Y + state_begin.CenterPoint.Y);
                return pt;
            }
            set
            {
                angle1.X = value.X - state_begin.CenterPoint.X;
                angle1.Y = value.Y - state_begin.CenterPoint.Y;
            }
        }
        public Point Angle2
        {
            get
            {
                Point pt = new Point(angle2.X + state_end.CenterPoint.X, angle2.Y + state_end.CenterPoint.Y);
                return pt;
            }
            set
            {
                angle2.X = value.X - state_end.CenterPoint.X;
                angle2.Y = value.Y - state_end.CenterPoint.Y;
            }
        }

        public override Size MaxSize
        {
            get 
            {
                Size res = new Size();
                if ((state_begin != null) && (state_end != null))
                {
                    if (Angle1.X > res.Width)
                        res.Width = Angle1.X;

                    if (Angle2.X > res.Width)
                        res.Width = Angle2.X;

                    if (Angle1.Y > res.Height)
                        res.Height = Angle1.Y;

                    if (Angle2.Y > res.Height)
                        res.Height = Angle2.Y;
                }
                return res;
            }
        }

        public override Point CenterPoint
        {
            get
            {
                Point pt = new Point(0, 0);
                pt.X = (Angle1.X + Angle2.X) / 2;
                pt.Y = (Angle1.Y + Angle2.Y) / 2;
                return pt;
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
                label_condition.Core = core;
            }
        }

        public void RotateStartPoint(double angle)
        {
            if (state_begin != state_end)
            {
                double R = Math.Sqrt((state_begin.CenterPoint.X - state_end.CenterPoint.X) * (state_begin.CenterPoint.X - state_end.CenterPoint.X) + (state_begin.CenterPoint.Y - state_end.CenterPoint.Y) * (state_begin.CenterPoint.Y - state_end.CenterPoint.Y)) / 2.0;
                double k = (double)(state_begin.CenterPoint.Y - state_end.CenterPoint.Y) / (double)(state_begin.CenterPoint.X - state_end.CenterPoint.X);
                double beta = Math.Atan(k);

                if (state_begin.CenterPoint.X < state_end.CenterPoint.X)
                    beta += Math.PI;
                double gama = angle + beta;
                angle1.X = -(int)(R * Math.Cos(gama));
                angle1.Y = -(int)(R * Math.Sin(gama));
            }
            else
            {
                double R = state_begin.rect.Width;
                angle1.X = -(int)(R * Math.Cos(angle));
                angle1.Y = -(int)(R * Math.Sin(angle));
            }
            
        }

        public void RotateEndPoint(double angle)
        {
            if (state_begin != state_end)
            {
                double R = Math.Sqrt((state_begin.CenterPoint.X - state_end.CenterPoint.X) * (state_begin.CenterPoint.X - state_end.CenterPoint.X) + (state_begin.CenterPoint.Y - state_end.CenterPoint.Y) * (state_begin.CenterPoint.Y - state_end.CenterPoint.Y)) / 2.0;
                double k = (double)(state_begin.CenterPoint.Y - state_end.CenterPoint.Y) / (double)(state_begin.CenterPoint.X - state_end.CenterPoint.X);
                double beta = Math.Atan(k);

                if (state_begin.CenterPoint.X < state_end.CenterPoint.X)
                    beta += Math.PI;
                double gama = angle + beta;
                angle2.X = (int)(R * Math.Cos(gama));
                angle2.Y = (int)(R * Math.Sin(gama));
            }
            else
            {
                double R = state_begin.rect.Width;
                angle1.X = (int)(R * Math.Cos(angle));
                angle1.Y = (int)(R * Math.Sin(angle));
            }
        }

        public override void Draw(object sender, PaintEventArgs e)
        {
            Point start_point;
            Point end_point;
            Point priority_point;

            start_point = core.Bitmap.GetCommonPoint(state_begin, this, true);
            end_point = core.Bitmap.GetCommonPoint(state_end, this, false);

            #region getting_priority_point
            priority_point = start_point;
            #endregion

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            Graphics dc = e.Graphics;
            Pen pen;
            if (selected == false)
                pen = new Pen(color, 1);
            else
                pen = new Pen(SelectedColor, 1);

            AdjustableArrowCap My_Cap = new AdjustableArrowCap(5, 10);
            My_Cap.BaseCap = LineCap.Triangle;
            pen.CustomEndCap = My_Cap;

            if (DrawStyle == DrawningStyle.DrawningBezier)
            {
                dc.DrawBezier(pen, start_point, Angle1, Angle2, end_point);
            }
            else
            {
                Point[] points = { start_point, Angle1, Angle2, end_point };
                dc.DrawCurve(pen, points);
            }

            #region draw_priority_point
            if (core.Paper.ShowLinePriority == true)
            {
                Rectangle rt = new Rectangle(priority_point.X - 10, priority_point.Y - 10, 20, 20);
                Brush brush = new SolidBrush(Color.DarkOrange);
                dc.FillEllipse(brush, rt);
                dc.DrawEllipse(pen, rt);
                System.Drawing.StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;
                dc.DrawString(priority.ToString(), new Font("Times New Roman", 10, FontStyle.Bold), new SolidBrush(Color.Black), rt, format);
            }
            #endregion
            if (string.IsNullOrEmpty(label_condition.Text) == false)
                label_condition.draw(sender, e);

            if (selected == true)
            {
                Pen pen2 = new Pen(color);
                pen2.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                if (string.IsNullOrEmpty(label_condition.Text) == false)
                {
                    dc.DrawLine(pen2, label_condition.CenterPoint, state_begin.CenterPoint);
                    dc.DrawLine(pen2, label_condition.CenterPoint, state_end.CenterPoint);
                }
                dc.FillRectangle(new SolidBrush(Color.Black), Angle1.X - 2, Angle1.Y - 2, 4, 4);
                dc.FillRectangle(new SolidBrush(Color.Black), Angle2.X - 2, Angle2.Y - 2, 4, 4);
                dc.DrawLine(pen2, start_point, Angle1);
                dc.DrawLine(pen2, Angle2, end_point);
            }
        }

        public override void Draw_Bitmap(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            Graphics dc = e.Graphics;
            Pen pen = new Pen(color_line, 4);

            Point start_point;
            Point end_point;
            start_point = core.Bitmap.GetCommonPoint(state_begin, this, true);
            end_point = core.Bitmap.GetCommonPoint(state_end, this, false);

            if (DrawStyle == DrawningStyle.DrawningBezier)
            {
                dc.DrawBezier(pen, start_point, Angle1, Angle2, end_point);
            }
            else
            {
                Point[] points = { start_point, Angle1, Angle2, end_point };
                dc.DrawCurve(pen, points);
            }

            dc.FillRectangle(new SolidBrush(color_mark[0]), Angle1.X - 6, Angle1.Y - 6, 12, 12);
            dc.FillRectangle(new SolidBrush(color_mark[1]), Angle2.X - 6, Angle2.Y - 6, 12, 12);

            if (string.IsNullOrEmpty(label_condition.Text) == false)
                label_condition.Draw_Bitmap(sender, e);
        }

        public void DrawCreateNew(object sender, PaintEventArgs e)
        {
            if (created_FirstPoint == false)
                return;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            Graphics dc = e.Graphics;
            dc.DrawLine(new Pen(Color.Black), creation_pt1, creation_pt2);
        }

        public override void MouseMoveCreateNew(object sender, MouseEventArgs e)
        {
            Point pt = new Point(e.X, e.Y);
            if (this.created_FirstPoint == false)
                return;
            creation_pt2 = pt;
            core.form.Invalidate();
        }

        public override void MouseDownCreateNew(object sender, MouseEventArgs e)
        {
            Point pt = new Point(e.X, e.Y);
            created_FirstPoint = true;
            creation_pt1 = pt;
            creation_pt2 = pt;
        }

        public override void MouseUpCreateNew(object sender, MouseEventArgs e)
        {
            Point pt = new Point(e.X, e.Y);
            creation_pt2 = pt;
            CreateLine();
            core.Bitmap.UpdateBitmap();
            core.form.Invalidate();
        }

        private new void MouseMove(object sender, MouseEventArgs e)
        {

        }

        public override void Select(Color point_color)
        {
            if (point_color.ToArgb() == color_mark[0].ToArgb())
            {
                selected_mark = 1;
                return;
            }

            if (point_color.ToArgb() == color_mark[1].ToArgb())
            {
                selected_mark = 2;
                return;
            }

            if (point_color.ToArgb() == color_line.ToArgb())
            {
                selected_mark = 0;
                return;
            }
        }

        public override bool Select(Rectangle SelectedRectangle)
        {
            if ((state_begin.CenterPoint.X < SelectedRectangle.Left) || (state_begin.CenterPoint.X > SelectedRectangle.Right))
            {
                return false;
            }
            if ((state_begin.CenterPoint.Y < SelectedRectangle.Top) || (state_begin.CenterPoint.Y > SelectedRectangle.Bottom))
            {
                return false;
            }
            if ((state_end.CenterPoint.X < SelectedRectangle.Left) || (state_end.CenterPoint.X > SelectedRectangle.Right))
            {
                return false;
            }
            if ((state_end.CenterPoint.Y < SelectedRectangle.Top) || (state_end.CenterPoint.Y > SelectedRectangle.Bottom))
            {
                return false;
            }
            return true;
        }

        public override bool IsSelected(Color point_color)
        {
            if (point_color.ToArgb() == color_mark[0].ToArgb())
            {
                return true;
            }

            if (point_color.ToArgb() == color_mark[1].ToArgb())
            {
                return true;
            }

            if (point_color.ToArgb() == color_line.ToArgb())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private new void MouseMoveResize(object sender, MouseEventArgs e)
        {
            Point pt = new Point(e.X, e.Y);
            switch (selected_mark)
            {
                case 1:
                    Angle1 = pt;
                    if (Angle1.X < 0)
                        Angle1 = new Point(0, Angle1.Y);
                    if (Angle1.Y < 0)
                        angle1 = new Point(Angle1.X, 0);
                    break;

                case 2:
                    Angle2 = pt;
                    if (Angle2.X < 0)
                        Angle2 = new Point(0, Angle2.Y);
                    if (Angle2.Y < 0)
                        Angle2 = new Point(Angle2.X, 0);
                    break;
                default:
                    break;
            }
            core.form.Invalidate();
        }

        private new void MouseDown(object sender, MouseEventArgs e)
        {
            Point pt = new Point(e.X, e.Y);
            if (selected == false)
                return;
            if (e.Button == MouseButtons.Left)
            {
                if (selected_mark == 0)
                    return;
                mouse_move = MouseMoveResize;
            }
            if (e.Button == MouseButtons.Right)
            {
                Point p = new Point(Control.MousePosition.X, Control.MousePosition.Y);
                //form.SelectedFigure = this;
                core.form.contextMenuStrip.Show(p);
            }
        }

        private new void MouseUp(object sender, MouseEventArgs e)
        {
            if (selected == false)
                return;
            if (e.Button == MouseButtons.Left)
            {
                core.Bitmap.UpdateBitmap();
                mouse_move = MouseMove;
                //core.AddToHistory("Line " + name + "Moved");
            }
        }

        public My_Line(My_Line line)
        {
            this.color = line.color;
            this.condition = line.condition;
            this.core = line.core;
            this.name = line.name;
            this.priority = line.priority;

            this.label_condition = new My_Label(line.label_condition);
            this.label_condition.Owner = this;

            this.color_mark = new Color[2];
            UpdateBitmapColors();
            mouse_move = MouseMove;
            mouse_down = MouseDown;
            mouse_up = MouseUp;
            draw = Draw;
        }

        public My_Line(My_State s1, My_State s2, string name, string condition, Schematix.FSM.Constructor_Core core)
        {
            this.core = core;
            this.state_begin = s1;
            this.state_end = s2;
            this.name = name;
            this.condition = condition;
            this.color = Color.Black;
            this.priority = 0;
            selected = false;
            selected_mark = 0;

            creation_pt1 = new Point();
            creation_pt2 = new Point();

            this.label_condition = new My_Label(condition, color, core, this);
            this.label_condition.Owner = this;

            color_mark = new Color[2];
            UpdateBitmapColors();
            /*
            if ((s1.CenterPoint.X == s2.CenterPoint.X) && (s1.CenterPoint.Y == s2.CenterPoint.Y))
            {
                angle1 = s1.rect.Location;
                angle2 = new Point(s1.rect.Location.X, s1.rect.Location.Y + s1.rect.Height);
            }
            else
            {
                int deltax = (state_end.CenterPoint.X - state_begin.CenterPoint.X) / 2;
                int deltay = (state_end.CenterPoint.Y - state_begin.CenterPoint.Y) / 2;

                angle1 = new Point((state_begin.CenterPoint.X + deltax), (state_begin.CenterPoint.Y + deltay));
                angle2 = new Point((state_end.CenterPoint.X - deltax), (state_end.CenterPoint.Y - deltay));
            }
            */
            CreateLine(s1, s2);

            mouse_move = MouseMove;
            mouse_down = MouseDown;
            mouse_up = MouseUp;
            draw = Draw;
        }

        public My_Line(My_State s1, My_State s2, string name, Schematix.FSM.Constructor_Core core)
            : this(s1, s2, name, " ", core)
        {
        }
        public My_Line(My_State s1, My_State s2, Schematix.FSM.Constructor_Core core)
            : this(s1, s2, ("L" + core.Graph.Lines.Count.ToString()), " ", core)
        {
        }
        public My_Line(bool creation, Schematix.FSM.Constructor_Core core)
        {
            this.core = core;
            if (creation == true)
            {
                draw = DrawCreateNew;
                mouse_move = MouseMoveCreateNew;
                mouse_down = MouseDownCreateNew;
                mouse_up = MouseUpCreateNew;
            }
            else
            {
                draw = Draw;
                mouse_move = MouseMove;
                mouse_down = MouseDown;
                mouse_up = MouseUp;
            }
            color_mark = new Color[2];
            UpdateBitmapColors();
            this.name = "L" + core.Graph.Lines.Count.ToString();
            condition = "";
            this.priority = 0;
            label_condition = new My_Label(condition, color, core, this);
            selected = false;
            selected_mark = 0;
            creation_pt1 = new Point();
            creation_pt2 = new Point();
            color = Color.Black;
        }
        
        private void CreateLine(Schematix.FSM.My_State st1, Schematix.FSM.My_State st2)
        {
        	this.state_begin = st1;
            this.state_end = st2;

            if ((st1.CenterPoint.X == st2.CenterPoint.X) && (st1.CenterPoint.Y == st2.CenterPoint.Y))
            {
                Angle1 = new Point((st1.CenterPoint.X - 2 * st1.rect.Width), (st1.CenterPoint.Y - 2 * st1.rect.Height));
                Angle2 = new Point((st1.CenterPoint.X - 2 * st1.rect.Width), (st1.CenterPoint.Y + 2 * st1.rect.Height));
            }
            else
            {
                int deltax = (state_end.CenterPoint.X - state_begin.CenterPoint.X) / 3;
                int deltay = (state_end.CenterPoint.Y - state_begin.CenterPoint.Y) / 3;

                Angle1 = new Point((state_begin.CenterPoint.X + deltax), (state_begin.CenterPoint.Y + deltay));
                Angle2 = new Point((state_end.CenterPoint.X - deltax), (state_end.CenterPoint.Y - deltay));
            }

            mouse_move = MouseMove;
            mouse_down = MouseDown;
            mouse_up = MouseUp;
            draw = Draw;

            core.Graph.UpdateLinesAggle(st1, st2);
            //core.AddToHistory("Line " + name + " created");
        }

        private void CreateLine()
        {
            Schematix.FSM.My_State st1 = new My_State(core);
            Schematix.FSM.My_State st2 = new My_State(core);

            float? L = null;
            foreach (Schematix.FSM.My_State st in core.Graph.States)
            {
                float S = (creation_pt1.X - st.CenterPoint.X) * (creation_pt1.X - st.CenterPoint.X) + (creation_pt1.Y - st.CenterPoint.Y) * (creation_pt1.Y - st.CenterPoint.Y);
                if ((L == null) || (L > S))
                {
                    L = S;
                    st1 = st;
                }
            }

            if (L > 10000)
            {
                core.Graph.RemoveFigure(this);
            }

            L = null;
            foreach (Schematix.FSM.My_State st in core.Graph.States)
            {
                float S = (creation_pt2.X - st.CenterPoint.X) * (creation_pt2.X - st.CenterPoint.X) + (creation_pt2.Y - st.CenterPoint.Y) * (creation_pt2.Y - st.CenterPoint.Y);
                if ((L == null) || (L > S))
                {
                    L = S;
                    st2 = st;
                }
            }
            if (L > 10000)
            {
                core.Graph.RemoveFigure(this);
            }

            CreateLine(st1, st2);
        }

        public override void UpdateBitmapColors()
        {
            color_mark[0] = GenerateRandomColor();
            color_mark[1] = GenerateRandomColor();
            color_line = GenerateRandomColor();
        }
    }
}