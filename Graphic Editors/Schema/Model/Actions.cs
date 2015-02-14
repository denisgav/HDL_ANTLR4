using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Drawing;

namespace csx
{
    public class Actions //инкапсулирует действи€ с выделенными элементами
    {
        private static Actions buffer;
        public static Actions Buffer
        {
            get { return buffer; }
            set { buffer = value; }
        }

        static Actions()
        {
            buffer = new Actions();
        }
        


        private bool isElementListChanged = false;
        private List<Element> selected = new List<Element>(); //массив выделенных элементов
        private List<Connect> sconnect = new List<Connect>(); //массив выделенных узлов
        private List<Line> sline = new List<Line>(); //массив св€занных с выделенными элементами линий
        private List<Port> sport = new List<Port>(); //массив св€занных с выделенными элементами портов
        public int Count //количество выделенных элементов 
        {
            get
            {
                return selected.Count;
            }
        }

        public Element getSelected(int i)
        {
            return (Element)selected[i];
        }

        private void choiseConnect()
        {
            ArrayList oneMark = new ArrayList();
            int i, j, q;
            Element el;
            Connect cn;
            Port pt1, pt2;
            Stack<Connect> buffer = new Stack<Connect>();
            sconnect.Clear();
            for (i = 0; i < selected.Count; i++)
            {
                el = (Element)selected[i];
                for (j = 0; j < el.ports.Count; j++)
                {
                    pt1 = (Port)el.ports[j];
                    if (pt1.isLine)
                    {
                        if (pt1.line.parentBegin == pt1)
                            pt2 = pt1.line.parentEnd;
                        else
                            pt2 = pt1.line.parentBegin;
                        if (!pt2.parent.isElement)
                        {
                            if (!oneMark.Contains(pt2.parent))
                                oneMark.Add(pt2.parent);
                            else
                            {
                                if (!sconnect.Contains((Connect)pt2.parent))
                                {
                                    sconnect.Add((Connect)pt2.parent);
                                    buffer.Push((Connect)pt2.parent);
                                    //sport.Add(pt2.parent.ports[0]);
                                    //sport.Add(pt2.parent.ports[1]);
                                    //sport.Add(pt2.parent.ports[2]);
                                    //sport.Add(pt2.parent.ports[3]);
                                }
                                while (buffer.Count > 0)
                                {
                                    cn = buffer.Pop();
                                    for (q = 0; q < 4; q++)
                                    {
                                        pt1 = (Port)cn.ports[q];
                                        if (pt1.isLine)
                                        {
                                            if (pt1.line.parentBegin == pt1)
                                                pt2 = pt1.line.parentEnd;
                                            else
                                                pt2 = pt1.line.parentBegin;
                                            if (!pt2.parent.isElement)
                                            {
                                                if (!oneMark.Contains(pt2.parent))
                                                    oneMark.Add(pt2.parent);
                                                else
                                                {
                                                    if (!sconnect.Contains((Connect)pt2.parent))
                                                    {
                                                        sconnect.Add((Connect)pt2.parent);
                                                        buffer.Push((Connect)pt2.parent);
                                                        //sport.Add(pt2.parent.ports[0]);
                                                        //sport.Add(pt2.parent.ports[1]);
                                                        //sport.Add(pt2.parent.ports[2]);
                                                        //sport.Add(pt2.parent.ports[3]);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            /*for (i = 0; i < oneMark.Count; i++)
            {
                cn = (Connect)oneMark[i];
                for (j = 0; j < 4; j++)
                {
                    pt1 = (Port)cn.ports[j];
                    if (pt1.isLine)
                    {
                        if (pt1.line.parentBegin == pt1)
                            pt2 = pt1.line.parentEnd;
                        else
                            pt2 = pt1.line.parentBegin;
                        if (!pt2.parent.isElement)
                        {
                            if (!oneMark.Contains(pt2.parent))
                                oneMark.Add(pt2.parent);
                            else
                            {
                                if (!sconnect.Contains((Connect)pt2.parent))
                                {
                                    sconnect.Add((Connect)pt2.parent);
                                    //sport.Add(pt2.parent.ports[0]);
                                    //sport.Add(pt2.parent.ports[1]);
                                    //sport.Add(pt2.parent.ports[2]);
                                    //sport.Add(pt2.parent.ports[3]);
                                }
                            }
                        }
                    }
                }
            }*/
        }

        private void choiseLine() //добавление линий, св€занных с выделенными узлами и компонентами
        {
            int i;
            sline.Clear();
            Element el;
            Line ln;
            bool f1 = false, f2 = false;
            if (selected.Count > 0)
            {
                el = (Element)selected[0];
                for (i = 0; i < el.parent.LinesCount; i++)
                {
                    ln = el.parent.ReturnLine(i);
                    if (ln.parentBegin.parent.isElement)
                        f1 = selected.Contains((Element)ln.parentBegin.parent);
                    else
                        f1 = sconnect.Contains((Connect)ln.parentBegin.parent);
                    if (ln.parentEnd.parent.isElement)
                        f2 = selected.Contains((Element)ln.parentEnd.parent);
                    else
                        f2 = sconnect.Contains((Connect)ln.parentEnd.parent);
                    if (f1 && f2)
                        if (!sline.Contains(ln))
                            sline.Add(ln);
                }
            }
        }

        private void choisePort() //добавление портов, св€занных с выделенными узлами и компонентами
        {
            int i, j;
            sport.Clear();
            Element el;
            Connect cn;
            for (i = 0; i < selected.Count; i++)
            {
                el = (Element)selected[i];
                for (j = 0; j < el.ports.Count; j++)
                {
                    sport.Add(el.ports[j]);
                    //pt = (Port)el.ports[j];
                    //pt2 = new Port(pt.type, pt.width, pt.location, pt.parent, pt.napr, pt.name);
                    //pt2.line = (Line)sline[sline.IndexOf(pt.line)];
                    //sport.Add(pt2);
                }
            }
            for (i = 0; i < sconnect.Count; i++)
            {
                cn = (Connect)sconnect[i];
                for (j = 0; j < 4; j++)
                {
                    sport.Add(cn.ports[j]);
                    //pt = (Port)cn.ports[j];
                    //pt2 = new Port(pt.type, pt.width, pt.location, pt.parent, pt.napr, pt.name);
                    //pt2.line = (Line)sline[sline.IndexOf(pt.line)];
                    //sport.Add(pt2);
                }
            }
        }

        public void Add(Element el) //добавление элемента к уже выбранным
        {
            isElementListChanged = true;
            selected.Add(el);
            //choiseConnect();
            //choiseLine();
            //choisePort();
        }

        public bool Find(Element el) //поиск наличи€ элемента среди выбранных
        {
            return selected.Contains(el);
        }

        public bool Find(Connect cn) //поиск наличи€ соединени€ среди выбранных (добавл€ютс€ автоматически)
        {
            CheckElementListChanged();
            return sconnect.Contains(cn);
        }

        public void CheckElementListChanged()
        {
            if (isElementListChanged)
            {
                choiseConnect();
                choiseLine();
                choisePort();
                isElementListChanged = false;
            }
        }

        public void Clear(Element el) //удаление элемента из выбранных
        {
            isElementListChanged = true;
            selected.Remove(el);
            el.parent.removeInvisibleAll();
            //choiseConnect();
            //choiseLine();
            //choisePort();
        }

        public void ClearAll() //очистка списка выделенных
        {
            isElementListChanged = true;
            if (selected.Count > 0)
                selected[0].parent.removeInvisibleAll();
            else if (sconnect.Count > 0)
                sconnect[0].parent.removeInvisibleAll();
            else if (sline.Count > 0)
                sline[0].parentBegin.parent.parent.removeInvisibleAll();
            else if (sport.Count > 0)
                sport[0].parent.parent.removeInvisibleAll();
            selected.Clear();
            sconnect.Clear();
            sline.Clear();
            sport.Clear();
        }

        public void DrawSelected(Graphics g) //рисование рамок выделенных элементов
        {
            Pen p = new Pen(Color.FromArgb(152, 202, 239));
            RectangleF tmp;
            CheckElementListChanged();
            for (int i = 0; i < selected.Count; i++)
            {
                tmp = selected[i].border;
                g.FillRectangle(p.Brush, new RectangleF(tmp.X - 2, tmp.Y - 2, tmp.Width + 4, tmp.Height + 4));
            }
            for (int i = 0; i < sconnect.Count; i++)
            {
                tmp = sconnect[i].border;
                g.FillRectangle(p.Brush, new RectangleF(tmp.X - tmp.Width - 3, tmp.Y - tmp.Height - 3, tmp.Width * 2 + 6, tmp.Height * 2 + 6));
            }
        }

        public void MouseDown(Construct parent) //обработка нажати€ мыши на выделенном элементе
                                //(скрытие всех линий идущих от выделенных элементов)
        {
            CheckElementListChanged();
            int i, j;
            Element el;
            Connect cn;
            Port pt;
            for (i = 0; i < selected.Count; i++)
                parent.RemoveElementMark((Element)selected[i]);
            for (i = 0; i < selected.Count; i++)
            {
                el = (Element)selected[i];
                for (j = 0; j < el.ports.Count; j++)
                {
                    pt = (Port)el.ports[j];
                    if (pt.isLine)
                    {
                        pt.line.setToChange();
                    }
                }
            }
            for (i = 0; i < sconnect.Count; i++)
            {
                cn = (Connect)sconnect[i];
                for (j = 0; j < cn.ports.Count; j++)
                {
                    pt = (Port)cn.ports[j];
                    if (pt.isLine)
                    {
                        pt.line.setToChange();
                    }
                }
            }
        }

        public void MouseUp(Construct parent) //перерисовка линий после перемещени€ выделенных элементов
        {
            int i, j;
            Element el;
            Connect cn;
            Port pt;
            for (i = 0; i < selected.Count; i++)
                parent.AddElementMark((Element)selected[i]);
            for (i = 0; i < selected.Count; i++)
            {
                el = (Element)selected[i];
                for (j = 0; j < el.ports.Count; j++)
                {
                    pt = (Port)el.ports[j];
                    if (pt.isLine)
                        if (pt.line.invisible)
                        {
                            pt.line.setToChange();
                        }
                }
            }
            for (i = 0; i < sconnect.Count; i++)
            {
                cn = (Connect)sconnect[i];
                for (j = 0; j < cn.ports.Count; j++)
                {
                    pt = (Port)cn.ports[j];
                    if (pt.isLine)
                        if (pt.line.invisible)
                        {
                            pt.line.setToChange();
                        }
                }
            }
        }

        public void MoveSelected(Construct parent, float dx, float dy) //перемещение выделенных элементов
        {
            int i;           
            for (i = 0; i < selected.Count; i++)            
                parent.MoveElement((Element)selected[i], dx, dy);
            for (i = 0; i < sconnect.Count; i++)
                parent.MoveConnect((Connect)sconnect[i], dx, dy);
        }

        public void Paste(PointF copyLocation, Construct parent)
        {
            if (Count == 0)
                return;
            List<Connect> pastedCon = new List<Connect>();
            List<Port> newPort = new List<Port>();
            List<Element> newSelected = new List<Element>();
            Port pt, pt2;
            Element el, el2;
            Line ln, ln2;
            Connect cn, cn2;
            int i, j;
            float dx, dy;
            dx = ((Element)selected[0]).border.X;
            dy = ((Element)selected[0]).border.Y;
            for (i = 1; i < selected.Count; i++)
            {
                el = (Element)selected[i];
                if (dx > el.border.X)
                    dx = el.border.X;
                if (dy > el.border.Y)
                    dy = el.border.Y;
            }
            dx = copyLocation.X - dx;
            dy = copyLocation.Y - dy;
            for (i = 0; i < sport.Count; i++)
            {
                pt = (Port)sport[i];
                newPort.Add(new Port(pt.inout, pt.type, pt.location, null, pt.napr, pt.name, pt.LeftBusBound, pt.RightBusBound, pt.bus));
            }
            for (i = 0; i < selected.Count; i++)
            {
                el = (Element)selected[i];
                el2 = new Element(el.img, parent, el.isLeftInOut, el.name, el.elementType);
                el2.text = el.text;
                for (j = 0; j < el.ports.Count; j++)
                {
                    pt = (Port)newPort[sport.IndexOf(el.ports[j])];
                    el2.ports.Add(pt);
                    pt.parent = el2;
                }
                el2.border = new RectangleF(el.border.X + dx, el.border.Y + dy, el.border.Width, el.border.Height);
                parent.AddElement(el2);
                newSelected.Add(el2);
            }
            for (i = 0; i < sconnect.Count; i++)
            {
                cn = (Connect)sconnect[i];
                cn2 = new Connect(new PointF(cn.border.X + dx, cn.border.Y + dy), parent, cn.name);
                cn2.ports.Clear();
                for (j = 0; j < cn.ports.Count; j++)
                {
                    pt = (Port)newPort[sport.IndexOf(cn.ports[j])];
                    cn2.ports.Add(pt);
                    pt.parent = cn2;
                }
                pastedCon.Add(cn2);
                parent.AddConnect(cn2);
            }
            for (i = 0; i < sline.Count; i++)
            {
                ln = (Line)sline[i];
                pt = (Port)newPort[sport.IndexOf(ln.parentBegin)];
                pt2 = (Port)newPort[sport.IndexOf(ln.parentEnd)];
                ln2 = new Line(pt, pt2, ln.bus, ln.LeftBusBound, ln.RightBusBound, ln.name);
                ln2.isSignal = ln.isSignal;
                pt.line = ln2;
                pt2.line = ln2;
                ln2.PassFinding();
                ln2.assign.Clear();
                foreach (KeyValuePair<int, int> ind in ln.assign)
                    ln2.assign.Add(ind.Key, ind.Value);
                parent.AddLine(ln2);
            }
            ClearAll();
            for (i = 0; i < newSelected.Count; i++)
                Add((Element)newSelected[i]);
            foreach (Connect buf in pastedCon)
                parent.CheckConnect(buf);
            parent.parent.history.Changed();
        }

        public void Delete(Construct parent)
        {            
            int i;
            CheckElementListChanged();
            for (i = 0; i < selected.Count; i++)
                parent.RemoveElement((Element)selected[i]);
            for (i = 0; i < sconnect.Count; i++)
                parent.RemoveConnect((Connect)sconnect[i]);
            ClearAll();
            parent.parent.history.Changed();
        }

        public void CopyIntoBuffer(Actions buffer)
        {
            buffer.ClearAll();
            int i;
            for (i = 0; i < selected.Count; i++)
                buffer.Add((Element)selected[i]);
        }

        public void CloneFromBuffer(List<Element> cselected, List<Connect> csconnect, List<Line> csline, List<Port> csport)
        {
            ClearAll();
            foreach (Element el in cselected)
                selected.Add(el);
            foreach (Connect cn in csconnect)
                sconnect.Add(cn);
            foreach (Line ln in csline)
                sline.Add(ln);
            foreach (Port pt in csport)
                sport.Add(pt);
        }

        public void CloneIntoBuffer(Actions buffer)
        {
            buffer.CloneFromBuffer(selected, sconnect, sline, sport);
        }

        public void ChoiseAll()
        {
            choiseConnect();
            choiseLine();
            choisePort();
            isElementListChanged = false;
        }
    }
}
