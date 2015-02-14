using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace csx
{
    public class Port
    {
        public int napr = 0; //направление отвода линии: 0-влево, 1-вверх, 2-вправо, 3-вниз
        public portInOut inout; //тип порта: 0-in, 1-out, 2-inout
        public string type; //тип переменной (integer, bool ...)
        //public int width; //ширина шины
        public bool bus; //признак шины
        public int LeftBusBound; //левая граница шины
        public int RightBusBound; //правая граница шины
        public bool isLine = false; //флаг присоединения линии к порту
        public Line lin; //присоединенная линия
        public Common parent;
        public string name;

        public string fullBorders
        {
            get
            {
                if (!bus)
                    return "";
                else
                    return "(" + LeftBusBound + (RightBusBound > LeftBusBound ? " to " : " downto ") + RightBusBound + ")";
            }
        }

        public string nameWithParent
        {
            get
            {
                return parent.name + "." + name;
            }
        }

        public string fullName
        {
            get
            {
                if (!bus)
                    return name;
                else
                    return name + "(" + LeftBusBound + (RightBusBound > LeftBusBound ? " to " : " downto ") + RightBusBound + ")";
            }
        }

        public string fullNameWithParent
        {
            get
            {
                return parent.name + "." + fullName;
            }
        }

        public string fullType
        {
            get
            {
                if (!bus)
                    return type.ToString();
                else
                    return type.ToString() + " (" + LeftBusBound + (RightBusBound > LeftBusBound ? " to " : " downto ") + RightBusBound + ")";
            }
        }

        public string fullTypeWithInout
        {
            get
            {
                if (parent != null && parent.isElement)
                    if (((Element)parent).elementType == ElementType.ExternPort)
                    {
                        if (inout == portInOut.In)
                            return portInOut.Out.ToString().ToLower() + " " + fullType;
                        else if (inout == portInOut.Out)
                            return portInOut.In.ToString().ToLower() + " " + fullType;
                    }
                return inout.ToString().ToLower() + " " + fullType;
            }
        }

        public Port(portInOut inout, string type, int napr, string name)
        {
            this.parent = null;
            this.inout = inout;
            this.name = name;
            this.napr = napr;
            this.type = type;
            this.bus = false;
            this.location = new PointF();
        }

        public Port(portInOut inout, string type, int napr, string name, int LeftBusBound, int RightBusBound)
        {
            this.parent = null;
            this.inout = inout;
            this.name = name;
            this.napr = napr;
            this.type = type;
            this.bus = LeftBusBound - RightBusBound != 0; // !!!! HARDCODED
            this.LeftBusBound = LeftBusBound;
            this.RightBusBound = RightBusBound;
            this.location = new PointF();
        }

        public Port(portInOut inout, string type, PointF location, Common parent, int napr, string name)
        {
            this.parent = parent;
            this.inout = inout;
            this.name = name;
            this.napr = napr;
            this.type = type;
            this.bus = false;
            this.location = location;            
        }

        public Port(portInOut inout, string type, PointF location, Common parent, int napr, string name, int LeftBusBound, int RightBusBound)
        {
            this.parent = parent;
            this.inout = inout;
            this.name = name;
            this.napr = napr;
            this.type = type;
            this.bus = true;
            this.LeftBusBound = LeftBusBound;
            this.RightBusBound = RightBusBound;
            this.location = location;            
        }

        public Port(portInOut inout, string type, PointF location, Common parent, int napr, string name, int LeftBusBound, int RightBusBound, bool bus)
        {
            this.parent = parent;
            this.inout = inout;
            this.name = name;
            this.napr = napr;
            this.type = type;
            this.bus = true;
            this.LeftBusBound = LeftBusBound;
            this.RightBusBound = RightBusBound;
            this.bus = bus;
            this.location = location;            
        }

        public PointF getDistance() //отступ от порта в сторону отвода линии 
        //(исключение конфликтности линии с элементом, к которому производится подключение)
        {
            int[] mx = { Construct.interval, 0, -Construct.interval, 0 };
            int[] my = { 0, -Construct.interval, 0, Construct.interval };
            int i, j;
            bool find = false;
            PointF f = new PointF(parent.border.X + location.X/* + mx[napr]*/, parent.border.Y + location.Y/* + my[napr]*/);
            PointF err = new PointF();
            Element el = null;
            Line ln = null;
            while (true)
            {
                f.X += mx[napr];
                f.Y += my[napr];
                find = false;
                for (i = 0; i < parent.parent.ElementsCount; i++)
                {
                    el = parent.parent.ReturnElement(i);
                    if ((f.X > el.border.Left - Construct.interval) && (f.X < el.border.Right + Construct.interval) &&
                        (f.Y > el.border.Top - Construct.interval) && (f.Y < el.border.Bottom + Construct.interval))
                    {
                        find = true;
                        break;
                    }
                }
                if (!find)
                {
                    for (i = 0; i < parent.parent.LinesCount; i++)
                    {
                        ln = parent.parent.ReturnLine(i);
                        if (ln.invisible)
                            continue;
                        if (ln.points != null)
                            for (j = 0; j < ln.points.Length - 1; j++)
                                if (!ln.checkSegment(ln.points[j], ln.points[j + 1], f, ref err))
                                {
                                    find = true;
                                    break;
                                }
                        if (find)
                            break;
                    }
                    if (!find)
                    {
                        return f;
                    }
                }
            }
        }

        public PointF location; //смещение порта относительно родительского элемента

        public Line line
        {
            set
            {
                if (value == null)
                    isLine = false;
                else
                {
                    lin = value;
                    isLine = true;
                }
            }
            get
            {
                return lin;
            }
        }        
    }
}