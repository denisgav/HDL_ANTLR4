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
    public class My_MultypointFigure : My_Figure
    {
        private Point start_point; // используется при перемещении
        private int SelectedPointIndex; // используется при перемещении

        [Serializable]
        public class My_Point
        {
            public int X;
            public int Y;

            public My_Point(int X, int Y)
            {
                this.X = X;
                this.Y = Y;
            }

            public My_Point()
            {
                this.X = 0;
                this.Y = 0;
            }
        }

        private bool MoveFigure = false; // true - перемещение фигуры, иначе - некоторой точки фигуры

        private List<My_Point> points;
        private Point[] array; //используется для перемещения
        public override Point[] Points
        {
            get
            {
                Point[] arr = new Point[points.Count];
                for (int i = 0; i < points.Count; i++)
                    arr[i] = new Point(points[i].X, points[i].Y);
                return arr;
            }
        }

        public override Point CenterPoint
        {
            get
            {
                if (points.Count > 2)
                {
                    Rectangle rect = new Rectangle(points[0].X, points[0].Y, 0, 0);
                    Point p1 = new Point(points[0].X, points[0].Y);
                    Point p2 = new Point(points[2].X, points[2].Y);
                    for (int num = 2; num < points.Count; num++)
                    {
                        Point p = new Point(points[num].X, points[num].Y);
                        Point center = new Point(((p1.X + p.X) / 2), ((p1.Y + p.Y) / 2));
                        if (IsSelected(center) == true)
                        {
                            return center;
                        }
                    }
                    return new Point(((p1.X + p2.X) / 2), ((p1.Y + p2.Y) / 2));
                }
                else
                    return new Point(points[0].X, points[0].Y);
            }
            set
            {
                base.CenterPoint = value;
            }
        }

        public virtual void Fix()
        {
            mouse_move = MouseMove;
            mouse_down = MouseDown;
            mouse_up = MouseUp;

            core.Lock = false;

            //core.AddToHistory();
            core.Form.Invalidate();
        }

        public My_MultypointFigure(EntityDrawningCore core)
            : base(core)
        {
            points = new List<My_Point>();

            mouse_move = MouseMoveCreateNew;
            mouse_down = MouseDownCreateNew;
            mouse_up = MouseUpCreateNew;
        }

        public My_MultypointFigure(My_MultypointFigure item)
            : base(item)
        {
            points = new List<My_Point>();
            foreach (My_Point p in item.points)
            {
                My_Point pt = new My_Point(p.X, p.Y);
                points.Add(pt);
            }

            mouse_move = MouseMove;
            mouse_down = MouseDown;
            mouse_up = MouseUp;
        }

        protected virtual void DrawCreateNew(object sender, PaintEventArgs e)
        {
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
                if (MoveFigure == false)
                {
                    Point pt = new Point(e.X, e.Y);
                    if ((core.KeyDown_ != null) && (core.KeyDown_.Control == true))
                        pt = core.Picture.GetNearestPoint(this, pt);
                    points.ElementAt<My_Point>(SelectedPointIndex).X = pt.X;
                    points.ElementAt<My_Point>(SelectedPointIndex).Y = pt.Y;
                }
                else
                {
                    int delta_x = start_point.X - e.X;
                    int delta_y = start_point.Y - e.Y;

                    int index = 0;
                    foreach (My_Point p in points)
                    {
                        p.X = array[index].X - delta_x;
                        p.Y = array[index].Y - delta_y;
                        index++;
                    }
                }
                core.Form.Invalidate();
            }
        }

        protected override void MouseMoveCreateNew(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                SelectedPointIndex = points.Count - 1;
                Point pt = new Point(e.X, e.Y);
                if ((core.KeyDown_ != null) && (core.KeyDown_.Control == true))
                    pt = core.Picture.GetNearestPoint(this, pt);
                points.ElementAt<My_Point>(SelectedPointIndex).X = pt.X;
                points.ElementAt<My_Point>(SelectedPointIndex).Y = pt.Y;
                core.Form.Invalidate();
            }
        }

        protected override void MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                start_point = new Point(e.X, e.Y);

                //нужно определить направление движения прямоугольника
                Point pt = new Point(e.X, e.Y);

                int? MinDistance = null;
                foreach (My_Point p in points)
                {
                    int D = (p.X - pt.X) * (p.X - pt.X) + (p.Y - pt.Y) * (p.Y - pt.Y);
                    if ((MinDistance == null) || (D < MinDistance))
                    {
                        MinDistance = D;
                        SelectedPointIndex = points.IndexOf(p);
                    }
                }
                if (MinDistance > 50)
                {
                    array = Points;
                    MoveFigure = true;
                }
                else
                {
                    MoveFigure = false;
                }
            }
            if (e.Button == MouseButtons.Right)
            {
                if(this is My_Polygon)
                    core.Form.contextMenuStrip1.Show(Control.MousePosition);
                else
                    core.Form.contextMenuStripLine.Show(Control.MousePosition);
            }
        }

        protected override void MouseDownCreateNew(object sender, MouseEventArgs e)
        {
            Point p = new Point(e.X, e.Y);
            if((core.KeyDown_ != null) && (core.KeyDown_.Control == true))
                p = core.Picture.GetNearestPoint(this, p);

            My_Point pt = new My_Point(p.X, p.Y);

            points.Add(pt);
            SelectedPointIndex = points.Count - 1;
            core.Form.Invalidate();
        }

        protected override void MouseUp(object sender, MouseEventArgs e)
        {
            core.Lock = false;
            brush.UpdateBrush(this);
            //core.AddToHistory();
        }

        protected override void MouseUpCreateNew(object sende, MouseEventArgs e)
        {
            core.Lock = true;
            //core.AddToHistory();
        }

        public override bool IsSelected(Point pt)
        {
            return base.IsSelected(pt);
        }

        public override bool IsSelected(Rectangle rt)
        {
            foreach (Point p in Points)
            {
                if ((rt.Left < p.X) && (rt.Top < p.Y) && (rt.Bottom > p.Y) && (rt.Right > p.X))
                    continue;
                else
                    return false;
            }
            return true;
        }
    }
}