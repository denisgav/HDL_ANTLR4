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
    public class My_Line : My_Figure
    {
        private Point start_point; // используется при перемещении
        private Point[] array; //используется для перемещения

        private Point point1;
        private Point point2;

        //Перемещение линии
        public enum LineMovementDirection
        {
            Point1,
            Point2,
            FreeMovement
        };

        private LineMovementDirection Direction = LineMovementDirection.FreeMovement;


        public override Point[] Points
        {
            get
            {
                return new Point[2] { point1, point2 };
            }
        }

        public My_Line(EntityDrawningCore core) : base(core)
        {
            draw = Draw;
            mouse_move = MouseMoveCreateNew;
            mouse_down = MouseDownCreateNew;
            mouse_up = MouseUpCreateNew;
        }

        public My_Line(EntityDrawningCore core, Point point1, Point point2)
            : base(core)
        {
            this.point1 = point1;
            this.point2 = point2;

            draw = Draw;
            mouse_move = MouseMove;
            mouse_down = MouseDown;
            mouse_up = MouseUp;
        }

        public My_Line(My_Line item)
            : base(item)
        {
            point1 = item.point1;
            point2 = item.point2;

            mouse_move = MouseMove;
            mouse_down = MouseDown;
            mouse_up = MouseUp;
            draw = Draw;
        }


        public override Point CenterPoint
        {
            get
            {
                return point1;
            }
            set
            {
                base.CenterPoint = value;
            }
        }

        public override void AddToGraphicsPath(System.Drawing.Drawing2D.GraphicsPath path)
        {
            path.AddLine(point1, point2);
        }

        private new void Draw(object sender, PaintEventArgs e)
        {
            Graphics dc = e.Graphics;
            if (selected == false)
            {
                dc.DrawLine(pen, point1, point2);
            }
            else
            {
                Pen pen2 = new Pen(Color.FromArgb(128, EntityDrawningCore.SelectedColor));

                dc.DrawLine(pen2, point1, point2);

                Brush br = new SolidBrush(Color.Black);
                foreach (Point p in Points)
                {
                    dc.FillRectangle(br, new Rectangle(p.X - 2, p.Y - 2, 4, 4));
                }
            }
        }

        private new void MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point pt = new Point(e.X, e.Y);
                if (Direction == LineMovementDirection.FreeMovement)
                {
                    int delta_x = start_point.X - e.X;
                    int delta_y = start_point.Y - e.Y;

                    point1.X = array[0].X - delta_x;
                    point1.Y = array[0].Y - delta_y;
                    point2.X = array[1].X - delta_x;
                    point2.Y = array[1].Y - delta_y;
                }
                if (Direction == LineMovementDirection.Point1)
                {
                    point1.X = e.X;
                    point1.Y = e.Y;

                    if ((core.KeyDown_ != null) && (core.KeyDown_.Shift == true))
                    {
                        float distance = (float)Math.Sqrt((point1.X - point2.X) * (point1.X - point2.X) + (point1.Y - point2.Y) * (point1.Y - point2.Y));
                        float angle = (float)Math.Acos((float)(point1.X - point2.X) / distance);
                        angle = Math.Abs(angle);
                        angle *= 180.0f / (float)Math.PI;

                        if(point1.Y < point2.Y)
                            angle = 360f - angle;
                        
                        angle = RoundAngle(angle);

                        double a = (angle/180.0f)*Math.PI;
                        point1.X = point2.X + (int)((float)distance * Math.Cos(a));
                        point1.Y = point2.Y + (int)((float)distance * Math.Sin(a));
                    }

                    if ((core.KeyDown_ != null) && (core.KeyDown_.Control == true))
                    {
                        point1 = core.Picture.GetNearestPoint(this, point1);
                    }
                }
                if (Direction == LineMovementDirection.Point2)
                {
                    point2.X = e.X;
                    point2.Y = e.Y;

                    if ((core.KeyDown_ != null) && (core.KeyDown_.Shift == true))
                    {
                        float distance = (float)Math.Sqrt((point1.X - point2.X) * (point1.X - point2.X) + (point1.Y - point2.Y) * (point1.Y - point2.Y));
                        float angle = (float)Math.Acos((float)(point2.X - point1.X) / distance);
                        angle = Math.Abs(angle);
                        angle *= 180.0f / (float)Math.PI;
                        if (point2.Y < point1.Y)
                            angle = 360f - angle;

                        angle = RoundAngle(angle);

                        double a = (angle / 180.0f) * Math.PI;
                        point2.X = point1.X + (int)((float)distance * Math.Cos(a));
                        point2.Y = point1.Y + (int)((float)distance * Math.Sin(a));
                    }

                    if ((core.KeyDown_ != null) && (core.KeyDown_.Control == true))
                    {
                        point2 = core.Picture.GetNearestPoint(this, point2);
                    }
                }
                core.Form.Invalidate();
                return;
            }
        }

        private new void MouseMoveCreateNew(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                point2.X = e.X;
                point2.Y = e.Y;

                if ((core.KeyDown_ != null) && (core.KeyDown_.Shift == true))
                {
                    float distance = (float)Math.Sqrt((point1.X - point2.X) * (point1.X - point2.X) + (point1.Y - point2.Y) * (point1.Y - point2.Y));
                    float angle = (float)Math.Acos((float)(point2.X - point1.X) / distance);
                    angle = Math.Abs(angle);
                    angle *= 180.0f / (float)Math.PI;
                    if (point2.Y < point1.Y)
                        angle = 360f - angle;

                    angle = RoundAngle(angle);

                    double a = (angle / 180.0f) * Math.PI;
                    point2.X = point1.X + (int)((float)distance * Math.Cos(a));
                    point2.Y = point1.Y + (int)((float)distance * Math.Sin(a));
                }

                if ((core.KeyDown_ != null) && (core.KeyDown_.Control == true))
                {
                    point2 = core.Picture.GetNearestPoint(this, point2);
                }

                core.Form.Invalidate();
            }
        }

        private new void MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                start_point.X = e.X;
                start_point.Y = e.Y;
                array = Points;

                //нужно определить направление движения прямоугольника
                Point pt = new Point(e.X, e.Y);

                Point[] points = Points;
                if ((Math.Abs(pt.X - point1.X) <= 2) && (Math.Abs(pt.Y - point1.Y) <= 2)) // влево и вверх
                {
                    Direction = LineMovementDirection.Point1;
                    return;
                }
                if ((Math.Abs(pt.X - point2.X) <= 2) && (Math.Abs(pt.Y - point2.Y) <= 2)) // вверх
                {
                    Direction = LineMovementDirection.Point2;
                    return;
                }
                Direction = LineMovementDirection.FreeMovement;
            }
            if (e.Button == MouseButtons.Right)
                core.Form.contextMenuStripLine.Show(Control.MousePosition);
        }

        private new void MouseDownCreateNew(object sender, MouseEventArgs e)
        {
            point1.X = e.X;
            point1.Y = e.Y;

            if ((core.KeyDown_ != null) && (core.KeyDown_.Control == true))
            {
                point1 = core.Picture.GetNearestPoint(this, point1);
            }
        }

        private new void MouseUp(object sender, MouseEventArgs e)
        {
            brush.UpdateBrush(this);
            //core.AddToHistory();
        }

        private new void MouseUpCreateNew(object sende, MouseEventArgs e)
        {
            mouse_move = MouseMove;
            mouse_down = MouseDown;
            mouse_up = MouseUp;

            //core.AddToHistory();
        }

        public override bool IsSelected(Point pt)
        {
            if (base.IsSelected(pt) == true)
                return true;
            if (!(((pt.X >= point1.X-2) && (pt.X <= point2.X+2)) || ((pt.X >= point2.X-2) && (pt.X <= point1.X+2))))
                return false;
            if (!(((pt.Y >= point1.Y-2) && (pt.Y <= point2.Y+2)) || ((pt.Y >= point2.Y-2) && (pt.Y <= point1.Y+2))))
                return false;

            if (Math.Abs(point1.X - point2.X) < 2)
                return true;

            double k = ((double)point1.Y - (double)point2.Y) / ((double)point1.X - (double)point2.X);
            double b = (double)point2.Y - k * (double)point2.X;

            double Y2 = k * pt.X + b;
            if (Math.Abs(Y2 - (double)pt.Y) < 5)
                return true;
            else
                return false;
        }

        public override bool IsSelected(Rectangle rt)
        {
            bool res1 = false;
            bool res2 = false;

            res1 = rt.Contains(point1);
            res2 = rt.Contains(point2);
            if ((res1 == true) && (res2 == true))
                return true;
            else
                return false;
        }
    }
}