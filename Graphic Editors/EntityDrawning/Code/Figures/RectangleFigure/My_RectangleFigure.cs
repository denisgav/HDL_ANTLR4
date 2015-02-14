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
    public class My_RectangleFigure : My_Figure
    {
        public Rectangle rect;

        private Point start_point; // используется для перемещения
        private Point start_location;// используется для перемещения

        //Перемещение прямоугольника
        public enum RectangleMovementDirection
        {
            Left,
            Top,
            Right,
            Bottom,
            LeftAndTop,
            RightAndTop,
            LeftAndBottom,
            RightAndBottom,
            FreeMovement
        };

        private RectangleMovementDirection Direction = RectangleMovementDirection.FreeMovement;

        public override Point[] Points
        {
            get
            {
                Point[] points = new Point[8];
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

        public My_RectangleFigure(EntityDrawningCore core)
            : base(core)
        {
            rect = new Rectangle();
            mouse_move = MouseMoveCreateNew;
            mouse_down = MouseDownCreateNew;
            mouse_up = MouseUpCreateNew;
        }

        public My_RectangleFigure(EntityDrawningCore core, Rectangle rect)
            : base(core)
        {
            this.rect = rect;

            mouse_move = MouseMove;
            mouse_down = MouseDown;
            mouse_up = MouseUp;
        }

        public My_RectangleFigure(My_RectangleFigure item)
            : base(item)
        {
            this.rect = item.rect;

            mouse_move = MouseMove;
            mouse_down = MouseDown;
            mouse_up = MouseUp;
        }

        private new void MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point pt = new Point(e.X, e.Y);
                if (Direction == RectangleMovementDirection.FreeMovement)
                {
                    Rectangle rect2 = new Rectangle(rect.Location, rect.Size);
                    rect2.Location = new Point(start_location.X - (this.start_point.X - pt.X), start_location.Y - (this.start_point.Y - pt.Y));
                    rect = rect2;
                }

                if ((Direction == RectangleMovementDirection.Left) || (Direction == RectangleMovementDirection.LeftAndBottom) || (Direction == RectangleMovementDirection.LeftAndTop))
                {
                    rect.Width += -pt.X + rect.Left;
                    rect.Location = new Point(pt.X, rect.Location.Y);
                }
                if ((Direction == RectangleMovementDirection.Top) || (Direction == RectangleMovementDirection.LeftAndTop) || (Direction == RectangleMovementDirection.RightAndTop))
                {
                    rect.Height += -pt.Y + rect.Top;
                    rect.Location = new Point(rect.Location.X, pt.Y);
                }
                if ((Direction == RectangleMovementDirection.Right) || (Direction == RectangleMovementDirection.RightAndBottom) || (Direction == RectangleMovementDirection.RightAndTop))
                {
                    rect.Width = pt.X - rect.Location.X;
                }
                if ((Direction == RectangleMovementDirection.Bottom) || (Direction == RectangleMovementDirection.RightAndBottom) || (Direction == RectangleMovementDirection.LeftAndBottom))
                {
                    rect.Height = pt.Y - rect.Location.Y;
                }

                if ((core.KeyDown_ != null) && (core.KeyDown_.Control == true))
                {
                    MagneticFunction();
                }

                if (rect.Width <= 20)
                    rect.Width = 20;
                if (rect.Height <= 20)
                    rect.Height = 20;

                if ((core.KeyDown_ != null) && (core.KeyDown_.Shift == true))
                    rect.Width = rect.Height;

                core.Form.Invalidate();
            }
        }

        private new void MouseMoveCreateNew(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point p = new Point(e.X, e.Y);
                Point Location = rect.Location;
                Size size = rect.Size;
                if (p.X <= Location.X)
                {
                    Location.X = p.X;
                    size.Width = Math.Abs(p.X - rect.Right);
                }
                else
                    size.Width = Math.Abs(p.X - rect.Left);

                if (p.Y <= Location.Y)
                {
                    Location.Y = p.Y;
                    size.Height = Math.Abs(p.Y - rect.Bottom);
                }
                else
                    size.Height = Math.Abs(p.Y - rect.Top);

                rect = new Rectangle(Location, size);

                if ((core.KeyDown_ != null) && (core.KeyDown_.Shift == true))
                    rect.Width = rect.Height;

                core.Form.Invalidate();
            }
        }

        private new void MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                start_point = new Point(e.X, e.Y);
                start_location = rect.Location;

                //нужно определить направление движения прямоугольника
                Point pt = new Point(e.X, e.Y);

                Point[] points = Points;
                if ((Math.Abs(pt.X - points[0].X) <= 2) && (Math.Abs(pt.Y - points[0].Y) <= 2)) // влево и вверх
                {
                    Direction = RectangleMovementDirection.LeftAndTop;
                    return;
                }
                if ((Math.Abs(pt.X - points[1].X) <= 2) && (Math.Abs(pt.Y - points[1].Y) <= 2)) // вверх
                {
                    Direction = RectangleMovementDirection.Top;
                    return;
                }
                if ((Math.Abs(pt.X - points[2].X) <= 2) && (Math.Abs(pt.Y - points[2].Y) <= 2)) // вправо и вверх
                {
                    Direction = RectangleMovementDirection.RightAndTop;
                    return;
                }
                if ((Math.Abs(pt.X - points[7].X) <= 2) && (Math.Abs(pt.Y - points[7].Y) <= 2)) // влево
                {
                    Direction = RectangleMovementDirection.Left;
                    return;
                }
                if ((Math.Abs(pt.X - points[3].X) <= 2) && (Math.Abs(pt.Y - points[3].Y) <= 2)) // вправо
                {
                    Direction = RectangleMovementDirection.Right;
                    return;
                }
                if ((Math.Abs(pt.X - points[6].X) <= 2) && (Math.Abs(pt.Y - points[6].Y) <= 2)) // влево и вниз
                {
                    Direction = RectangleMovementDirection.LeftAndBottom;
                    return;
                }
                if ((Math.Abs(pt.X - points[5].X) <= 2) && (Math.Abs(pt.Y - points[5].Y) <= 2)) // вниз
                {
                    Direction = RectangleMovementDirection.Bottom;
                    return;
                }
                if ((Math.Abs(pt.X - points[4].X) <= 2) && (Math.Abs(pt.Y - points[4].Y) <= 2)) // вправо и вниз
                {
                    Direction = RectangleMovementDirection.RightAndBottom;
                    return;
                }
                Direction = RectangleMovementDirection.FreeMovement;
            }
            if (e.Button == MouseButtons.Right)
                core.Form.contextMenuStrip1.Show(Control.MousePosition);
        }

        private new void MouseDownCreateNew(object sender, MouseEventArgs e)
        {
            rect.Location = new Point(e.X, e.Y);
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

        private void MagneticFunction()
        {
            Point pt = new Point();

            if (Direction == RectangleMovementDirection.LeftAndTop)
            {
                pt = rect.Location;
                pt = core.Picture.GetNearestPoint(this, pt);
                rect.Width += -pt.X + rect.Left;
                rect.Height += -pt.Y + rect.Top;
                rect.Location = pt;
            }

            if (Direction == RectangleMovementDirection.Top)
            {
                pt.Y = rect.Y;
                pt.X = rect.X + rect.Width / 2;
                pt = core.Picture.GetNearestPoint(this, pt);
                rect.Height += -pt.Y + rect.Top;
            }

            if (Direction == RectangleMovementDirection.RightAndTop)
            {
                pt.Y = rect.Y;
                pt.X = rect.Right;
                pt = core.Picture.GetNearestPoint(this, pt);
                rect.Width -= -pt.X + rect.Right;
                rect.Height += -pt.Y + rect.Top;
                rect.Y = pt.Y;
            }

            if (Direction == RectangleMovementDirection.Left)
            {
                pt.X = rect.X;
                pt.Y = rect.Y + rect.Height / 2;
                pt = core.Picture.GetNearestPoint(this, pt);
                rect.Width += -pt.X + rect.Left;
                rect.X = pt.X;
            }

            if (Direction == RectangleMovementDirection.Right)
            {
                pt.X = rect.Right;
                pt.Y = rect.Y + rect.Height / 2;
                pt = core.Picture.GetNearestPoint(this, pt);
                rect.Width -= -pt.X + rect.Right;
            }

            if (Direction == RectangleMovementDirection.LeftAndBottom)
            {
                pt.X = rect.X;
                pt.Y = rect.Bottom;
                pt = core.Picture.GetNearestPoint(this, pt);
                rect.Width += -pt.X + rect.Left;
                rect.Height -= -pt.Y + rect.Bottom;
                rect.X = pt.X;
            }

            if (Direction == RectangleMovementDirection.Bottom)
            {
                pt.X = rect.X + rect.Width / 2;
                pt.Y = rect.Bottom;
                pt = core.Picture.GetNearestPoint(this, pt);
                rect.Height -= -pt.Y + rect.Bottom;
            }

            if (Direction == RectangleMovementDirection.RightAndBottom)
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