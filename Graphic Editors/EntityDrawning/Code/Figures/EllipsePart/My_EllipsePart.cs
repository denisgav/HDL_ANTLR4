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
    public class My_EllipsePart : My_Figure
    {
        public Rectangle rect;
        public float startAngle = 45.0F;
        public float sweepAngle = 270.0F;

        private Point start_point; // используется для перемещения
        private Point start_location;// используется для перемещения

        //Перемещение прямоугольника
        public enum MovementDirection
        {
            Left,
            Top,
            Right,
            Bottom,
            LeftAndTop,
            RightAndTop,
            LeftAndBottom,
            RightAndBottom,
            StartAngle,
            SweepAngle,
            FreeMovement
        };

        private MovementDirection Direction = MovementDirection.FreeMovement;

        public override Point[] Points
        {
            get
            {
                Point[] points = new Point[10];
                points[0].X = rect.Left;
                points[0].Y = rect.Top;

                points[1].X = rect.Left + rect.Width / 2;
                points[1].Y = rect.Top;

                points[2].X = rect.Right;
                points[2].Y = rect.Top;

                points[7].X = rect.Left;
                points[7].Y = rect.Top + rect.Height / 2;

                points[3].X = rect.Right;
                points[3].Y = rect.Top + rect.Height / 2;

                points[6].X = rect.Left;
                points[6].Y = rect.Bottom;

                points[5].X = rect.Left + rect.Width / 2;
                points[5].Y = rect.Bottom;

                points[4].X = rect.Right;
                points[4].Y = rect.Bottom;

                if ((rect.Width < 20) || (rect.Height < 20))
                {
                    points[8].X = 0;
                    points[8].Y = 0;

                    points[9].X = 0;
                    points[9].Y = 0;
                }
                else
                {
                    /*точки для зменения углов*/
                    //данные для еллипса
                    Point center_point = new Point(rect.Left + rect.Width / 2, rect.Top + rect.Height / 2);
                    double a = (double)(rect.Width / 2);
                    double b = (double)(rect.Height / 2);
                    //x^2/a^2 + y^2/b^2 = 1;

                    double k1 = Math.Tan(startAngle * Math.PI / 180f);
                    double x1 = (b * b) / (k1 * k1 + ((b * b) / (a * a)));
                    x1 = Math.Sqrt(x1);
                    double y1 = k1 * x1;

                    if ((startAngle > 90) && (startAngle < 270))
                    {
                        x1 = -x1;
                        y1 = -y1;
                    }

                    y1 += center_point.Y;
                    x1 += center_point.X;

                    points[8].X = (int)x1;
                    points[8].Y = (int)y1;

                    double k2 = Math.Tan((startAngle + sweepAngle) * Math.PI / 180f);
                    double x2 = (b * b) / (k2 * k2 + ((b * b) / (a * a)));
                    x2 = Math.Sqrt(x2);
                    double y2 = k2 * x2;

                    if (((startAngle + sweepAngle) % 360 > 90) && ((startAngle + sweepAngle) % 360 < 270))
                    {
                        x2 = -x2;
                        y2 = -y2;
                    }

                    y2 += center_point.Y;
                    x2 += center_point.X;

                    points[9].X = (int)x2;
                    points[9].Y = (int)y2;
                }

                return points;
            }
        }

        public override Point CenterPoint
        {
            get
            {
                Point pt = new Point();
                pt.X = rect.Location.X + rect.Width / 2;
                pt.Y = rect.Location.Y + rect.Height / 2;
                return pt;
            }
            set
            {
                base.CenterPoint = value;
            }
        }

        public My_EllipsePart(EntityDrawningCore core)
            : base(core)
        {
            draw = Draw;
            rect = new Rectangle();
            mouse_move = MouseMoveCreateNew;
            mouse_down = MouseDownCreateNew;
            mouse_up = MouseUpCreateNew;
        }

        public My_EllipsePart(EntityDrawningCore core, Rectangle rect)
            : base(core)
        {
            draw = Draw;
            this.rect = rect;
            mouse_move = MouseMove;
            mouse_down = MouseDown;
            mouse_up = MouseUp;
        }

        public My_EllipsePart(My_EllipsePart item)
            : base(item)
        {
            this.rect = item.rect;
            this.startAngle = item.startAngle;
            this.sweepAngle = item.sweepAngle;
            mouse_move = MouseMove;
            mouse_down = MouseDown;
            mouse_up = MouseUp;
            draw = Draw;
        }

        public override void AddToGraphicsPath(System.Drawing.Drawing2D.GraphicsPath path)
        {
        }

        protected override void Draw(object sender, PaintEventArgs e)
        {
        }

        protected override void MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point pt = new Point(e.X, e.Y);
                if (Direction == MovementDirection.FreeMovement)
                {
                    Rectangle rect2 = new Rectangle(rect.Location, rect.Size);
                    rect2.Location = new Point(start_location.X - (this.start_point.X - pt.X), start_location.Y - (this.start_point.Y - pt.Y));
                    rect = rect2;
                }

                if (Direction == MovementDirection.StartAngle)
                {
                    Point center_point = new Point(rect.Left + rect.Width / 2, rect.Top + rect.Height / 2);
                    double distance = Math.Sqrt((pt.X - center_point.X) * (pt.X - center_point.X) + (pt.Y - center_point.Y) * (pt.Y - center_point.Y));
                    double angle = Math.Acos((pt.X - center_point.X) / distance);
                    angle *= 180f / Math.PI;
                    if (center_point.Y > pt.Y)
                        angle = 360f - angle;
                    startAngle = (float)angle;
                    if ((core.KeyDown_ != null) && (core.KeyDown_.Shift == true))
                    {
                        startAngle = RoundAngle(startAngle);
                    }
                }

                if (Direction == MovementDirection.SweepAngle)
                {
                    Point center_point = new Point(rect.Left + rect.Width / 2, rect.Top + rect.Height / 2);
                    double distance = Math.Sqrt((pt.X - center_point.X) * (pt.X - center_point.X) + (pt.Y - center_point.Y) * (pt.Y - center_point.Y));
                    double angle = Math.Acos((pt.X - center_point.X) / distance);
                    angle *= 180f / Math.PI;
                    if (center_point.Y > pt.Y)
                        angle = 360f - angle;
                    sweepAngle = Math.Abs(360 - startAngle + (float)angle) % 360f;
                    if ((core.KeyDown_ != null) && (core.KeyDown_.Shift == true))
                    {
                        sweepAngle = RoundAngle(sweepAngle);
                    }
                }

                if ((Direction == MovementDirection.Left) || (Direction == MovementDirection.LeftAndBottom) || (Direction == MovementDirection.LeftAndTop))
                {
                    rect.Width += -pt.X + rect.Left;
                    rect.Location = new Point(pt.X, rect.Location.Y);
                }
                if ((Direction == MovementDirection.Top) || (Direction == MovementDirection.LeftAndTop) || (Direction == MovementDirection.RightAndTop))
                {
                    rect.Height += -pt.Y + rect.Top;
                    rect.Location = new Point(rect.Location.X, pt.Y);
                }
                if ((Direction == MovementDirection.Right) || (Direction == MovementDirection.RightAndBottom) || (Direction == MovementDirection.RightAndTop))
                {
                    rect.Width = pt.X - rect.Location.X;
                }
                if ((Direction == MovementDirection.Bottom) || (Direction == MovementDirection.RightAndBottom) || (Direction == MovementDirection.LeftAndBottom))
                {
                    rect.Height = pt.Y - rect.Location.Y;
                }

                if ((core.KeyDown_ != null) && (core.KeyDown_.Control == true))
                {
                    MagneticFunction();
                }

                if (rect.Width <= 40)
                    rect.Width = 40;
                if (rect.Height <= 40)
                    rect.Height = 40;

                core.Form.Invalidate();
            }
        }

        protected override void MouseMoveCreateNew(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point p = new Point(e.X, e.Y);
                Point Location = rect.Location;
                Size size = rect.Size;
                if (p.X < rect.Location.X)
                    Location.X = p.X;
                if (p.Y < rect.Location.Y)
                    Location.Y = p.Y;
                size.Width = Math.Abs(p.X - rect.Left);
                size.Height = Math.Abs(p.Y - rect.Top);
                rect = new Rectangle(Location, size);
                core.Form.Invalidate();
            }
        }

        protected override void MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                start_point = new Point(e.X, e.Y);
                start_location = rect.Location;

                //нужно определить направление движения прямоугольника
                Point pt = new Point(e.X, e.Y);

                Point[] points = Points;
                if ((Math.Abs(pt.X - points[8].X) <= 2) && (Math.Abs(pt.Y - points[8].Y) <= 2)) // начальный угол
                {
                    Direction = MovementDirection.StartAngle;
                    return;
                }
                if ((Math.Abs(pt.X - points[9].X) <= 2) && (Math.Abs(pt.Y - points[9].Y) <= 2)) // угол разворота
                {
                    Direction = MovementDirection.SweepAngle;
                    return;
                }
                if ((Math.Abs(pt.X - points[0].X) <= 2) && (Math.Abs(pt.Y - points[0].Y) <= 2)) // влево и вверх
                {
                    Direction = MovementDirection.LeftAndTop;
                    return;
                }
                if ((Math.Abs(pt.X - points[1].X) <= 2) && (Math.Abs(pt.Y - points[1].Y) <= 2)) // вверх
                {
                    Direction = MovementDirection.Top;
                    return;
                }
                if ((Math.Abs(pt.X - points[2].X) <= 2) && (Math.Abs(pt.Y - points[2].Y) <= 2)) // вправо и вверх
                {
                    Direction = MovementDirection.RightAndTop;
                    return;
                }
                if ((Math.Abs(pt.X - points[7].X) <= 2) && (Math.Abs(pt.Y - points[7].Y) <= 2)) // влево
                {
                    Direction = MovementDirection.Left;
                    return;
                }
                if ((Math.Abs(pt.X - points[3].X) <= 2) && (Math.Abs(pt.Y - points[3].Y) <= 2)) // вправо
                {
                    Direction = MovementDirection.Right;
                    return;
                }
                if ((Math.Abs(pt.X - points[6].X) <= 2) && (Math.Abs(pt.Y - points[6].Y) <= 2)) // влево и вниз
                {
                    Direction = MovementDirection.LeftAndBottom;
                    return;
                }
                if ((Math.Abs(pt.X - points[5].X) <= 2) && (Math.Abs(pt.Y - points[5].Y) <= 2)) // вниз
                {
                    Direction = MovementDirection.Bottom;
                    return;
                }
                if ((Math.Abs(pt.X - points[4].X) <= 2) && (Math.Abs(pt.Y - points[4].Y) <= 2)) // вправо и вниз
                {
                    Direction = MovementDirection.RightAndBottom;
                    return;
                }
                Direction = MovementDirection.FreeMovement;
            }
            if (e.Button == MouseButtons.Right)
                core.Form.contextMenuStripLine.Show(Control.MousePosition);
        }

        protected override void MouseDownCreateNew(object sender, MouseEventArgs e)
        {
            rect.Location = new Point(e.X, e.Y);
        }

        protected override void MouseUp(object sender, MouseEventArgs e)
        {
            brush.UpdateBrush(this);
            //core.AddToHistory();
        }
        protected override void MouseUpCreateNew(object sende, MouseEventArgs e)
        {
            mouse_move = MouseMove;
            mouse_down = MouseDown;
            mouse_up = MouseUp;

            //core.AddToHistory();
        }

        public override bool IsSelected(Point pt)
        {
            return base.IsSelected(pt);
        }

        public override bool IsSelected(Rectangle rt)
        {
            if ((rt.Left <= rect.Left) && (rt.Right >= rect.Right) && (rt.Top <= rect.Top) && (rt.Bottom >= rect.Bottom))
                return true;
            else
                return false;
        }

        private void MagneticFunction()
        {
            Point pt = new Point();

            if (Direction == MovementDirection.LeftAndTop)
            {
                pt = rect.Location;
                pt = core.Picture.GetNearestPoint(this, pt);
                rect.Width += -pt.X + rect.Left;
                rect.Height += -pt.Y + rect.Top;
                rect.Location = pt;
            }

            if (Direction == MovementDirection.Top)
            {
                pt.Y = rect.Y;
                pt.X = rect.X + rect.Width / 2;
                pt = core.Picture.GetNearestPoint(this, pt);
                rect.Height += -pt.Y + rect.Top;
            }

            if (Direction == MovementDirection.RightAndTop)
            {
                pt.Y = rect.Y;
                pt.X = rect.Right;
                pt = core.Picture.GetNearestPoint(this, pt);
                rect.Width -= -pt.X + rect.Right;
                rect.Height += -pt.Y + rect.Top;
                rect.Y = pt.Y;
            }

            if (Direction == MovementDirection.Left)
            {
                pt.X = rect.X;
                pt.Y = rect.Y + rect.Height / 2;
                pt = core.Picture.GetNearestPoint(this, pt);
                rect.Width += -pt.X + rect.Left;
                rect.X = pt.X;
            }

            if (Direction == MovementDirection.Right)
            {
                pt.X = rect.Right;
                pt.Y = rect.Y + rect.Height / 2;
                pt = core.Picture.GetNearestPoint(this, pt);
                rect.Width -= -pt.X + rect.Right;
            }

            if (Direction == MovementDirection.LeftAndBottom)
            {
                pt.X = rect.X;
                pt.Y = rect.Bottom;
                pt = core.Picture.GetNearestPoint(this, pt);
                rect.Width += -pt.X + rect.Left;
                rect.Height -= -pt.Y + rect.Bottom;
                rect.X = pt.X;
            }

            if (Direction == MovementDirection.Bottom)
            {
                pt.X = rect.X + rect.Width / 2;
                pt.Y = rect.Bottom;
                pt = core.Picture.GetNearestPoint(this, pt);
                rect.Height -= -pt.Y + rect.Bottom;
            }

            if (Direction == MovementDirection.RightAndBottom)
            {
                pt.X = rect.Right;
                pt.Y = rect.Bottom;
                pt = core.Picture.GetNearestPoint(this, pt);
                rect.Height -= -pt.Y + rect.Bottom;
                rect.Width -= -pt.X + rect.Right;
            }
        }
    }
}