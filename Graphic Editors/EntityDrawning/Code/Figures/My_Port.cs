using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using csx;

namespace Schematix.EntityDrawning
{
    [Serializable]
    public class My_Port : My_Figure
    {
        private Point start_point; // используется при перемещении
        private Point[] array; //используется для перемещения

        private Point point1;
        private Point point2;

        public My_Text TextLabel;

        //Перемещение линии
        public enum LineMovementDirection
        {
            Point1,
            Point2,
            FreeMovement
        };

        //тип порта
        public enum PortType
        {
            Simple,
            Simultaneous,
            Asynchronous
        };
        
        public PortType type;
        public vhdPort vhdPort;
        public bool Inverse;
        public string Name { get; set; }
        
        private LineMovementDirection Direction = LineMovementDirection.FreeMovement;


        public override Point[] Points
        {
            get
            {
                return new Point[2] { point1, point2 };
            }
        }

        private float angle
        {
            get
            {
                float distance = (float)Math.Sqrt((point1.X - point2.X) * (point1.X - point2.X) + (point1.Y - point2.Y) * (point1.Y - point2.Y));
                if (distance == 0)
                    return 0f;
                else
                {
                    float a = ((float)point2.X - (float)point1.X) / distance;
                    a = (float)Math.Acos(a);
                    a = a * 180f / (float)Math.PI;

                    if (point1.Y>point2.Y)
                        a = 360f - a;

                    return a;
                }
            }
        }

        public override Point CenterPoint
        {
            get
            {
                return point2;
            }
        }

        public My_Port(EntityDrawningCore core, PortType type, bool Inverse)
            : base(core)
        {
            this.Inverse = Inverse;
            this.type = type;
            Name = "New_Port";

            TextLabel = new My_Text(core, Name, new Point(0, 0));
            TextLabel.Owner = this;

            vhdPort = new vhdPort();

            draw = Draw;
            mouse_move = MouseMoveCreateNew;
            mouse_down = MouseDownCreateNew;
            mouse_up = MouseUpCreateNew;
        }

        public My_Port(EntityDrawningCore core, String Name, PortType type, bool Inverse, vhdPort vhdPort, Point point1, Point point2)
            : base(core)
        {
            this.Inverse = Inverse;
            this.type = type;
            this.Name = Name;
            this.point1 = point1;
            this.point2 = point2;
            this.vhdPort = vhdPort;

            TextLabel = new My_Text(core, Name, new Point(0, 0));
            TextLabel.Owner = this;

            draw = Draw;
            mouse_move = MouseMove;
            mouse_down = MouseDown;
            mouse_up = MouseUp;
        }

        public My_Port(My_Port item)
            : base(item)
        {
            point1 = item.point1;
            point2 = item.point2;

            Inverse = item.Inverse;
            type = item.type;
            this.Name = item.Name;

            this.vhdPort = item.vhdPort;

            TextLabel = new My_Text(item.TextLabel);
            TextLabel.Owner = this;

            mouse_move = MouseMove;
            mouse_down = MouseDown;
            mouse_up = MouseUp;
            draw = Draw;
        }

        private void DrawBus(Graphics dc)
        {
            Pen pen_ = pen;
            pen_.Width = 4;
            pen_.CompoundArray = new float[] { 0.1f, 0.2f, 0.3f, 0.7f, 0.8f, 0.9f };
            dc.DrawLine(pen_, point1, point2);

            Pen BlackPen = new Pen(Color.Black, 3);
            Point center_point_ = new Point((point1.X + point2.X) / 2, (point1.Y + point2.Y) / 2);
            dc.DrawLine(BlackPen, center_point_.X - 6, center_point_.Y + 6, center_point_.X + 6, center_point_.Y - 6);

            Font font = new Font("Times new roman", 8);
            if (point1.X == point2.X)
            {
                dc.TranslateTransform(-center_point_.X, -center_point_.Y);
                dc.RotateTransform(90.0f);
                dc.DrawString(Math.Abs(vhdPort.leftBound - vhdPort.rightBound).ToString(), font, new SolidBrush(Color.Black), 0, 0);
                dc.RotateTransform(-90.0f);
                dc.TranslateTransform(center_point_.X, center_point_.Y);
            }
            else
                dc.DrawString(Math.Abs(vhdPort.leftBound - vhdPort.rightBound).ToString(), font, new SolidBrush(Color.Black), center_point_);

        }

        private void DrawLine(Graphics dc)
        {
            dc.DrawLine(pen, point1, point2);
        }

        private void DrawArrow(Graphics dc)
        {
            Brush br = new SolidBrush(Color.White);
            Pen p = new Pen(Color.Black);
            switch (type)
            {
                case PortType.Simple:
                    {
                        if (Inverse == true)
                        {
                            dc.FillEllipse(br, point2.X - 3, point2.Y - 3, 6, 6);
                            dc.DrawEllipse(p, point2.X - 3, point2.Y - 3, 6, 6);
                        }
                    }
                    break;
                case PortType.Asynchronous:
                    {
                        //определяем угол
                        float angl = angle;
                   
                        angl = angl * (float)Math.PI / 180f;
                        Point[] triangle = new Point[3];

                        //находим 3 дополнительных координаты
                        Point center, left, right;
                        center = new Point();
                        left = new Point();
                        right = new Point();

                        double x, y;
                        x = (double)16 * Math.Cos(angl);
                        y = (double)16 * Math.Sin(angl);
                        center.X = (int)-x + point2.X;
                        center.Y = (int)-y + point2.Y;

                        x = (double)24 * Math.Cos(angl + Math.PI / 6);
                        y = (double)24 * Math.Sin(angl + Math.PI / 6);
                        left.X = (int)-x + point2.X;
                        left.Y = (int)-y + point2.Y;

                        x = (double)24 * Math.Cos(angl - Math.PI / 6);
                        y = (double)24 * Math.Sin(angl - Math.PI / 6);
                        right.X = (int)-x + point2.X;
                        right.Y = (int)-y + point2.Y;


                        //рисуем стрелочку
                        Point[] points = { left, point2, right, center };
                        dc.FillPolygon(br, points);
                        dc.DrawPolygon(p, points);

                        if (Inverse == true)
                        {
                            x = (double)6 * Math.Cos(angl);
                            y = (double)6 * Math.Sin(angl);
                            Point inv_point = center;
                            inv_point.X -= (int)x;
                            inv_point.Y -= (int)y;

                            dc.FillEllipse(br, inv_point.X - 3, inv_point.Y - 3, 6, 6);
                            dc.DrawEllipse(p, inv_point.X - 3, inv_point.Y - 3, 6, 6);
                        }
                    }
                    break;
                case PortType.Simultaneous:
                    {
                        //определяем угол
                        float angl = angle;

                        angl = angl * (float)Math.PI / 180f;
                        Point[] triangle = new Point[3];

                        //находим 3 дополнительных координаты
                        Point center, left, right;
                        center = new Point();
                        left = new Point();
                        right = new Point();

                        double x, y;
                        x = (double)16 * Math.Cos(angl);
                        y = (double)16 * Math.Sin(angl);
                        center.X = (int)-x + point2.X;
                        center.Y = (int)-y + point2.Y;

                        x = (double)24 * Math.Cos(angl + Math.PI / 6);
                        y = (double)24 * Math.Sin(angl + Math.PI / 6);
                        left.X = (int)x + center.X;
                        left.Y = (int)y + center.Y;

                        x = (double)24 * Math.Cos(angl - Math.PI / 6);
                        y = (double)24 * Math.Sin(angl - Math.PI / 6);
                        right.X = (int)x + center.X;
                        right.Y = (int)y + center.Y;


                        //рисуем стрелочку
                        Point[] points = { left, point2, right, center };
                        dc.FillPolygon(br, points);
                        dc.DrawPolygon(p, points);

                        if (Inverse == true)
                        {
                            x = (double)6 * Math.Cos(angl);
                            y = (double)6 * Math.Sin(angl);
                            Point inv_point = center;
                            inv_point.X -= (int)x;
                            inv_point.Y -= (int)y;

                            dc.FillEllipse(br, inv_point.X - 3, inv_point.Y - 3, 6, 6);
                            dc.DrawEllipse(p, inv_point.X - 3, inv_point.Y - 3, 6, 6);
                        }
                    };
                    break;

                default:
                    break;

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
                if (vhdPort.bus == false)
                    DrawLine(dc);
                else
                    DrawBus(dc);
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
                dc.DrawLine(pen2, CenterPoint, TextLabel.CenterPoint);
            }

            DrawArrow(dc);
            TextLabel.draw(sender, e);
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

                    if (Math.Abs(point1.X - point2.X) < Math.Abs(point1.Y - point2.Y))
                    {
                        point1.X = point2.X;
                    }
                    else
                    {
                        point1.Y = point2.Y;
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

                    if (Math.Abs(point1.X - point2.X) < Math.Abs(point1.Y - point2.Y))
                    {
                        point2.X = point1.X;
                    }
                    else
                    {
                        point2.Y = point1.Y;
                    }

                    if ((core.KeyDown_ != null) && (core.KeyDown_.Control == true))
                    {
                        point2 = core.Picture.GetNearestPoint(this, point2);
                    }
                }

                core.Form.Invalidate();
            }
        }

        private new void MouseMoveCreateNew(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                point2.X = e.X;
                point2.Y = e.Y;

                if (Math.Abs(point1.X - point2.X) < Math.Abs(point1.Y - point2.Y))
                {
                    point2.X = point1.X;
                }
                else
                {
                    point2.Y = point1.Y;
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
                core.Form.contextMenuStripPort.Show(Control.MousePosition);
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