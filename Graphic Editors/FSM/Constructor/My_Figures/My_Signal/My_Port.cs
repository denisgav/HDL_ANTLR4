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
    public class My_Port : My_Signal
    {
        public enum PortDirection
        {
            In, Out, InOut, Buffer
        };

        public enum PortType
        {
            Combinatioral, Registered, Clocked, Clock, ClockEnable
        };

        public PortDirection Direction {get; set;}
        public PortType Port_Type { get; set; }

        public override Rectangle rect
        {
            get
            {
                Rectangle rt = new Rectangle(CenterPoint.X - 20, CenterPoint.Y - 10, 40, 20);
                return rt;
            }
        }

        public My_Port(Schematix.FSM.Constructor_Core core, PortDirection Direction) :
            base(core)
        {
            Color = Color.Aqua;
            this.Direction = Direction;
            this.Port_Type = PortType.Registered;

            base.name = ("Port" + core.Graph.Ports.Count.ToString());
            base.label_name.Text = name;
        }

        public My_Port(Schematix.FSM.Constructor_Core core) :
            base(core)
        {
            Color = Color.Aqua;
            this.Direction = PortDirection.In;
            this.Port_Type = PortType.Registered;

            base.name = ("Port" + core.Graph.Ports.Count.ToString());
            base.label_name.Text = name;
        }

        public My_Port(My_Port item) :
            base(item as My_Signal)
        {
            this.Direction = item.Direction;
            this.Port_Type = item.Port_Type;
        }

        public My_Port(string name, string Type, Point center_point, Schematix.FSM.Constructor_Core core) :
            base(name, Type, null, center_point, core)
        {
            Color = Color.Aqua;
        }

        private void draw_type(Graphics dc, Rectangle r)
        {
            switch (Port_Type)
            {
                case PortType.Combinatioral:
                    break;

                case PortType.Registered:
                    {
                        Point[] pt = new Point[4];
                        pt[0].X = r.Left + 2;
                        pt[0].Y = r.Top + 2;

                        pt[1].X = r.Left + r.Width / 2;
                        pt[1].Y = r.Top + 2;

                        pt[2].X = r.Left + r.Width / 2;
                        pt[2].Y = r.Bottom - 2;

                        pt[3].X = r.Right - 2;
                        pt[3].Y = r.Bottom - 2;
                        dc.DrawLines(new Pen(Color.Black), pt);
                    }
                    break;

                case PortType.Clocked:
                    {
                        Pen pen = new Pen(Color.Black);
                        Point[] pt = new Point[4];
                        pt[0].X = r.Left + 2;
                        pt[0].Y = r.Top + 2;

                        pt[1].X = r.Left + r.Width / 2;
                        pt[1].Y = r.Top + 2;

                        pt[2].X = r.Left + r.Width / 2;
                        pt[2].Y = r.Bottom - 2;

                        pt[3].X = r.Right - 2;
                        pt[3].Y = r.Bottom - 2;
                        dc.DrawLines(pen, pt);

                        Point p1 = new Point(r.Left + r.Width / 2 + 2, r.Top + 2);
                        Point p2 = new Point(r.Right - 2, r.Top + 2);
                        Point p3 = new Point(r.Left + r.Width / 2 + 2, r.Bottom - 4);
                        Point p4 = new Point(r.Right - 2, r.Bottom - 4);

                        dc.DrawLine(pen, p1, p4);
                        dc.DrawLine(pen, p2, p3);
                    }
                    break;

                case PortType.Clock:
                    {
                        Point[] pt = new Point[10];

                        pt[0].X = r.Left + 2;
                        pt[0].Y = r.Bottom - 2;

                        pt[1].X = r.Left + r.Width / 8;
                        pt[1].Y = r.Bottom - 2;

                        pt[2].X = r.Left + r.Width / 8;
                        pt[2].Y = r.Bottom - r.Height / 2;

                        pt[3].X = r.Left + (3 * r.Width / 8);
                        pt[3].Y = r.Bottom - r.Height / 2;

                        pt[4].X = r.Left + (3 * r.Width / 8);
                        pt[4].Y = r.Bottom - 2;

                        pt[5].X = r.Left + (5 * r.Width / 8);
                        pt[5].Y = r.Bottom - 2;

                        pt[6].X = r.Left + (5 * r.Width / 8);
                        pt[6].Y = r.Bottom - r.Height / 2;

                        pt[7].X = r.Left + (7 * r.Width / 8);
                        pt[7].Y = r.Bottom - r.Height / 2;

                        pt[8].X = r.Left + (7 * r.Width / 8);
                        pt[8].Y = r.Bottom - 2;

                        pt[9].X = r.Right - 2;
                        pt[9].Y = r.Bottom - 2;

                        dc.DrawLines(new Pen(Color.Black), pt);
                    }
                    break;

                case PortType.ClockEnable:
                    {
                        System.Drawing.StringFormat format = new StringFormat();
                        format.Alignment = StringAlignment.Center;
                        format.LineAlignment = StringAlignment.Center;
                        dc.DrawString("CE", new Font("Times New Roman", 8, FontStyle.Bold), new SolidBrush(Color.Black), r, format);
                    }
                    break;
            }
        }

        private void draw_port(Graphics dc)
        {
            Brush brush;
            if (selected == false)
                brush = new SolidBrush(Color);
            else
                brush = new SolidBrush(SelectedColor);

            switch (Direction)
            {
                case PortDirection.In:
                    {
                        Point[] points = new Point[8];

                        points[0].X = rect.Left;
                        points[0].Y = rect.Bottom;

                        points[1].X = rect.Left;
                        points[1].Y = rect.Top;

                        points[2].X = rect.Left + rect.Width / 2;
                        points[2].Y = rect.Top;

                        points[3].X = rect.Left + (3 * rect.Width / 4);
                        points[3].Y = rect.Top + rect.Height / 2;

                        points[4].X = rect.Right;
                        points[4].Y = rect.Top + rect.Height / 2;

                        points[5].X = rect.Left + (3 * rect.Width / 4);
                        points[5].Y = rect.Top + rect.Height / 2;

                        points[6].X = rect.Left + rect.Width / 2;
                        points[6].Y = rect.Bottom;

                        points[7].X = rect.Left;
                        points[7].Y = rect.Bottom;

                        dc.FillPolygon(brush, points);
                        dc.DrawPolygon(new Pen(Color.Black), points);

                        draw_type(dc, new Rectangle(rect.Left, rect.Top, rect.Width / 2, rect.Height));
                    }
                    break;

                case PortDirection.Out:
                    {
                        Point[] points = new Point[9];

                        points[0].X = rect.Left;
                        points[0].Y = rect.Top + rect.Height / 2;

                        points[1].X = rect.Left + rect.Width / 4;
                        points[1].Y = rect.Top + rect.Height / 2;

                        points[2].X = rect.Left + rect.Width / 4;
                        points[2].Y = rect.Top;

                        points[3].X = rect.Left + (3 * rect.Width / 4);
                        points[3].Y = rect.Top;

                        points[4].X = rect.Right;
                        points[4].Y = rect.Top + rect.Height / 2;

                        points[5].X = rect.Left + (3 * rect.Width / 4);
                        points[5].Y = rect.Bottom;

                        points[6].X = rect.Left + rect.Width / 4;
                        points[6].Y = rect.Bottom;

                        points[7].X = rect.Left + rect.Width / 4;
                        points[7].Y = rect.Top + rect.Height / 2;

                        points[8].X = rect.Left;
                        points[8].Y = rect.Top + rect.Height / 2;

                        dc.FillPolygon(brush, points);
                        dc.DrawPolygon(new Pen(Color.Black), points);

                        draw_type(dc, new Rectangle(rect.Left + rect.Width/4, rect.Top, rect.Width / 2, rect.Height));
                    }
                    break;

                case PortDirection.InOut:
                    {
                        Point[] points = new Point[12];

                        points[0].X = rect.Left;
                        points[0].Y = rect.Top + rect.Height / 2;

                        points[1].X = rect.Left + rect.Width / 8;
                        points[1].Y = rect.Top + rect.Height / 2;

                        points[2].X = rect.Left + rect.Width / 4;
                        points[2].Y = rect.Top;

                        points[3].X = rect.Right - rect.Width / 4;
                        points[3].Y = rect.Top;

                        points[4].X = rect.Right - rect.Width / 8;
                        points[4].Y = rect.Top + rect.Height / 2;

                        points[5].X = rect.Right;
                        points[5].Y = rect.Top + rect.Height / 2;

                        points[6].X = rect.Right - rect.Width / 8;
                        points[6].Y = rect.Top + rect.Height / 2;

                        points[7].X = rect.Right - rect.Width / 4;
                        points[7].Y = rect.Bottom;

                        points[8].X = rect.Right - rect.Width / 4;
                        points[8].Y = rect.Bottom;

                        points[9].X = rect.Left + rect.Width / 4;
                        points[9].Y = rect.Bottom;

                        points[10].X = points[1].X;
                        points[10].Y = points[1].Y;

                        points[11].X = points[0].X;
                        points[11].Y = points[0].Y;

                        dc.FillPolygon(brush, points);
                        dc.DrawPolygon(new Pen(Color.Black), points);

                        draw_type(dc, new Rectangle(rect.Left + rect.Width / 4, rect.Top, rect.Width / 2, rect.Height));
                    }
                    break;

                case PortDirection.Buffer:
                    {
                        Point[] points = new Point[8];

                        points[0].X = rect.Left;
                        points[0].Y = rect.Top + rect.Height / 2;

                        points[1].X = rect.Left + rect.Width / 4;
                        points[1].Y = rect.Top + rect.Height / 2;

                        points[2].X = rect.Left + rect.Width / 4;
                        points[2].Y = rect.Top;

                        points[3].X = rect.Right;
                        points[3].Y = rect.Top;

                        points[4].X = rect.Right;
                        points[4].Y = rect.Bottom;

                        points[5].X = rect.Left + rect.Width / 4;
                        points[5].Y = rect.Bottom;

                        points[6].X = rect.Left + rect.Width / 4;
                        points[6].Y = rect.Top + rect.Height / 2; ;

                        points[7].X = rect.Left;
                        points[7].Y = rect.Top + rect.Height / 2;

                        dc.FillPolygon(brush, points);
                        dc.DrawPolygon(new Pen(Color.Black), points);

                        draw_type(dc, new Rectangle(rect.Left + rect.Width / 4, rect.Top, 3 * rect.Width / 4, rect.Height));
                    }
                    break;

                default:
                    break;
            }
        }

        public override void Draw(object sender, PaintEventArgs e)
        {
            Graphics dc = e.Graphics;

            draw_port(dc);

            Pen pen = new Pen(Color.Black);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

            dc.DrawLine(pen, CenterPoint, label_name.CenterPoint);

            label_name.draw(sender, e);
        }
    }
}