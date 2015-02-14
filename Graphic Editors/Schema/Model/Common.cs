using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Drawing;

namespace csx
{
    public class Common //компонент, €вл€ющийс€ родительским дл€ узлов и элементов
    {
        public static int count = 0;
        public string name;
        public Construct parent;
        public int countIn = 0;
        public int countOut = 0;
        public List<Port> ports;
        public RectangleF border;
        public bool isElement = false; //флаг элемент/узел
        public void Add(Port port) //добавление порта в компонент
        {
            switch (port.inout)
            {
                case portInOut.In:
                    countIn++;
                    break;
                case portInOut.Out:
                    countOut++;
                    break;
            }
            port.parent = this;
            ports.Add(port);
        }
        public virtual void Draw(Graphics e) //функци€, перегружаема€ в потомках
        {            
        }
        public bool check(PointF a, PointF b, ref PointF c) //проверка пересечени€ компонента линией a-b
        //при обнаружении пересечени€ выдаетс€ отрезок максимально возможной длины, который не будет пересекать элемент (лини€ a-c)
        {
            RectangleF rect;
            if (a.X > border.Left - Construct.interval && a.X < border.Right + Construct.interval &&
                a.Y > border.Top - Construct.interval && a.Y < border.Bottom + Construct.interval)
                return true;
            if (a.Y == b.Y && a.Y > border.Top - Construct.interval && a.Y < border.Top + border.Height + Construct.interval)
                if (a.X < b.X)
                    if (a.X < border.Left - Construct.interval && b.X > border.Left - Construct.interval)
                    {
                        c = new PointF(border.Left - Construct.interval, a.Y);
                        rect = new RectangleF(border.Location, border.Size);
                        return false;
                    }
                    else
                        return true;
                else
                    if (a.X > border.Right + Construct.interval && b.X < border.Right + Construct.interval)
                    {
                        c = new PointF(border.Right + Construct.interval, a.Y);
                        rect = new RectangleF(border.Location, border.Size);
                        return false;
                    }
                    else
                        return true;
            else
                if (a.X > border.Left - Construct.interval && a.X < border.Right + Construct.interval)
                    if (a.Y < b.Y)
                        if (a.Y < border.Top - Construct.interval && b.Y > border.Top - Construct.interval)
                        {
                            c = new PointF(a.X, border.Top - Construct.interval);
                            rect = new RectangleF(border.Location, border.Size);
                            return false;
                        }
                        else
                            return true;
                    else
                        if (a.Y > border.Bottom + Construct.interval && b.Y < border.Bottom + Construct.interval)
                        {
                            c = new PointF(a.X, border.Bottom + Construct.interval);
                            rect = new RectangleF(border.Location, border.Size);
                            return false;
                        }
                        else
                            return true;
                else
                    return true;
        }
        public void DrawSelect(Graphics e) //рисование выделени€ при наведении мыши
        {
            if (isElement)
                e.FillRectangle(new SolidBrush(Color.FromArgb(160, Color.LightGreen/*141, 254, 162*/)), border.X - 4, border.Y - 4, border.Width + 8, border.Height + 8);
            else
                e.FillRectangle(new SolidBrush(Color.FromArgb(160, Color.LightGreen/*141, 254, 162*/)), border.X - border.Width - 5, border.Y - border.Height - 5, border.Width * 2 + 10, border.Height * 2 + 10);
        }
    }
}
