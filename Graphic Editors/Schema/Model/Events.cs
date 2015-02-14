using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace csx
{    
    public class Events //обработка событий
    {
        //тестовые переменные
        //public int[] horp = new int[0];
        //public int[] verp = new int[0];
        //public long[,] mat = new long[0, 0];
        //public int hcnt = 0, vcnt = 0;


        //public ArrayList selectedCommon;
        public Construct parent;

        private bool mouseDown = false; //флаг удержания мыши в нажатом положении
        private Point mouseLocation;
        private PointF mouseDownLocation; //позиция в которой произвелось нажатие мыши

        private bool selectElement = false; //флаг выбора элемента
        private Element selectedElement = null; //выбранный элемент
        private bool isSelectedElementInBuffer = false; //выбранный элемент выбран
        public Element nearElement = null; //ближайший элемент к указателю
        //public Element nearElement
        //{
        //    set
        //    {                
        //        if (NearElement != value)
        //        {
        //            NearElement = value;                    
        //            //parent.parent.Invalidate();
        //        }
        //        if (isNearElement && !IsNearPort && !IsNearLine)
        //        {
        //            tipElement(NearElement, parent.parent.tip, parent.parent, mouseLocation);
        //            status = NearElement.name;
        //        }
        //    }
        //    get
        //    {
        //        return NearElement;
        //    }
        //}
        private bool isNearElement = false;
        //private bool isNearElement
        //{
        //    set
        //    {
        //        if (IsNearElement != value)
        //        {
        //            IsNearElement = value;
        //            if (value == false)
        //            {
        //                tipHide(parent.parent.tip, parent.parent);
        //                status = "";
        //            }
        //            //else
        //            //{
        //            //    if (NearElement != null && !IsNearPort && !IsNearLine)
        //            //    {
        //            //        if (mouseLocation != mousePopupLocation)
        //            //        {
        //            //            tipElement(NearElement, parent.parent.tip, parent.parent, mouseLocation);
        //            //            mousePopupLocation = mouseLocation;
        //            //        }
        //            //        status = NearElement.name;
        //            //    }
        //            //}
        //            //parent.parent.Invalidate();
        //        }
        //    }
        //    get
        //    {
        //        return IsNearElement;
        //    }
        //}

        private bool selectConnect = false;
        private Connect selectedConnect = null;
        private Connect nearConnect = null;
        //private Connect nearConnect
        //{
        //    set
        //    {
        //        if (NearConnect != value)
        //        {
        //            NearConnect = value;
        //            //parent.parent.Invalidate();
        //        }
        //        if (isNearConnect && !IsNearPort && !IsNearLine)
        //        {
        //            tipConnect(NearConnect, parent.parent.tip, parent.parent, mouseLocation);
        //            status = NearConnect.name;
        //        }
        //    }
        //    get
        //    {
        //        return NearConnect;
        //    }
        //}
        private bool isNearConnect = false;
        //private bool isNearConnect
        //{
        //    set
        //    {
        //        if (IsNearConnect != value)
        //        {
        //            IsNearConnect = value;
        //            if (value == false)
        //            {
        //                tipHide(parent.parent.tip, parent.parent);
        //                status = "";
        //            }
        //            //else
        //            //{
        //            //    if (nearConnect != null && IsNearPort && !IsNearLine)
        //            //    {
        //            //        tipConnect(NearConnect, parent.parent.tip, parent.parent, mouseLocation);
        //            //        status = nearConnect.name;
        //            //    }
        //            //}
        //        }
        //    }
        //    get
        //    {
        //        return IsNearConnect;
        //    }
        //}


        private bool selectLine = false;
        private Line selectedLine = null;
        private Line nearLine = null;
        //private Line nearLine
        //{
        //    set
        //    {
        //        if (NearLine != value)
        //        {
        //            NearLine = value;
        //            parent.parent.Invalidate();
        //        }
        //        if (isNearLine && !IsNearPort)
        //        {
        //            tipLine(NearLine, parent.parent.tip, parent.parent, mouseLocation);
        //            status = NearLine.parentBegin.parent.name + ":" + NearLine.parentBegin.name +
        //                        " <--> " + NearLine.parentEnd.parent.name + ":" + NearLine.parentEnd.name;
        //        }
        //    }
        //    get
        //    {
        //        return NearLine;
        //    }
        //}
        private int selectedSegment = 0; //номер выбранного сегмента в линии
        private bool isVertSegment = false; //флаг вертикальности/горизонтальности выбранного сегмента
        public bool isNearLine = false;
        //public bool isNearLine
        //{
        //    set
        //    {
        //        if (IsNearLine != value)
        //        {
        //            IsNearLine = value;
        //            if (value == false)
        //            {
        //                tipHide(parent.parent.tip, parent.parent);
        //                status = "";
        //            }
        //            //else
        //            //{
        //            //    if (NearLine != null && IsNearPort)
        //            //    {
        //            //        tipLine(NearLine, parent.parent.tip, parent.parent, mouseLocation);
        //            //        status = NearLine.parentBegin.parent.name + ":" + NearLine.parentBegin.name + 
        //            //            " <--> " + NearLine.parentEnd.parent.name + ":" + NearLine.parentEnd.name;
        //            //    }
        //            //}
        //            //parent.parent.Invalidate();
        //        }
        //    }
        //    get
        //    {
        //        return IsNearLine;
        //    }
        //}

        private bool selectPort = false;
        private Port selectedPort = null;
        private Port nearPort = null;
        //private Port nearPort
        //{
        //    set
        //    {                
        //        if (NearPort != value)
        //        {
        //            NearPort = value;
        //            parent.parent.Invalidate();
        //        }
        //        if (isNearPort)
        //        {
        //            tipPort(NearPort, parent.parent.tip, parent.parent, mouseLocation);
        //            status = NearPort.parent.name + ":" + NearPort.name;
        //        }
        //    }
        //    get
        //    {
        //        return NearPort;
        //    }
        //}

        private bool isNearPort = false;
        //private bool isNearPort
        //{
        //    set
        //    {
        //        if (IsNearPort != value)
        //        {
        //            IsNearPort = value;
        //            if (value == false)
        //            {
        //                tipHide(parent.parent.tip, parent.parent);
        //                status = "";
        //                parent.parent.Invalidate();
        //            }
        //            //else
        //            //{
        //            //    if (NearPort != null)
        //            //    {
        //            //        tipPort(NearPort, parent.parent.tip, parent.parent, mouseLocation);
        //            //        status = NearPort.parent.name + ":" + NearPort.name;
        //            //    }
        //            //}
        //        }
        //    }
        //    get
        //    {
        //        return IsNearPort;
        //    }
        //}


        private bool drawConnect = false; //флаг рисования активного(добавляемого) узла
        private PointF connectLocation; //размещение активного(добавляемого) узла

        private bool drawLine = false; //флаг рисования добавляемой(редактируемой) линии
        private PointF linePointF1; //начало добавляемой(редактируемой) линии
        private PointF linePointF2; //конец добавляемой(редактируемой) линии

        private int connectDistanceSQR = 9; //квадрат расстояния при котором производится соединение(сглаживание) (для ускорения вычислений)
        public static int connectDistance = 3; //расстояние при котором производится соединение(сглаживание)

        //private GraphicsContainer buffer;
        //private bool useBuffer = false;

        //private bool invalidate = true;

        private bool control = false; //флаг зажатия Ctrl

        private bool moved = false; //флаг перемещения

        private string Status; //строка состояния
        public string status
        {
            set
            {
                Status = value;
                //parent.parent.setStatus(value);
            }
            get
            {
                return Status;
            }
        }

        private Rectangle selectingRect; //прямоугольник выделения
        private bool isSelectingRect = false; //флаг выделения

        public Events(Construct parent)
        {
            this.parent = parent;
        }

        private void tipHide(ToolTip tip, IWin32Window win)
        {
            tip.Hide(win);
        }

        public void ExecuteTip()
        {
            if (isNearConnect)
            {
                tipConnect(nearConnect, parent.parent.tip, parent.parent, mouseLocation);
            }
            else
                if (isNearPort)
                {
                    tipPort(nearPort, parent.parent.tip, parent.parent, mouseLocation);
                }
                else
                    if (isNearLine)
                    {
                        tipLine(nearLine, parent.parent.tip, parent.parent, mouseLocation);
                    }
                    else
                        if (isNearElement)
                        {
                            tipElement(nearElement, parent.parent.tip, parent.parent, mouseLocation);
                        }
                        else
                            parent.parent.tip.Hide(parent.parent);
        }

        private int tipDx = 20;
        private int tipDy = 50;

        private const int tipD = 3;

        private Point LastTipElementPosition = new Point(-1, -1);
        private Element LastTipElement = null;
        private string TipElementCap = "";

        private void tipElement(Element el, ToolTip tip, IWin32Window win, Point position)
        {
            if (el == LastTipElement)
            {
                if (Math.Abs(position.X - LastTipElementPosition.X) > tipD ||
                    Math.Abs(position.Y - LastTipElementPosition.Y) > tipD)
                {
                    LastTipElementPosition = position;
                    if (el.elementType == ElementType.ExternPort)
                        tip.ToolTipTitle = "ExternPort " + el.name;
                    else if (el.elementType == ElementType.Element)
                        tip.ToolTipTitle = "Element " + el.name;
                    else if (el.elementType == ElementType.Terminator)
                        tip.ToolTipTitle = "Terminator";
                    tip.Show(TipElementCap, win, position.X + tipDx - (int)parent.parent.ScrollDx, 
                        position.Y + tipDy - (int)parent.parent.ScrollDy);
                }
            }
            else
            {
                LastTipElement = el;
                LastTipElementPosition = position;
                TipElementCap = "";
                if (el.elementType == ElementType.ExternPort)
                    tip.ToolTipTitle = "ExternPort " + el.name;
                else if (el.elementType == ElementType.Element)
                    tip.ToolTipTitle = "Element " + el.name;
                else if (el.elementType == ElementType.Terminator)
                    tip.ToolTipTitle = "Terminator";
                foreach (Port pt in el.ports)
                    TipElementCap += pt.name + "\t: " + pt.inout.ToString() + " " + pt.fullType + "\n";
                tip.Show(TipElementCap, win, position.X + tipDx - (int)parent.parent.ScrollDx,
                        position.Y + tipDy - (int)parent.parent.ScrollDy);
            }
        }

        private Point LastTipConnectPosition = new Point(-1, -1);
        private Connect LastTipConnect = null;
        private string TipConnectCap = "";

        private void tipConnect(Connect cn, ToolTip tip, IWin32Window win, Point position)
        {
            if (cn == LastTipConnect)
            {
                if (Math.Abs(position.X - LastTipConnectPosition.X) > 1 ||
                    Math.Abs(position.Y - LastTipConnectPosition.Y) > 1)
                {
                    LastTipConnectPosition = position;
                    tip.ToolTipTitle = "Connect " + cn.name + " " + ((Port)cn.ports[0]).fullBorders;
                    tip.Show(TipConnectCap, win, position.X + tipDx - (int)parent.parent.ScrollDx, 
                        position.Y + tipDy - (int)parent.parent.ScrollDy);
                }
            }
            else
            {
                LastTipConnect = cn;
                LastTipConnectPosition = position;

                Port pt = (Port)cn.ports[0];
                TipConnectCap = "";
                tip.ToolTipTitle = "Connect " + cn.name + " " + pt.fullBorders;
                string[] nam = { "left\t: ", "top\t: ", "left\t: ", "bottom\t: " };
                for (int i = 0; i < 4; i++)
                {
                    TipConnectCap += nam[i];
                    pt = (Port)cn.ports[i];
                    if (pt.isLine)
                    {
                        TipConnectCap += " --> ";
                        if (pt.lin.parentBegin == pt)
                        {
                            TipConnectCap += pt.lin.parentEnd.fullNameWithParent;
                        }
                        else
                        {
                            TipConnectCap += pt.lin.parentBegin.fullNameWithParent;
                        }
                    }
                    else
                        TipConnectCap += "none";
                    if (i != 3)
                        TipConnectCap += "\n";
                }
                tip.Show(TipConnectCap, win, position.X + tipDx - (int)parent.parent.ScrollDx,
                        position.Y + tipDy - (int)parent.parent.ScrollDy);
            }
        }

        private Point LastTipLinePosition = new Point(-1, -1);
        private Line LastTipLine = null;
        private string TipLineCap = "";

        private void tipLine(Line ln, ToolTip tip, IWin32Window win, Point position)
        {
            if (ln == LastTipLine)
            {
                if (Math.Abs(position.X - LastTipLinePosition.X) > tipD ||
                    Math.Abs(position.Y - LastTipLinePosition.Y) > tipD)
                {
                    LastTipLinePosition = position;
                    if (ln.isSignal)
                        tip.ToolTipTitle = "Signal ";
                    else
                        tip.ToolTipTitle = "Line ";
                    tip.ToolTipTitle = ln.name;
                    tip.Show(TipLineCap, win, position.X + tipDx - (int)parent.parent.ScrollDx, 
                        position.Y + tipDy - (int)parent.parent.ScrollDy);
                }
            }
            else
            {
                LastTipLine = ln;
                LastTipLinePosition = position;

                TipLineCap = "";
                if (ln.isSignal)
                    tip.ToolTipTitle = "Signal ";
                else
                    tip.ToolTipTitle = "Line ";
                tip.ToolTipTitle = ln.name;
                TipLineCap += ln.parentBegin.fullNameWithParent + " <--> " + ln.parentEnd.fullNameWithParent;
                KeyValuePair<int, int> oldValue = new KeyValuePair<int, int>();
                KeyValuePair<int, int> startValue = new KeyValuePair<int, int>();
                bool old = false;
                foreach (KeyValuePair<int, int> ind in ln.assign)
                {
                    if (!old)
                    {
                        startValue = ind;
                        oldValue = ind;
                        old = true;
                    }
                    else
                        if (Math.Abs(oldValue.Value - ind.Value) > 1 || Math.Abs(oldValue.Key - ind.Key) > 1)
                        {
                            TipLineCap += string.Format("\n({0,4};\t{1,4})\t-->\t({2,4};\t{3,4})", startValue.Key, oldValue.Key, startValue.Value, oldValue.Value);
                            startValue = ind;
                            oldValue = ind;
                        }
                        else
                            oldValue = ind;
                }
                if (old)
                    TipLineCap += string.Format("\n({0,4};\t{1,4})\t-->\t({2,4};\t{3,4})", startValue.Key, oldValue.Key, startValue.Value, oldValue.Value);

                tip.Show(TipLineCap, win, position.X + tipDx - (int)parent.parent.ScrollDx,
                        position.Y + tipDy - (int)parent.parent.ScrollDy);
            }             
        }

        private Point LastTipPortPosition = new Point(-1, -1);
        private Port LastTipPort = null;
        private string TipPortCap = "";

        private void tipPort(Port pt, ToolTip tip, IWin32Window win, Point position)
        {
            if (pt == LastTipPort)
            {
                if (Math.Abs(position.X - LastTipPortPosition.X) > tipD ||
                    Math.Abs(position.Y - LastTipPortPosition.Y) > tipD)
                {
                    LastTipPortPosition = position;
                    tip.ToolTipTitle = "Port " + pt.nameWithParent;
                    tip.Show(TipPortCap, win, position.X + tipDx - (int)parent.parent.ScrollDx, 
                        position.Y + tipDy - (int)parent.parent.ScrollDy);
                }
            }
            else
            {
                LastTipPort = pt;
                LastTipPortPosition = position;

                TipPortCap = "";
                tip.ToolTipTitle = "Port " + pt.nameWithParent;
                TipPortCap += "Type\t: ";
                TipPortCap += pt.inout.ToString();
                TipPortCap += " " + pt.fullType + "\n";
                TipPortCap += "Line\t: ";
                if (pt.isLine)
                {
                    TipPortCap += "--> ";
                    if (pt.line.parentBegin == pt)
                        TipPortCap += pt.line.parentEnd.fullNameWithParent;
                    else
                        TipPortCap += pt.line.parentBegin.fullNameWithParent;
                }
                else
                    TipPortCap += "none";
                tip.Show(TipPortCap, win, position.X + tipDx - (int)parent.parent.ScrollDx,
                        position.Y + tipDy - (int)parent.parent.ScrollDy);
            }
        }

        private bool findNearPort(PointF location, ref Port pt) //поиск ближайшего порта (с учетом connectDistance)
        {
            Common el = null;
            int i, j;
            for (i = 0; i < parent.ElementsCount; i++)
            {
                el = parent.ReturnElement(i);
                for (j = 0; j < el.ports.Count; j++)
                {
                    pt = (Port)el.ports[j];
                    if (Math.Abs(el.border.Left + pt.location.X - location.X) + Math.Abs(el.border.Top + pt.location.Y - location.Y) < connectDistanceSQR)
                    {
                        return true;
                    }
                }
            }
            for (i = 0; i < parent.ConnectsCount; i++)
            {
                el = (Connect)parent.ReturnConnect(i);
                for (j = 0; j < el.ports.Count; j++)
                {
                    pt = (Port)el.ports[j];
                    if (Math.Abs(el.border.Left + pt.location.X - location.X) + Math.Abs(el.border.Top + pt.location.Y - location.Y) < connectDistanceSQR)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool findNearLine(PointF loc, ref Line lin, ref int segm) //поиск ближайшей линии(и сегмена в линии) (с учетом connectDistance)
        {
            int i;
            for (i = 0; i < parent.LinesCount; i++)
            {
                lin = parent.ReturnLine(i);
                if (lin.points != null)
                    for (segm = 0; segm < lin.points.Length - 1; segm++)
                        if (lin.points[segm].Y == lin.points[segm + 1].Y)
                        {
                            if (lin.points[segm].Y - connectDistance < loc.Y && lin.points[segm].Y + connectDistance > loc.Y)
                                if ((lin.points[segm].X + connectDistance < loc.X && lin.points[segm + 1].X - connectDistance > loc.X) ||
                                    (lin.points[segm + 1].X + connectDistance < loc.X && lin.points[segm].X - connectDistance > loc.X))
                                    return true;
                        }
                        else
                        {
                            if (lin.points[segm].X - connectDistance < loc.X && lin.points[segm].X + connectDistance > loc.X)
                                if ((lin.points[segm].Y + connectDistance < loc.Y && lin.points[segm + 1].Y - connectDistance > loc.Y) ||
                                    (lin.points[segm + 1].Y + connectDistance < loc.Y && lin.points[segm].Y - connectDistance > loc.Y))
                                    return true;
                        }
            }
            return false;
        }


        private bool findNearElement(PointF location, ref Element el) //поиск ближайшего элемента (с учетом connectDistance)
        {
            int i;
            for (i = 0; i < parent.ElementsCount; i++)
            {
                el = parent.ReturnElement(i);
                if (el.border.Left - connectDistance < location.X && el.border.Top - connectDistance < location.Y &&
                    el.border.Right + connectDistance > location.X && el.border.Bottom + connectDistance > location.Y)
                {
                    return true;
                }
            }        
            return false;
        }

        private bool findNearConnect(PointF location, ref Connect cn) //поиск ближайшего узла (с учетом connectDistance)
        {
            int i;
            for (i = 0; i < parent.ConnectsCount; i++)
            {
                cn = parent.ReturnConnect(i);
                if (cn.border.Right > location.X && cn.border.Bottom > location.Y &&
                    cn.border.Left - cn.border.Width < location.X && cn.border.Top - cn.border.Height < location.Y)
                {
                    return true;
                }
            }
            return false;
        }

        public void onMouseMove(MouseEventArgs e)
        {
            mouseLocation = e.Location;
            //int min, imin = 0, i2min = 0;
            //min = 9999;
            //for (int i = 0; i < hcnt; i++)
            //    if (Math.Abs(e.LineNumber - horp[i]) < min)
            //    {
            //        min = Math.Abs(e.LineNumber - horp[i]);
            //        imin = i;
            //    }
            //min = 9999;
            //for (int i = 0; i < vcnt; i++)
            //    if (Math.Abs(e.ColumnNumber - verp[i]) < min)
            //    {
            //        min = Math.Abs(e.ColumnNumber - verp[i]);
            //        i2min = i;
            //    }
            //if (mat.Length > 0)
            //    parent.parent.setStatus(mat[imin, i2min].ToString());
            //string cap = "";
            moved = true;
            Element el = null;
            Connect cn = null;
            Line ln = null;
            Port pt = null;
            bool find = false;
            int buf = 0;
            if (!mouseDown)
            { 
                //find near port
                find = findNearPort(e.Location, ref pt);
                bool flag = false;
                if (find)
                {
                    if (pt.isLine)
                        if (pt.line.isSignal)
                            flag = true;
                    nearPort = pt;
                    isNearPort = true;
                    drawConnect = true;
                    connectLocation = new PointF(pt.parent.border.Location.X + pt.location.X, 
                        pt.parent.border.Location.Y + pt.location.Y);
                }
                if (!find || flag)
                {
                    if (drawConnect)
                    {
                        drawConnect = false;
                    }
                    isNearPort = false;
                }
               // else
                    //if (!drawConnect || !isNearPort || nearPort != pt)
                    {
                        
                    }

                //find near line
                find = findNearLine(e.Location, ref ln, ref buf);
                if (!find)
                {
                    isNearLine = false;
                }
                else
                {
                    selectedSegment = buf;
                    isVertSegment = (ln.points[buf].X == ln.points[buf + 1].X);
                    isNearLine = true;
                    nearLine = ln;
                }

                //find near connect
                find = findNearConnect(e.Location, ref cn);
                if (!find)
                {
                    isNearConnect = false;
                }
                else
                {
                    drawConnect = false;
                    isNearConnect = true;
                    nearConnect = cn;                    
                    //parent.parent.Invalidate();
                    //ExecuteTip();
                    //return;
                }

                //find near element
                find = findNearElement(e.Location, ref el);
                if (!find)
                {
                    isNearElement = false;
                }
                else
                {
                    isNearElement = true;
                    nearElement = el;
                }

                if (isNearConnect || isNearElement || isNearLine || isNearPort)
                    parent.parent.popupTimer.Start();
                else
                {
                    parent.parent.popupTimer.Stop();
                    parent.parent.tip.Hide(parent.parent);
                }
            }
            else
            {
                if (selectPort)
                {
                    PointF[] pts = new PointF[4];
                    linePointF2 = e.Location;
                    find = findNearPort(e.Location, ref pt);                                          
                    if (find && pt != selectedPort && !pt.isLine)
                    {
                        isNearLine = false;
                        drawConnect = true;
                        connectLocation = new PointF(pt.location.X + pt.parent.border.X, pt.location.Y + pt.parent.border.Y);
                        nearPort = pt;
                        isNearPort = true;
                    }
                    else
                    {                        
                        find = findNearLine(e.Location, ref ln, ref buf);
                        if ((isNearPort && nearPort != selectedPort) || (!find && isNearLine))
                        {
                            drawConnect = true;
                            connectLocation = new PointF(selectedPort.location.X + selectedPort.parent.border.X, selectedPort.location.Y + selectedPort.parent.border.Y);
                            isNearLine = false;
                            isNearPort = false;
                        }
                        if (find)
                        {
                            isNearPort = false;
                            if (drawConnect)
                            {
                                drawConnect = false;
                                parent.parent.Invalidate();
                            }
                            isNearLine = true;
                            nearLine = ln;
                            drawConnect = true;
                            selectedSegment = buf;
                            if  (ln.points[buf].X == ln.points[buf + 1].X)
                            {
                                connectLocation = new PointF(ln.points[buf].X, e.Y);
                            }
                            else
                                connectLocation = new PointF(e.X, ln.points[buf].Y);
                        }
                        else
                            if (findNearConnect(e.Location, ref cn))
                            {
                                find = false;
                                if (!cn.ports[0].isLine)
                                {
                                    find = true;
                                    pt = cn.ports[0];
                                }
                                if (!cn.ports[1].isLine)
                                {
                                    find = true;
                                    pt = cn.ports[1];
                                }
                                if (!cn.ports[2].isLine)
                                {
                                    find = true;
                                    pt = cn.ports[2];
                                }
                                if (!cn.ports[3].isLine)
                                {
                                    find = true;
                                    pt = cn.ports[3];
                                }
                                if (find && pt != selectedPort)
                                {
                                    isNearLine = false;
                                    drawConnect = true;
                                    connectLocation = new PointF(pt.location.X + pt.parent.border.X, pt.location.Y + pt.parent.border.Y);
                                    nearPort = pt;
                                    isNearPort = true;
                                }
                            }  
                    }
                    if (isNearPort)
                    {
                        status = selectedPort.parent.name + ":" + selectedPort.name + " --> " +
                            "(" + nearPort.parent.name + ":" + nearPort.name + ")";
                    }
                    else
                        if (isNearLine)
                        {
                            status = selectedPort.parent.name + ":" + selectedPort.name + " --> " +
                                "(" + nearLine.parentBegin.parent.name + ":" + nearLine.parentBegin.name + " <--> " +
                                nearLine.parentEnd.parent.name + ":" + nearLine.parentEnd.name + ")";
                        }
                        else
                            status = selectedPort.parent.name + ":" + selectedPort.name + " --> ";
                    parent.parent.Invalidate();
                    return;
                }

                if (selectLine)
                {
                    PointF p = new PointF(e.X, e.Y);
                    float d = 0;
                    selectedLine.changeInvisible(false);
                    //selectedLine.invisible = false;                  
                    if (isVertSegment)
                    {
                        float d1 = Math.Abs(selectedLine.points[selectedSegment - 1].X - e.X);
                        float d2 = Math.Abs(e.X - selectedLine.points[selectedSegment + 2].X);
                        if (Math.Min(d1, d2) <= connectDistance)
                            if (d1 <= d2)
                                p.X = selectedLine.points[selectedSegment - 1].X;
                            else
                                p.X = selectedLine.points[selectedSegment + 2].X;                        
                        d = p.X - selectedLine.points[selectedSegment].X;
                        selectedLine.points[selectedSegment].X = p.X;
                        selectedLine.points[selectedSegment + 1].X = p.X;                        
                    }
                    else
                    {
                        float d1 = Math.Abs(selectedLine.points[selectedSegment - 1].Y - e.Y);
                        float d2 = Math.Abs(e.Y - selectedLine.points[selectedSegment + 2].Y);
                        if (Math.Min(d1, d2) <= connectDistance)
                            if (d1 <= d2)
                                p.Y = selectedLine.points[selectedSegment - 1].Y;
                            else
                                p.Y = selectedLine.points[selectedSegment + 2].Y;                           
                        d = p.Y - selectedLine.points[selectedSegment].Y;
                        selectedLine.points[selectedSegment].Y = p.Y;
                        selectedLine.points[selectedSegment + 1].Y = p.Y;                        
                    }
                    if (e.Button == MouseButtons.Left)
                        parent.parent.Invalidate();
                    return;
                }

                if (selectElement)
                {
                    float dx = e.X - mouseDownLocation.X;
                    float dy = e.Y - mouseDownLocation.Y;
                    mouseDownLocation = e.Location; 
                    if (isSelectedElementInBuffer)
                    {
                        parent.buffer.MoveSelected(parent, dx, dy);
                    }
                    else
                    {
                        parent.MoveElement(selectedElement, dx, dy);
                    }                                       
                    parent.parent.Invalidate();
                    return;
                }

                if (selectConnect)
                {
                    float dx = e.X - mouseDownLocation.X;
                    float dy = e.Y - mouseDownLocation.Y;
                    mouseDownLocation = e.Location;
                    parent.MoveConnect(selectedConnect, dx, dy);
                    parent.parent.Invalidate();
                    return;
                }

                if (isSelectingRect)
                {
                    bool inOldRect;
                    bool inNewRect;
                    Rectangle rct = new Rectangle(selectingRect.X < selectingRect.Right ? selectingRect.X : selectingRect.Right,
                        selectingRect.Y < selectingRect.Bottom ? selectingRect.Y : selectingRect.Bottom,
                        Math.Abs(selectingRect.Width), Math.Abs(selectingRect.Height));
                    Rectangle nrct = new Rectangle(selectingRect.X < e.X ? selectingRect.X : e.X,
                        selectingRect.Y < e.Y ? selectingRect.Y : e.Y,
                        Math.Abs(e.X - selectingRect.X), Math.Abs(e.Y - selectingRect.Y));
                    for (int i = 0; i < parent.ElementsCount; i++)
                    {
                        el = (Element)parent.ReturnElement(i);
                        inOldRect = (rct.X > el.border.X && rct.X < el.border.Right && rct.Y < el.border.Bottom && rct.Bottom > el.border.Y) ||
                            (rct.Right > el.border.X && rct.Right < el.border.Right && rct.Y < el.border.Bottom && rct.Bottom > el.border.Y) ||
                            (rct.Y > el.border.Y && rct.Y < el.border.Bottom && rct.X < el.border.Right && rct.Right > el.border.X) ||
                            (rct.Bottom > el.border.Y && rct.Bottom < el.border.Bottom && rct.X < el.border.Right && rct.Right > el.border.X)||
                            (rct.X < el.border.X && rct.Right > el.border.X && rct.Y < el.border.Y && rct.Bottom > el.border.Y);
                        inNewRect = (nrct.X > el.border.X && nrct.X < el.border.Right && nrct.Y < el.border.Bottom && nrct.Bottom > el.border.Y) ||
                            (nrct.Right > el.border.X && nrct.Right < el.border.Right && nrct.Y < el.border.Bottom && nrct.Bottom > el.border.Y) ||
                            (nrct.Y > el.border.Y && nrct.Y < el.border.Bottom && nrct.X < el.border.Right && nrct.Right > el.border.X) ||
                            (nrct.Bottom > el.border.Y && nrct.Bottom < el.border.Bottom && nrct.X < el.border.Right && nrct.Right > el.border.X)||
                            (nrct.X < el.border.X && nrct.Right > el.border.X && nrct.Y < el.border.Y && nrct.Bottom > el.border.Y);
                        if ((inOldRect && !inNewRect) || (!inOldRect && inNewRect))
                        {
                            if (parent.buffer.Find(el))
                                parent.buffer.Clear(el);
                            else
                                parent.buffer.Add(el);
                        }
                    }
                    selectingRect.Width = e.X - selectingRect.X;
                    selectingRect.Height = e.Y - selectingRect.Y;
                }
            }
            
            //if (isNearLine)
            //{
            //    cap = nearLine.parentBegin.name;
            //    if (nearLine.parentBegin.bus)
            //        cap += "(" + nearLine.parentBegin.LeftBusBound + "; " + nearLine.parentBegin.RightBusBound + ")";
            //    cap += " --> " + nearLine.parentEnd.name;
            //    if (nearLine.parentEnd.bus)
            //        cap += "(" + nearLine.parentEnd.LeftBusBound + "; " + nearLine.parentEnd.RightBusBound + ")";
            //}
            //else
               

            //if (cap != "")
            //    parent.parent.tip.Show(cap, parent.parent, e.LineNumber, e.ColumnNumber);
            //else
            //    parent.parent.tip.Hide(parent.parent);

            parent.parent.Invalidate();
        }

        public void onMouseDown(MouseEventArgs e)
        {
            tipHide(parent.parent.tip, parent.parent);
            //if (e.Button == (MouseButtons.Right) && parent.buffer.Count > 0)
            //{
            //    parent.buffer.CheckElementListChanged();
            //    parent.buffer.Paste(e.Location, parent);
            //    parent.parent.Invalidate();
            //    return;
            //}
            if (e.Button != MouseButtons.Left)
                return;
            moved = false;
            mouseDown = true;
            if (isNearConnect)
            {
                selectedConnect = nearConnect;
                selectConnect = true;
                mouseDownLocation = e.Location;
                for (int i = 0; i < selectedConnect.ports.Count; i++)
                    if (((Port)selectedConnect.ports[i]).isLine)
                        ((Port)selectedConnect.ports[i]).line.setToChange();
                parent.setInvisible();
                parent.parent.Invalidate();
                return;
            }

            if (isNearPort)
            {
                if (nearPort.isLine)
                {
                    parent.RemoveLineWithoutCheckConnect(nearPort.line);
                    selectLine = true;                    
                    selectedLine = nearPort.line;
                    isNearLine = false;
                    nearPort.isLine = false;
                    //parent.RemoveLine(nearPort.line);
                    if (nearPort == nearPort.line.parentBegin)
                        selectedPort = nearPort.line.parentEnd;
                    else
                        selectedPort = nearPort.line.parentBegin;
                    selectedPort.isLine = false;
                    parent.parent.Invalidate();
                    connectLocation = new PointF(nearPort.location.X + nearPort.parent.border.X, nearPort.location.Y + nearPort.parent.border.Y);
                }
                else
                {
                    selectedPort = nearPort;
                    connectLocation = new PointF(selectedPort.location.X + selectedPort.parent.border.X, selectedPort.location.Y + selectedPort.parent.border.Y);
                    isNearLine = false;
                    isNearPort = false;
                }
                selectPort = true;
                drawConnect = true;
                mouseDownLocation = e.Location;
                drawLine = true;
                linePointF1 = new PointF(selectedPort.location.X + selectedPort.parent.border.X,
                    selectedPort.location.Y + selectedPort.parent.border.Y);
                linePointF2 = e.Location;
                parent.parent.Invalidate();
                return;
            }
           
            if (isNearLine)
            {                
                selectedLine = nearLine;
                selectLine = true;
                parent.RemoveLineMark(selectedLine);
                if (selectedSegment == 0)
                {
                    PointF[] newp = selectedLine.points;
                    selectedLine.points = new PointF[newp.Length + 1];
                    selectedLine.points[0] = newp[0];
                    for (int i = 0; i < newp.Length; i++)
                        selectedLine.points[i + 1] = newp[i];
                    selectedSegment = 1;
                }
                if (selectedSegment == selectedLine.points.Length - 2)
                {
                    PointF[] newp = selectedLine.points;
                    selectedLine.points = new PointF[newp.Length + 1];
                    for (int i = 0; i < newp.Length; i++)
                        selectedLine.points[i] = newp[i];
                    selectedLine.points[newp.Length] = newp[newp.Length - 1];
                }
                if (isVertSegment)
                {
                    selectedLine.points[selectedSegment].X = e.X;
                    selectedLine.points[selectedSegment + 1].X = e.X;
                    parent.parent.Cursor = Cursors.SizeWE;
                }
                else
                {
                    selectedLine.points[selectedSegment].Y = e.Y;
                    selectedLine.points[selectedSegment + 1].Y = e.Y;
                    parent.parent.Cursor = Cursors.SizeNS;
                }                
                parent.parent.Invalidate(); 
                return;
            }
                
            if (isNearElement)
            {
                if (control)
                {
                    if (!parent.buffer.Find((Element)nearElement))
                        parent.buffer.Add((Element)nearElement);
                    else
                        parent.buffer.Clear((Element)nearElement);
                }
                /*else
                {
                    if (parent.buffer.Count == 1)
                    {
                        parent.buffer.ClearAll();
                        parent.buffer.Add((Element)nearElement);
                    }
                    else
                    {
                        //parent.buffer.MouseDown(parent);
                    }
                }*/
                mouseDownLocation = e.Location;
                selectedElement = nearElement;
                selectElement = true;
                isSelectedElementInBuffer = parent.buffer.Find(selectedElement);
                if (isSelectedElementInBuffer)
                    parent.buffer.MouseDown(parent);
                else
                {
                    parent.RemoveElementMark(selectedElement);
                    for (int i = 0; i < selectedElement.ports.Count; i++)
                        if (((Port)selectedElement.ports[i]).isLine)
                        {
                            ((Port)selectedElement.ports[i]).line.setToChange();
                        }
                }
                parent.setInvisible();                
                parent.parent.Invalidate();
                return;
            }
            if (!control)
            {
                parent.buffer.ClearAll();
            }
            selectingRect = new Rectangle(e.X, e.Y, 0, 0);
            isSelectingRect = true;
            parent.parent.Invalidate(); 
        }

        public void onMouseUp(MouseEventArgs e)
        {
            mouseDown = false;

            if (isSelectingRect)
            {
                isSelectingRect = false;
                return;
            }

            if (selectPort)
            {
                drawLine = false;
                selectPort = false;
                drawConnect = false;   
                //invalidateConnection();
                if (isNearPort)
                    if (!nearPort.isLine)
                    {
                        Line l = new Line(selectedPort, nearPort);
                        if (selectedPort.isLine)
                        {
                            parent.RemoveLine(selectedPort.line);
                            selectedPort.isLine = false;
                        }
                        if (nearPort.isLine)
                        {
                            parent.RemoveLine(nearPort.line);
                            nearPort.isLine = false;
                        }                        
                        selectedPort.line = l;
                        nearPort.line = l;
                        nearPort.parent.parent.AddLine(l);
                        l.PassFinding();
                        parent.AddLineMark(l);

                        parent.parent.tip.Hide(parent.parent);

                        cnManager cnM = new cnManager(selectedPort, nearPort, parent.parent);
                        if (cnM.resultOk)
                        {
                            l.assign = cnM.assign;
                            if (l.assign.Count > 1)
                            {
                                l.bus = true;
                                l.LeftBusBound = 0;
                                l.RightBusBound = l.assign.Count - 1;
                            }
                            else
                                l.bus = false;
                        }
                        else
                            parent.RemoveLine(l);
                    }
                                
                if (isNearLine)
                {
                    Connect cn = new Connect(connectLocation, this.parent);
                    this.parent.AddConnect(cn);
                    Port pt0 = (Port)cn.ports[0];
                    Port pt1 = (Port)cn.ports[1];
                    Port pt2 = (Port)cn.ports[2];
                    Port pt3 = (Port)cn.ports[3];
                    Line ln1 = null;
                    Line ln2 = null;
                    Line ln3 = null;
                    this.parent.RemoveLineWithoutCheckConnect(nearLine);
                    pt0.bus = pt1.bus = pt2.bus = pt3.bus = nearLine.bus;
                    pt0.LeftBusBound = pt1.LeftBusBound = pt2.LeftBusBound = pt3.LeftBusBound = nearLine.LeftBusBound;
                    pt0.RightBusBound = pt1.RightBusBound = pt2.RightBusBound = pt3.RightBusBound = nearLine.RightBusBound;
                    //this.parent.RemoveLineMark(nearLine);
                    if (nearLine.points[selectedSegment].X == nearLine.points[selectedSegment + 1].X)
                    {
                        if (nearLine.begin.Y < nearLine.end.Y)
                        {
                            ln1 = new Line(nearLine.parentBegin, pt1, nearLine.name);
                            //nearLine.parentEnd = pt1;
                            ln2 = new Line(nearLine.parentEnd, pt3, nearLine.name);
                            pt1.line = ln1;
                            //pt1.line = nearLine;
                            pt3.line = ln2;
                        }
                        else
                        {
                            ln1 = new Line(nearLine.parentBegin, pt3, nearLine.name);
                            //nearLine.parentEnd = pt3;
                            ln2 = new Line(nearLine.parentEnd, pt1, nearLine.name);
                            pt3.line = ln1;
                            //pt3.line = nearLine;
                            pt1.line = ln2;
                        }
                        if (selectedPort.location.X + selectedPort.parent.border.X < connectLocation.X)
                        {
                            ln3 = new Line(selectedPort, pt2, nearLine.name);
                            pt2.line = ln3;
                        }
                        else
                        {
                            ln3 = new Line(selectedPort, pt0, nearLine.name);
                            pt0.line = ln3;
                        }  
                    }
                    else
                    {
                        if (nearLine.begin.X < nearLine.end.X)
                        {
                            ln1 = new Line(nearLine.parentBegin, pt2, nearLine.name);
                            //nearLine.parentEnd = pt2;
                            ln2 = new Line(nearLine.parentEnd, pt0, nearLine.name);
                            pt2.line = ln1;
                            //pt2.line = nearLine;
                            pt0.line = ln2;
                        }
                        else
                        {
                            ln1 = new Line(nearLine.parentBegin, pt0, nearLine.name);
                            //nearLine.parentEnd = pt0;
                            ln2 = new Line(nearLine.parentEnd, pt2, nearLine.name);
                            pt0.line = ln1;
                            //pt0.line = nearLine;
                            pt2.line = ln2;
                        }
                        if (selectedPort.location.Y + selectedPort.parent.border.Y < connectLocation.Y)
                        {
                            ln3 = new Line(selectedPort, pt1, nearLine.name);
                            pt1.line = ln3;
                        }
                        else
                        {
                            ln3 = new Line(selectedPort, pt3, nearLine.name);
                            pt3.line = ln3;                           
                        }                        
                    }
                    ln1.bus = ln2.bus = nearLine.bus;
                    ln1.LeftBusBound = ln2.LeftBusBound = nearLine.LeftBusBound;
                    ln1.RightBusBound = ln2.RightBusBound = nearLine.RightBusBound;
                    ln1.isSignal = ln2.isSignal = nearLine.isSignal;
                    int i = 0;
                    foreach (KeyValuePair<int, int> ind in nearLine.assign)
                    {
                        ln1.assign.Add(ind.Key, i);
                        ln2.assign.Add(ind.Value, i++);
                    }
                    nearLine.parentBegin.line = ln1;
                    nearLine.parentEnd.line = ln2;
                    selectedPort.line = ln3;
                    ln1.PassFinding();
                    this.parent.AddLine(ln1);
                    //nearLine.PassFinding();
                    //parent.AddLineMark(nearLine);
                    ln2.PassFinding();
                    this.parent.AddLine(ln2);
                    ln3.PassFinding();
                    this.parent.AddLine(ln3);
                    cnManager cnM = new cnManager(selectedPort, nearLine, parent.parent);
                    if (cnM.resultOk)
                    {
                        ln3.assign = cnM.assign;
                        if (ln3.assign.Count > 1)
                        {
                            ln3.bus = true;
                            ln3.LeftBusBound = 0;
                            ln3.RightBusBound = ln3.assign.Count - 1;
                        }
                        else
                        {
                            ln3.bus = false;
                        }
                    }
                    else
                    {
                        parent.RemoveLine(ln3);
                    }
                }
                if (selectLine)
                {
                    if (!selectedLine.parentBegin.parent.isElement)
                    {
                        parent.CheckConnect((Connect)selectedLine.parentBegin.parent);
                    }
                    if (!selectedLine.parentEnd.parent.isElement)
                    {
                        parent.CheckConnect((Connect)selectedLine.parentEnd.parent);
                    }
                    selectLine = false;
                }
                if (!isNearPort && !isNearLine && !selectedPort.parent.isElement)
                {
                    parent.CheckConnect((Connect)selectedPort.parent);
                }
                if (isNearPort || isNearLine)
                {

                }
                isNearPort = false;
                isNearLine = false;
                //parent.checkConnects();
                parent.parent.Invalidate();
                return;
            }

            if (selectLine)
            {
                selectLine = false;
                parent.parent.Cursor = Cursors.Default;
                selectedLine.simpling(connectDistance);
                parent.AddLineMark(selectedLine);
                if (moved)
                {
                    parent.parent.history.Changed();
                }
                parent.parent.Invalidate();                
                return;
            }                

            if (selectElement)
            {
                if (!moved && !control)
                {
                    parent.buffer.ClearAll();
                    parent.buffer.Add((Element)selectedElement);
                }
                if (isSelectedElementInBuffer)
                {
                    //обработка перемещения выделенных обьектов
                    parent.buffer.MouseUp(parent);
                }
                else
                {
                    //добавления меток перемещаемого обьекта, не находящегося в буфере
                    parent.AddElementMark(selectedElement);
                    Line ln;
                    for (int i = 0; i < parent.LinesCount; i++)
                    {
                        ln = parent.ReturnLine(i);
                        if (ln.invisible)
                        {
                            ln.setToChange();
                        }
                    }
                }
                selectElement = false;
                parent.removeInvisible();
                if (moved)
                {
                    parent.parent.history.Changed();
                }
                parent.parent.Invalidate();
                return;
            }

            if (selectConnect)
            {
                selectConnect = false;
                Line ln;
                for (int i = 0; i < parent.LinesCount; i++)
                {
                    ln = parent.ReturnLine(i);
                    if (ln.invisible)
                    {
                        ln.setToChange();
                    }
                }
                parent.removeInvisible();
                if (moved)
                {
                    parent.parent.history.Changed();
                }
                parent.parent.Invalidate();
                return;
            }            
            parent.parent.Invalidate();
        }

        public RectangleF onDraw(Graphics e)
        {
            RectangleF rect;
            if (isNearElement)
            {
               nearElement.DrawSelect(e);
            }
            if (isNearConnect)
            {
                nearConnect.DrawSelect(e);
            }
            parent.buffer.DrawSelected(e);            

            //int i, j;
            //for (i = 0; i < hcnt; i++)
            //    e.DrawLine(Pens.Crimson, horp[i], 0, horp[i], parent.parent.Height);
            //for (i = 0; i < vcnt; i++)
            //    e.DrawLine(Pens.Crimson, 0, verp[i], parent.parent.Width, verp[i]);
            //for (i = 0; i < hcnt; i++)
            //    for (j = 0; j < vcnt; j++)
            //        e.DrawString(mat[i, j].ToString(), new Font(FontFamily.GenericMonospace, 8), Brushes.Crimson, horp[i], verp[j]);

            rect = parent.Draw(e);
            if (isNearLine)
            {
                if (nearLine.bus)
                    e.DrawLines(new Pen(Color.Red, 3), nearLine.points);
                else
                    e.DrawLines(new Pen(Color.Red, 1), nearLine.points);
            }
            if (drawConnect)
            {
                RectangleF rt = new RectangleF(connectLocation.X - connectDistance, connectLocation.Y - connectDistance, 2 * connectDistance, 2 * connectDistance);
                e.FillEllipse(Brushes.Red, rt);
            }
            if (drawLine)
            {
                e.DrawLine(new Pen(Color.Red, 1), linePointF1, linePointF2);
            }
            if (isSelectingRect)
            {
                Pen ff = new Pen(Color.Blue);
                ff.DashStyle = DashStyle.DashDot;
                ff.DashCap = DashCap.Round;
                Rectangle rct = new Rectangle(selectingRect.X < selectingRect.Right ? selectingRect.X : selectingRect.Right,
                    selectingRect.Y < selectingRect.Bottom ? selectingRect.Y : selectingRect.Bottom,
                    Math.Abs(selectingRect.Width), Math.Abs(selectingRect.Height));

                Pen br = new Pen(Color.FromArgb(120, Color.LightGreen));
                e.FillRectangle(br.Brush, rct);
                e.DrawRectangle(ff, rct);
                
            }
            return rect;
        }

        public void onKeyDown(KeyEventArgs e)
        {
            control = e.Control;
            if (e.KeyCode == Keys.Space)
            {
                parent.parent.Cursor = Cursors.Default; 
                PointF loc = new PointF();
                if (isVertSegment)
                {
                    loc.X = selectedLine.points[selectedSegment].X;
                    loc.Y = mouseLocation.Y;
                }
                else
                {
                    loc.X = mouseLocation.X;
                    loc.Y = selectedLine.points[selectedSegment].Y;
                }                

                Connect cn = new Connect(loc, this.parent);
                this.parent.AddConnect(cn);
                Port pt0 = cn.ports[0];
                Port pt1 = cn.ports[1];
                Port pt2 = cn.ports[2];
                Port pt3 = cn.ports[3];
                Line ln1 = null;
                Line ln2 = null;
                selectLine = false;
                isNearLine = false;
                this.parent.RemoveLineWithoutCheckConnect(selectedLine);
                pt0.bus = pt1.bus = pt2.bus = pt3.bus = selectedLine.bus;
                pt0.LeftBusBound = pt1.LeftBusBound = pt2.LeftBusBound = pt3.LeftBusBound = selectedLine.LeftBusBound;
                pt0.RightBusBound = pt1.RightBusBound = pt2.RightBusBound = pt3.RightBusBound = selectedLine.RightBusBound;
                if (isVertSegment)
                {
                    if (nearLine.begin.Y < nearLine.end.Y)
                    {
                        ln1 = new Line(selectedLine.parentBegin, pt1, selectedLine.name);
                        //nearLine.parentEnd = pt1;
                        ln2 = new Line(selectedLine.parentEnd, pt3, selectedLine.name);
                        pt1.line = ln1;
                        //pt1.line = nearLine;
                        pt3.line = ln2;
                    }
                    else
                    {
                        ln1 = new Line(selectedLine.parentBegin, pt3, selectedLine.name);
                        //nearLine.parentEnd = pt3;
                        ln2 = new Line(selectedLine.parentEnd, pt1, selectedLine.name);
                        pt3.line = ln1;
                        //pt3.line = nearLine;
                        pt1.line = ln2;
                    }
                    if (mouseLocation.X < selectedLine.points[selectedSegment].X)
                    {
                        selectedPort = pt2;
                    }
                    else
                    {
                        selectedPort = pt0;
                    }
                }
                else
                {
                    if (nearLine.begin.X < nearLine.end.X)
                    {
                        ln1 = new Line(selectedLine.parentBegin, pt2, selectedLine.name);
                        //nearLine.parentEnd = pt2;
                        ln2 = new Line(selectedLine.parentEnd, pt0, selectedLine.name);
                        pt2.line = ln1;
                        //pt2.line = nearLine;
                        pt0.line = ln2;
                    }
                    else
                    {
                        ln1 = new Line(selectedLine.parentBegin, pt0, selectedLine.name);
                        //nearLine.parentEnd = pt0;
                        ln2 = new Line(selectedLine.parentEnd, pt2, selectedLine.name);
                        pt0.line = ln1;
                        //pt0.line = nearLine;
                        pt2.line = ln2;
                    }
                    if (mouseLocation.Y < selectedLine.points[selectedSegment].Y)
                    {
                        selectedPort = pt3;
                    }
                    else
                    {
                        selectedPort = pt1;
                    }
                }

                selectPort = true;
                drawConnect = true;
                mouseDownLocation = mouseLocation;
                drawLine = true;
                linePointF1 = new PointF(selectedPort.location.X + selectedPort.parent.border.X,
                    selectedPort.location.Y + selectedPort.parent.border.Y);
                linePointF2 = mouseLocation;
                connectLocation = new PointF(selectedPort.location.X + selectedPort.parent.border.X, selectedPort.location.Y + selectedPort.parent.border.Y);
                ln1.bus = ln2.bus = selectedLine.bus;
                ln1.LeftBusBound = ln2.LeftBusBound = selectedLine.LeftBusBound;
                ln1.RightBusBound = ln2.RightBusBound = selectedLine.RightBusBound;
                ln1.isSignal = ln2.isSignal = selectedLine.isSignal;
                foreach (KeyValuePair<int, int> ind in selectedLine.assign)
                {
                    ln1.assign.Add(ind.Key, ind.Key);
                    ln2.assign.Add(ind.Value, ind.Value);
                }
                selectedLine.parentBegin.line = ln1;
                selectedLine.parentEnd.line = ln2;
                //selectedPort.line = ln3;
                ln1.PassFinding();
                this.parent.AddLine(ln1);
                //nearLine.PassFinding();
                //parent.AddLineMark(nearLine);
                ln2.PassFinding();
                this.parent.AddLine(ln2);
                this.parent.parent.Invalidate();
            }
        }

        public void onKeyUp(KeyEventArgs e)
        {
            control = e.Control;
        }

        public void DeleteNearLine()
        {
            if (isNearLine)
            {
                parent.RemoveLine(nearLine);
                isNearLine = false;
                nearLine.parentBegin.parent.parent.parent.history.Changed();
            }
        }
    }
}