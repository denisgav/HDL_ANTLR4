using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;

namespace csx
{
    public class Connect : Common //узел
    {
        public Connect(PointF location, Construct parent)
        {
            this.name = "Connect_" + count.ToString();
            count++;
            this.parent = parent;
            this.border.Location = location;
            this.border.Width = this.border.Height = (int)Construct.interval / 2;
            ports = new List<Port>();
            Port p = new Port(portInOut.InOut, "std_logic_vector", new PointF((int)Construct.interval / 2, 0), this, 0, "InOutRight");
            ports.Add(p);
            p = new Port(portInOut.InOut, "std_logic_vector", new PointF(0, -(int)Construct.interval / 2), this, 1, "InOutTop");
            ports.Add(p);
            p = new Port(portInOut.InOut, "std_logic_vector", new PointF(-(int)Construct.interval / 2, 0), this, 2, "InOutLeft");
            ports.Add(p);
            p = new Port(portInOut.InOut, "std_logic_vector", new PointF(0, (int)Construct.interval / 2), this, 3, "InOutBottom");
            ports.Add(p);
        }

        public Connect(PointF location, Construct parent, string name)
        {
            this.name = name;
            this.parent = parent;
            this.border.Location = location;
            this.border.Width = this.border.Height = (int)Construct.interval / 2;
            ports = new List<Port>();
            Port p = new Port(portInOut.InOut, "std_logic_vector", new PointF((int)Construct.interval / 2, 0), this, 0, "InOutRight");
            ports.Add(p);
            p = new Port(portInOut.InOut, "std_logic_vector", new PointF(0, -(int)Construct.interval / 2), this, 1, "InOutTop");
            ports.Add(p);
            p = new Port(portInOut.InOut, "std_logic_vector", new PointF(-(int)Construct.interval / 2, 0), this, 2, "InOutLeft");
            ports.Add(p);
            p = new Port(portInOut.InOut, "std_logic_vector", new PointF(0, (int)Construct.interval / 2), this, 3, "InOutBottom");
            ports.Add(p);
        }

        public override void Draw(Graphics e)
        {
            RectangleF rct = new RectangleF(border.X - border.Width,border.Y - border.Height, 2 * border.Width, 2 * border.Height);
            e.FillEllipse(Brushes.DeepSkyBlue, rct);
            e.DrawArc(Pens.Black, rct, 0, 360);
        }
    }
}
