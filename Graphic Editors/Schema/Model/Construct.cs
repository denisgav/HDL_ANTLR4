using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Schematix_all;

namespace csx
{
    public class Construct //описывает всю структуру схеммы
    {
        public SchemaUserControl parent;
        public Actions buffer; //буфер обмена
        public static int interval = 8; //интервал между линиями
        public List<Element> elements; //массив элементов
        public List<Connect> connects; //массив узлов
        public List<Line> lines; //массив линий
        public SortedList<float, int> horGrid; //горизонтальная разметка
        public SortedList<float, int> vertGrid; //вертикальная разметка
        public Events events; //обработчик событий

        public int nRigthPorts = 0;
        public int nLeftPorts = 0;

        public int ElementsCount
        {
            get
            {
                return elements.Count;
            }
        }

        public int ConnectsCount
        {
            get
            {
                return connects.Count;
            }
        }

        public int LinesCount
        {
            get
            {
                return lines.Count;
            }
        }

        public Element ReturnElement(int index)
        {
            return (Element) elements[index];
        }

        public Connect ReturnConnect(int index)
        {
            return (Connect)connects[index];
        }

        public Line ReturnLine(int index)
        {
            return (Line)lines[index];
        }

        public Construct(SchemaUserControl parent)
        {
            this.buffer = new Actions();
            this.parent = parent;
            elements = new List<Element>();
            connects = new List<Connect>();
            lines = new List<Line>();
            horGrid = new SortedList<float, int>();
            vertGrid = new SortedList<float, int>();
            events = new Events(this);
        }

        public void AddElementMark(Common obj) //добавление меток, связанных с элементом
        {
            addHorLabel(obj.border.Left - interval);
            addHorLabel(obj.border.Right + interval);
            addVertLabel(obj.border.Top - interval);
            addVertLabel(obj.border.Bottom + interval);

            addHorLabel(obj.border.Left + (int)obj.border.Width / 2);
            addVertLabel(obj.border.Top + (int)obj.border.Height / 2);
        }

        public void AddLineMark(Line line) //добавление меток, связанный с линией
        {
            for (int i = 1; i < line.points.Length; i++)
                if (line.points[i - 1].X == line.points[i].X)
                {
                    addHorLabel(line.points[i].X - interval);
                    addHorLabel(line.points[i].X + interval);
                }
                else
                {
                    addVertLabel(line.points[i].Y - interval);
                    addVertLabel(line.points[i].Y + interval);
                }
        }
        
        public void AddElement(Element obj) //добавление элемента
        {
            AddElementMark(obj);
            elements.Add(obj);
            this.buffer.CheckElementListChanged();
        }

        public void AddConnect(Connect obj) //добавление узла
        {
            //AddConnectMark(obj);
            this.buffer.ChoiseAll();
            connects.Add(obj);            
        }

        public void AddConnectMark(Connect obj) //добавление меток, связанных с узлом
        {
            addHorLabel(obj.border.X);
            addVertLabel(obj.border.Y);
            addHorLabel(obj.border.X - interval);
            addHorLabel(obj.border.X + interval);
            addVertLabel(obj.border.Y - interval);
            addVertLabel(obj.border.Y + interval);
        }

        public void RemoveConnectMark(Common obj) //удаление меток, связанных с узлом
        {
            removeHorLabel(obj.border.X);
            removeVertLabel(obj.border.Y);
            removeHorLabel(obj.border.X - interval);
            removeHorLabel(obj.border.X + interval);
            removeVertLabel(obj.border.Y - interval);
            removeVertLabel(obj.border.Y + interval);
        }

        public void AddLine(Line line) //добавление линии
        {
            if (line.points != null)
            {
                AddLineMark(line);
            }
            lines.Add(line);
            this.buffer.ChoiseAll();
        }

        public void RemoveElementMark(Common obj) //удаление меток, связанных с элементом            
        {
            removeHorLabel(obj.border.Left - interval);
            removeHorLabel(obj.border.Right + interval);
            removeVertLabel(obj.border.Top - interval);
            removeVertLabel(obj.border.Bottom + interval);

            removeHorLabel(obj.border.Left + (int)obj.border.Width / 2);
            removeVertLabel(obj.border.Top + (int)obj.border.Height / 2);
        }

        public void RemoveLineMark(Line line) //удаление меток, связанных с линией
        {
            for (int i = 1; i < line.points.Length; i++)
                if (line.points[i - 1].X == line.points[i].X)
                {
                    removeHorLabel(line.points[i].X - interval);
                    removeHorLabel(line.points[i].X + interval);
                }
                else
                {
                    removeVertLabel(line.points[i].Y - interval);
                    removeVertLabel(line.points[i].Y + interval);
                }
        }

        public void RemoveElement(Element obj) //удаление элемента
        {
            RemoveElementMark(obj);
            elements.Remove(obj);            
            Port pt;
            for (int i = 0; i < obj.ports.Count; i++)
            {
                pt = (Port)obj.ports[i];
                if (pt.isLine)
                    RemoveLine(pt.line);
            }
            this.buffer.CheckElementListChanged();
        }

        public void RemoveConnect(Connect obj) //удаление узла
        {
            //RemoveConnectMark(obj);
            connects.Remove(obj);
            Port pt;
            for (int i = 0; i < obj.ports.Count; i++)
            {
                pt = (Port)obj.ports[i];
                if (pt.isLine)
                    RemoveLine(pt.line);
            }
        }

        public void RemoveLineWithoutCheckConnect(Line line) //удаление линии без проверки соединений
        {
            RemoveLineMark(line);
            lines.Remove(line);
            line.parentBegin.line = null;
            line.parentEnd.line = null;            
        }

        public void CheckConnect(Connect cn)
        {
            Line ln;
            Port p1 = null;
            Port p2 = null;
            int cnt = 0, i;
            for (i = 0; i < 4; i++)
                if (((Port)cn.ports[i]).isLine)
                    cnt++;
            if (cnt == 2)
            {
                for (i = 0; i < 4; i++)
                    if (((Port)cn.ports[i]).isLine)
                    {
                        p1 = (Port)cn.ports[i];
                        break;
                    }
                for (i++; i < 4; i++)
                    if (((Port)cn.ports[i]).isLine)
                    {
                        p2 = (Port)cn.ports[i];
                        break;
                    }
                connects.Remove(cn);
                //RemoveConnectMark(cn);
                RemoveLineWithoutCheckConnect(p1.line);
                RemoveLineWithoutCheckConnect(p2.line);
                //RemoveConnect(cn);
                ln = new Line(p1.line.parentBegin == p1 ? p1.line.parentEnd : p1.line.parentBegin,
                    p2.line.parentBegin == p2 ? p2.line.parentEnd : p2.line.parentBegin, p1.line.name);
                if (p1.line.parentBegin == p1)
                    p1.line.parentEnd.line = ln;
                else
                    p1.line.parentBegin.line = ln;
                if (p2.line.parentBegin == p2)
                    p2.line.parentEnd.line = ln;
                else
                    p2.line.parentBegin.line = ln;
                ln.assign.Clear();
                bool p1b = p1.line.parentBegin == p1;
                bool p2b = p2.line.parentBegin == p2;
                int di = Math.Sign(cn.ports[0].RightBusBound - cn.ports[0].LeftBusBound);
                if (p1b)
                {
                    if (p2b)
                    {
                        for (i = cn.ports[0].LeftBusBound; i != cn.ports[0].RightBusBound + di; i+=di)
                            if (p1.line.assign.ContainsKey(i) && p2.line.assign.ContainsKey(i))
                                ln.assign.Add(p1.line.assign.Values[p1.line.assign.IndexOfKey(i)], p2.line.assign.Values[p2.line.assign.IndexOfKey(i)]);
                    }
                    else
                    {
                        for (i = cn.ports[0].LeftBusBound; i != cn.ports[0].RightBusBound + di; i += di)
                            if (p1.line.assign.ContainsKey(i) && p2.line.assign.ContainsValue(i))
                                ln.assign.Add(p1.line.assign.Values[p1.line.assign.IndexOfKey(i)], p2.line.assign.Keys[p2.line.assign.IndexOfValue(i)]);
                    }
                }
                else
                {
                    if (p2b)
                    {
                        for (i = cn.ports[0].LeftBusBound; i != cn.ports[0].RightBusBound + di; i += di)
                            if (p1.line.assign.ContainsValue(i) && p2.line.assign.ContainsKey(i))
                                ln.assign.Add(p1.line.assign.Keys[p1.line.assign.IndexOfValue(i)], p2.line.assign.Values[p2.line.assign.IndexOfKey(i)]);
                    }
                    else
                    {
                        for (i = cn.ports[0].LeftBusBound; i != cn.ports[0].RightBusBound + di; i += di)
                            if (p1.line.assign.ContainsValue(i) && p2.line.assign.ContainsValue(i))
                                ln.assign.Add(p1.line.assign.Keys[p1.line.assign.IndexOfValue(i)], p2.line.assign.Keys[p2.line.assign.IndexOfValue(i)]);
                    }
                }
                if (ln.assign.Count > 0)
                {
                    ln.bus = true;
                    ln.LeftBusBound = 0;
                    ln.RightBusBound = ln.assign.Count - 1;
                }
                else
                {
                    ln.bus = false;
                    ln.LeftBusBound = 0;
                    ln.RightBusBound = 0;
                    if (di == 0)
                        ln.assign.Add(0, 0);
                }
                if (ln.assign.Count > 0)
                {
                    ln.isSignal = p1.line.isSignal && p2.line.isSignal;
                    ln.PassFinding();
                    AddLine(ln);
                }
            }
        }

        public void RemoveLine(Line line) //удаление линии
        {
            RemoveLineMark(line);
            lines.Remove(line);
            line.parentBegin.line = null;
            line.parentEnd.line = null;
            if (!line.parentBegin.parent.isElement)
            {
                if (line.isSignal)
                {
                    if (connects.Contains((Connect)line.parentBegin.parent))
                        RemoveConnect((Connect)line.parentBegin.parent);
                }
                else
                    CheckConnect((Connect)line.parentBegin.parent);
            }
            else
                if (line.isSignal)
                    if (elements.Contains((Element)line.parentBegin.parent))
                        RemoveElement((Element)line.parentBegin.parent);
            if (!line.parentEnd.parent.isElement)
            {
                if (line.isSignal)
                {
                    if (connects.Contains((Connect)line.parentEnd.parent))
                        RemoveConnect((Connect)line.parentEnd.parent);
                }
                else
                    CheckConnect((Connect)line.parentEnd.parent);
            }
            else
                if (line.isSignal)
                    if (elements.Contains((Element)line.parentEnd.parent))
                        RemoveElement((Element)line.parentEnd.parent);
            this.buffer.ChoiseAll();
        } 
       
        private void addHorLabel(float X) //добавление горизонтальной метки
        {
            if (horGrid.ContainsKey(X))
            {
                horGrid[X]++;
            }
            else
                horGrid.Add(X, 1);
        }

        private void addVertLabel(float Y) //добавление вертикальной метки
        {
            if (vertGrid.ContainsKey(Y))
            {
                vertGrid[Y]++;                
            }
            else
                vertGrid.Add(Y, 1);
        }

        private void removeHorLabel(float X) //удаление горизонтальной разметки
        {
            if (horGrid.ContainsKey(X))
            {
                horGrid[X]--;
                if ((int)horGrid[X] == 0)
                    horGrid.Remove(X);
            }
        }

        private void removeVertLabel(float Y) //удаление вертикальной разметки
        {
            if (vertGrid.ContainsKey(Y))
            {
                vertGrid[Y]--;
                if ((int)vertGrid[Y] == 0)                
                    vertGrid.Remove(Y);
            }
        }

        public RectangleF Draw(Graphics e)
        {
            PointF min = new PointF(100, 100);
            PointF max = new PointF(100, 100);
            int i;
            //for (i = 0; i < horGrid.Count; i++)
            //{
            //    e.DrawLine(Pens.Crimson, (int)horGrid.GetKey(i), 0, (int)horGrid.GetKey(i), parent.Height);
            //}
            //for (i = 0; i < vertGrid.Count; i++)
            //{
            //    e.DrawLine(Pens.Crimson, 0, (int)vertGrid.GetKey(i), parent.Width, (int)vertGrid.GetKey(i));
            //}
            //foreach (KeyValuePair<float, int> ind in horGrid)
            //    e.DrawLine(Pens.Crimson, ind.Key, 0, ind.Key, parent.Height);
            //foreach (KeyValuePair<float, int> ind in vertGrid)
            //    e.DrawLine(Pens.Crimson, 0, ind.Key, parent.Width, ind.Key);
            Element el;
            for (i = 0; i < elements.Count; i++)
            {
                el = (Element)elements[i];
                if (min.X > el.border.X)
                    min.X = el.border.X;
                if (min.Y > el.border.Y)
                    min.Y = el.border.Y;
                if (max.X < el.border.Right)
                    max.X = el.border.Right;
                if (max.Y < el.border.Bottom)
                    max.Y = el.border.Bottom;
                el.Draw(e);
            }
            Line ln;
            int j;
            for (i = 0; i < lines.Count; i++)
            {
                ln = (Line)lines[i];
                for (j = 0; j < ln.points.Length; j++)
                {
                    if (min.X > ln.points[j].X)
                        min.X = ln.points[j].X;
                    if (min.Y > ln.points[j].Y)
                        min.Y = ln.points[j].Y;
                    if (max.X < ln.points[j].X)
                        max.X = ln.points[j].X;
                    if (max.Y < ln.points[j].Y)
                        max.Y = ln.points[j].Y;
                }
                ln.Draw(e);
            }
            Connect cn;
            for (i = 0; i < connects.Count; i++)
            {
                cn = (Connect)connects[i];
                if (min.X > cn.border.X)
                    min.X = cn.border.X;
                if (min.Y > cn.border.Y)
                    min.Y = cn.border.Y;
                if (max.X < cn.border.Right)
                    max.X = cn.border.Right;
                if (max.Y < cn.border.Bottom)
                    max.Y = cn.border.Bottom;
                cn.Draw(e);
            }
            return new RectangleF(min.X, min.Y, max.X - min.X, max.Y - min.Y);
        }

        public void MoveElement(Common obj, float dx, float dy)
        {
            obj.border.X += dx;
            obj.border.Y += dy;
        }

        public void MoveConnect(Common obj, float dx, float dy)
        {            
            obj.border.X += dx;
            obj.border.Y += dy;
        }        

        public void removeInvisible()
        {
            Line ln;
            for (int i = 0; i < LinesCount; i++)
            {
                ln = ReturnLine(i);
                if (ln.toChange && ln.invisible == true)
                {
                    ln.PassFinding();
                    ln.changeInvisible(false);
                    AddLineMark(ln);
                }
                else
                {
                    ln.toChange = false;
                }
            }
        }

        public void removeInvisibleAll()
        {
            Line ln;
            for (int i = 0; i < LinesCount; i++)
            {
                ln = ReturnLine(i);
                if (ln.invisible == true)
                {
                    ln.PassFinding();
                    ln.changeInvisible(false);
                    AddLineMark(ln);
                }
            }
        }

        public void setInvisible()
        {
            Line ln;
            for (int i = 0; i < LinesCount; i++)
            {
                ln = ReturnLine(i);
                if (ln.toChange && ln.invisible == false)
                {
                    ln.changeInvisible(true);
                    RemoveLineMark(ln);
                }
                else
                {
                    ln.toChange = false;
                }
            }
        }

        Metafile mtf;
        private bool EnumMetafileProc(EmfPlusRecordType eprt, int iFlags, int iDataSize, IntPtr ipData, PlayRecordCallback prc)
        {                         
            byte[] abyData = new byte[iDataSize];
            if (ipData != IntPtr.Zero)
                Marshal.Copy(ipData, abyData, 0, iDataSize);
            mtf.PlayRecord(eprt, iFlags, iDataSize, abyData);
            return true;
        }

        public void save(Stream stream)
        { 
            ArrayList buf = new ArrayList();
            Element el;
            Connect cn;
            Port pt;
            Line ln;
            int i, j;
            for (i = 0; i < elements.Count; i++)
            {
                el = (Element)elements[i];
                for (j = 0; j < el.ports.Count; j++)
                    buf.Add(el.ports[j]);
            }
            for (i = 0; i < connects.Count; i++)
            {
                cn = (Connect)connects[i];
                for (j = 0; j < 4; j++)
                    buf.Add(cn.ports[j]);
            }
            BinaryWriter w = new BinaryWriter(stream);
            w.Write(buf.Count);
            for (i = 0; i < buf.Count; i++)
            {
                pt = (Port)buf[i];
                w.Write(pt.type);
                w.Write((Double)pt.location.X);
                w.Write((Double)pt.location.Y);
                w.Write(pt.napr);
                w.Write((int)pt.inout);
                w.Write(pt.bus);
                w.Write(pt.LeftBusBound);
                w.Write(pt.RightBusBound);
                w.Write(pt.name);
            }
            w.Write(elements.Count);
            ImageFormat imf = new ImageFormat(ImageFormat.Emf.Guid);  
            Graphics gr = parent.CreateGraphics();
            Graphics gr2 = parent.CreateGraphics();
            IntPtr hdc = gr2.GetHdc();
            RectangleF rct;
            MemoryStream mst;
            GraphicsUnit gu = GraphicsUnit.Pixel;
            for (i = 0; i < elements.Count; i++)
            {
                el = (Element)elements[i];
                w.Write((Double)el.border.X);
                w.Write((Double)el.border.Y);
                w.Write((Double)el.border.Width);
                w.Write((Double)el.border.Height);
                w.Write(el.isLeftInOut);
                w.Write((int)el.elementType);
                mst = new MemoryStream();
                rct = el.img.GetBounds(ref gu);
                Metafile mttf = new Metafile(mst, hdc, rct, MetafileFrameUnit.Pixel);
                gr = Graphics.FromImage(mttf);
                mtf = el.img;
                gr.EnumerateMetafile(mtf, new PointF(0, 0), new Graphics.EnumerateMetafileProc(EnumMetafileProc));
                gr.Dispose();
                w.Write((int)mst.Length);
                byte[] by = new byte[(int)mst.Length];
                mst.Seek(0, SeekOrigin.Begin);
                mst.Read(by, 0, (int)mst.Length);
                w.Write(by, 0, (int)mst.Length);
                w.Write(el.name);
                w.Write(el.ports.Count);
                for (j = 0; j < el.ports.Count; j++)
                    w.Write(buf.IndexOf(el.ports[j]));
                w.Write(el.text);
            }
            gr2.ReleaseHdc(hdc);
            gr2.Dispose();
            gr.Dispose();
            w.Write(connects.Count);
            for (i = 0; i < connects.Count; i++)
            {
                cn = (Connect)connects[i];
                w.Write((Double)cn.border.X);
                w.Write((Double)cn.border.Y);
                w.Write(cn.name);
                w.Write(buf.IndexOf(cn.ports[0]));
                w.Write(buf.IndexOf(cn.ports[1]));
                w.Write(buf.IndexOf(cn.ports[2]));
                w.Write(buf.IndexOf(cn.ports[3]));
            }
            w.Write(lines.Count);
            for (i = 0; i < lines.Count; i++)
            {
                ln = (Line)lines[i];
                w.Write(ln.name);
                w.Write(ln.bus);
                w.Write(ln.LeftBusBound);
                w.Write(ln.RightBusBound);
                w.Write(buf.IndexOf(ln.parentBegin));
                w.Write(buf.IndexOf(ln.parentEnd));
                w.Write(ln.points.Length);
                for (j = 0; j < ln.points.Length; j++)
                {
                    w.Write((Double)ln.points[j].X);
                    w.Write((Double)ln.points[j].Y);
                }
                w.Write(ln.assign.Count);
                foreach (KeyValuePair<int, int> ind in ln.assign)
                {
                    w.Write(ind.Key);
                    w.Write(ind.Value);
                }
                w.Write(ln.isSignal);
            }
        }

        public void open(Stream stream)
        {
            BinaryReader r = new BinaryReader(stream);
            ArrayList buf = new ArrayList();
            elements.Clear();
            connects.Clear();
            lines.Clear();
            buffer.ClearAll();
            horGrid.Clear();
            vertGrid.Clear();
            Element el;
            Connect cn;
            Port pt, pt2;
            Line ln;
            int[] param = new int[7];
            float[] paramf = new float[4];
            string name, str;
            bool flag;
            int i, j, cnt = r.ReadInt32();
            for (i = 0; i < cnt; i++)
            {
                str = r.ReadString();
                paramf[0] = (float)r.ReadDouble();
                paramf[1] = (float)r.ReadDouble();
                param[2] = r.ReadInt32();
                param[3] = r.ReadInt32();
                flag = r.ReadBoolean();
                param[4] = r.ReadInt32();
                param[5] = r.ReadInt32();
                name = r.ReadString();
                pt = new Port((portInOut)param[3], str, new PointF(paramf[0], paramf[1]), null, param[2], name, param[4], param[5], flag);
                buf.Add(pt);
            }
            cnt = r.ReadInt32();
            Metafile mtf;
            for (i = 0; i < cnt; i++)
            {
                paramf[0] = (float)r.ReadDouble();
                paramf[1] = (float)r.ReadDouble();
                paramf[2] = (float)r.ReadDouble();
                paramf[3] = (float)r.ReadDouble();
                flag = r.ReadBoolean();
                param[4] = r.ReadInt32();
                MemoryStream mst = new MemoryStream();                
                param[6] = r.ReadInt32();
                byte[] by = new byte[param[6]];
                by = r.ReadBytes(param[6]);
                mst.Seek(0, SeekOrigin.Begin);
                mst.Write(by, 0, param[6]);
                mst.Seek(0, SeekOrigin.Begin);
                mtf = (Metafile)Metafile.FromStream(mst);                
                name = r.ReadString();
                param[5] = r.ReadInt32();
                el = new Element(mtf, this, flag, name, (ElementType)param[4]);
                el.border = new RectangleF(paramf[0], paramf[1], paramf[2], paramf[3]);
                //el.elementType = (ElementType)param[4];
                for (j = 0; j < param[5]; j++)
                {
                    el.Add((Port)buf[r.ReadInt32()]);
                }
                str = r.ReadString();
                el.text = str;
                AddElementMark(el);
                elements.Add(el);
            }
            cnt = r.ReadInt32();
            for (i = 0; i < cnt; i++)
            {
                paramf[0] = (float)r.ReadDouble();
                paramf[1] = (float)r.ReadDouble();
                name = r.ReadString();
                param[2] = r.ReadInt32();
                param[3] = r.ReadInt32();
                param[4] = r.ReadInt32();
                param[5] = r.ReadInt32();
                cn = new Connect(new PointF(paramf[0], paramf[1]), this, name);
                cn.ports.Clear();
                cn.Add((Port)buf[param[2]]);
                cn.Add((Port)buf[param[3]]);
                cn.Add((Port)buf[param[4]]);
                cn.Add((Port)buf[param[5]]);
                //AddConnectMark(cn);
                connects.Add(cn);
            }
            cnt = r.ReadInt32();
            for (i = 0; i < cnt; i++)
            {
                name = r.ReadString();
                flag = r.ReadBoolean();                
                param[4] = r.ReadInt32();
                param[5] = r.ReadInt32();
                param[1] = r.ReadInt32();
                param[2] = r.ReadInt32();
                param[3] = r.ReadInt32();
                pt = (Port)buf[param[1]];
                pt2 = (Port)buf[param[2]];
                ln = new Line(pt, pt2, name);
                pt.line = ln;
                pt2.line = ln;
                ln.bus = flag;
                ln.LeftBusBound = param[4];
                ln.RightBusBound = param[5];
                ln.points = new PointF[param[3]];
                for (j = 0; j < param[3]; j++)
                {
                    ln.points[j].X = (float)r.ReadDouble();
                    ln.points[j].Y = (float)r.ReadDouble();
                }
                param[6] = r.ReadInt32();
                ln.assign.Clear();
                for (j = 0; j < param[6]; j++)
                {
                    param[1] = r.ReadInt32();
                    param[2] = r.ReadInt32();
                    ln.assign.Add(param[1], param[2]);
                }
                flag = r.ReadBoolean();
                ln.isSignal = flag;
                AddLineMark(ln);
                lines.Add(ln);
            }
            //parent.history.Changed();
            parent.Invalidate();
        }

        public void selectAll()
        {
            buffer.ClearAll();
            int i;
            for (i = 0; i < elements.Count; i++)
                buffer.Add((Element)elements[i]);
        }

        private void volna(int lin, int n1, int n2, ref SortedList<int, SortedList<int, int>> lineAccess)
        {
            /* =============== Параметры
             * 
             * lin          - индекс текущей шины для обработки волной
             * 
             * n1           - номер провода в шине, с которого начинается волна
             * 
             * n2           - цвет (метка) для обозначения проводов, по которым прошла волна. Т.е. в результате все соединенные с этим проводом
             *                провода других линий, должны быть помечены этим цветом
             *      
             * lineAccess   - список, в который помещаются линии, затронутые волной
             * 
             * lineAccess   - список, элемент которого соответствует элементу lineAccess с таким же индексом. Представляет собой сортированные
             *                списки, в которых ключ - номер провода шины из lineAccess, а значение - цвет шины
             *                
             * =============== Локальные переменные
             * 
             * stP, stP2    - список портов, соединенных с линией посредством узлов. используется для нерекурсивного прохождения алгоритма
             *                волны через линии, соединенные с текущей
             *                
             * stI, stI2    - список номеров проводов узлов (из stP с соответствующим индексом), к которым присоединен провод с цветом n2
             * 
             * =============== Пояснения в тексте функции
             * 
             * 1.           - добавление портов узла, (если линия заканчивается на узел), для дальнейшего распространения волны через
             *                линии, присоединенные к этим портам
             *                
             */

            int i, j, d;
            List<Port> stP, stP2;
            List<int> stI, stI2;
            Line ln = lines[lin];
            stP = new List<Port>(); 
            stI = new List<int>();
            //if (!lineAccess.ContainsKey(lin))
            //{
            //    lineAccess.Add(lin, new SortedList<int,int>());
            //}
            //if (!lineAccess[lin].ContainsKey(n1))
            lineAccess[lin].Add(n1, n2);
            if (!ln.parentBegin.parent.isElement) /* 1. */
                for (i = 0; i < 4; i++)
                    if (ln.parentBegin.parent.ports[i].isLine && ln.parentBegin.parent.ports[i].line != ln
                        && !stP.Contains(ln.parentBegin.parent.ports[i]) && ln.assign.Count > n1)
                    {
                        stP.Add(ln.parentBegin.parent.ports[i]);
                        stI.Add(ln.assign.Keys[n1]);                    
                    }
            if (!ln.parentEnd.parent.isElement) /* 1. */
                for (i = 0; i < 4; i++)
                    if (ln.parentEnd.parent.ports[i].isLine && ln.parentEnd.parent.ports[i].line != ln
                        && !stP.Contains(ln.parentEnd.parent.ports[i]) && ln.assign.Count > n1)
                    {
                        stP.Add(ln.parentEnd.parent.ports[i]);
                        stI.Add(ln.assign.Values[n1]);
                    }
            int bufLin;
            Line bufLn;
            while (stP.Count > 0)
            {
                stP2 = new List<Port>();
                stI2 = new List<int>();
                for (i = 0; i < stP.Count; i++)
                {
                    bufLn = stP[i].line;
                    bufLin = lines.IndexOf(bufLn);
                    //if (!lineAccess.ContainsKey(bufLin))
                    //{
                    //    lineAccess.Add(bufLin, new SortedList<int,int>());                       
                    //}
                    if (bufLn.parentBegin == stP[i])
                    {
                        if (bufLn.assign.ContainsKey(stI[i]))                        
                        {
                            d = bufLn.assign.IndexOfKey(stI[i]);
                            if (!lineAccess[bufLin].ContainsKey(d))
                            {
                                lineAccess[bufLin].Add(d, n2);
                                if (!bufLn.parentEnd.parent.isElement) /* 1. */
                                    for (j = 0; j < 4; j++)
                                        if (bufLn.parentEnd.parent.ports[j].isLine && bufLn.parentEnd.parent.ports[j].line != bufLn
                                            && !stP2.Contains(bufLn.parentEnd.parent.ports[j]) && ln.assign.Count > d)
                                        {
                                            stP2.Add(bufLn.parentEnd.parent.ports[j]);
                                            stI2.Add(bufLn.assign.Values[d]);
                                        }
                            }
                        }
                    }
                    else
                        if (bufLn.assign.ContainsValue(stI[i]))
                        {
                            d = bufLn.assign.IndexOfValue(stI[i]);
                            if (!lineAccess[bufLin].ContainsKey(d))
                            {
                                lineAccess[bufLin].Add(d, n2);
                                if (!bufLn.parentBegin.parent.isElement) /* 1. */
                                    for (j = 0; j < 4; j++)
                                        if (bufLn.parentBegin.parent.ports[j].isLine && bufLn.parentBegin.parent.ports[j].line != bufLn
                                            && !stP2.Contains(bufLn.parentBegin.parent.ports[j]) && ln.assign.Count > d)
                                        {
                                            stP2.Add(bufLn.parentBegin.parent.ports[j]);
                                            stI2.Add(bufLn.assign.Keys[d]);
                                        }
                            }
                        }
                }
                stP = stP2;
                stI = stI2;
            }
        }

        public void vvv()
        {
            Graphics g = parent.CreateGraphics();
            for (int i = 0; i < lines.Count; i++)
                if (lines[i].assign.Count < 1)
                    g.DrawLines(Pens.Red, lines[i].points);
        }

        private void porting(string Name1, string Name2, SortedList<int, int> assign, int leftBound, int rightBound, 
                             ref List<string> l1, ref List<string> l2)
        {
            if (assign.Count == 0)
                return;
            int i, di;

            l1.Clear();
            l2.Clear();
            
            di = Math.Sign(rightBound - leftBound);
            if (di == 0)
            {
                l1.Add(Name1 + "(" + leftBound.ToString() + ")");
                l2.Add(Name2 + "(" + assign.Values[0] + ")");
            }
            else
            {
                string napr = (di == -1) ? " downto " : " to ";
                bool oldOpen = false;
                bool oldUp = false;
                bool oldDown = false;
                bool add = false;
                int beginLeft = 0;
                int beginRight = 0;
                for (i = leftBound; i != rightBound + di; i += di)
                {
                    if (!oldOpen && !oldUp && !oldDown)
                    {
                        if (assign.Keys.Contains(i))
                        {
                            add = false;
                            beginLeft = i;
                            beginRight = assign[i];
                            oldUp = true;
                            oldDown = true;
                        }
                        else
                        {
                            add = false;
                            beginLeft = i;
                            oldOpen = true;
                        }
                    }
                    else if (oldUp && oldDown && !oldOpen)
                    {
                        if (assign.Keys.Contains(i))
                        {
                            if (assign[i] - beginRight == 1)
                                oldDown = false;
                            else if (assign[i] - beginRight == -1)
                                oldUp = false;
                            else
                            {
                                if (add)
                                    l2[l2.Count - 1] += "&" + Name2 + "(" + beginRight.ToString() + ")";
                                else
                                {
                                    l2.Add(Name2 + "(" + beginRight.ToString() + ")");
                                    add = true;
                                }
                                beginRight = assign[i];
                            }
                        }
                        else
                        {
                            l1.Add(Name1 + "(" + beginLeft.ToString() + ")");
                            if (add)
                                l2[l2.Count - 1] += "&" + Name2 + "(" + beginRight.ToString() + ")";
                            else
                                l2.Add(Name2 + "(" + beginRight.ToString() + ")");
                            add = false;
                            beginLeft = i;
                            oldUp = false;
                            oldDown = false;
                            oldOpen = true;
                        }
                    }
                    else if (oldUp && !oldDown && !oldOpen)
                    {
                        if (assign.Keys.Contains(i))
                        {
                            if (assign[i] - assign[i - di] != 1)
                            {
                                oldDown = true;
                                if (add)
                                    l2[l2.Count - 1] += "&" + Name2 + "(" + beginRight.ToString() + " to " + assign[i - di].ToString() + ")";
                                else
                                {
                                    l2.Add(Name2 + "(" + beginRight.ToString() + " to " + assign[i - di].ToString() + ")");
                                    add = true;
                                }
                                beginRight = assign[i];
                            }
                        }
                        else
                        {
                            l1.Add(Name1 + "(" + beginLeft.ToString() + napr + (i - di).ToString() + ")");
                            if (add)
                                l2[l2.Count - 1] += "&" + Name2 + "(" + beginRight.ToString() + " to " + assign[i - di].ToString() + ")";
                            else
                                l2.Add(Name2 + "(" + beginRight.ToString() + " to " + assign[i - di].ToString() + ")");
                            add = false;
                            beginLeft = i;
                            oldUp = false;
                            oldOpen = true;
                        }
                    }
                    else if (!oldUp && oldDown && !oldOpen)
                    {
                        if (assign.Keys.Contains(i))
                        {
                            if (assign[i] - assign[i - di] != -1)
                            {
                                oldUp = true;
                                if (add)
                                    l2[l2.Count - 1] += "&" + Name2 + "(" + beginRight.ToString() + " downto " + assign[i - di].ToString() + ")";
                                else
                                {
                                    l2.Add(Name2 + "(" + beginRight.ToString() + " to " + assign[i - di].ToString() + ")");
                                    add = true;
                                }
                                beginRight = assign[i];
                            }
                        }
                        else
                        {
                            l1.Add(Name1 + "(" + beginLeft.ToString() + napr + (i - di).ToString() + ")");
                            if (add)
                                l2[l2.Count - 1] += "&" + Name2 + "(" + beginRight.ToString() + " downto " + assign[i - di].ToString() + ")";
                            else
                                l2.Add(Name2 + "(" + beginRight.ToString() + " to " + assign[i - di].ToString() + ")");
                            add = false;
                            beginLeft = i;
                            oldDown = false;
                            oldOpen = true;
                        }
                    }
                    else if (!oldUp && !oldDown && oldOpen)
                    {
                        if (assign.Keys.Contains(i))
                        {
                            oldUp = true;
                            oldDown = true;
                            oldOpen = false;
                            if (Math.Abs(beginLeft - i) > 1)
                                l1.Add(Name1 + "(" + beginLeft.ToString() + napr + (i - di).ToString() + ")");
                            else
                                l1.Add(Name1 + "(" + beginLeft.ToString() + ")");
                            l2.Add("open");
                            beginLeft = i;
                            beginRight = assign[i];
                        }
                    }
                }
                if (Math.Abs(beginLeft - i) > 1)
                    l1.Add(Name1 + "(" + beginLeft.ToString() + napr + (i - di).ToString() + ")");
                else
                    l1.Add(Name1 + "(" + beginLeft.ToString() + ")");
                if (oldUp && oldDown && !oldOpen)
                {
                    if (add)
                        l2[l2.Count - 1] += "&" + Name2 + "(" + beginRight.ToString() + ")";
                    else
                        l2.Add(Name2 + "(" + beginRight.ToString() + ")");
                }
                else if (oldUp && !oldDown && !oldOpen)
                {
                    if (add)
                        l2[l2.Count - 1] += "&" + Name2 + "(" + beginRight.ToString() + " to " + assign[i - di].ToString() + ")";
                    else
                        l2.Add(Name2 + "(" + beginRight.ToString() + " to " + assign[i - di].ToString() + ")");
                }
                else if (!oldUp && oldDown && !oldOpen)
                {
                    if (add)
                        l2[l2.Count - 1] += "&" + Name2 + "(" + beginRight.ToString() + " downto " + assign[i - di].ToString() + ")";
                    else
                        l2.Add(Name2 + "(" + beginRight.ToString() + " downto " + assign[i - di].ToString() + ")");
                }
                else if (!oldUp && !oldDown && oldOpen)
                    l2.Add("open");
            }
        }

        public void addColorInOut(Port pt, ref List<int> InList, ref List<int> OutList, ref List<int> InOutList, ref List<int> InOut2List,
            SortedList<int, SortedList<int, int>> lineAccess)
        {
            int i;
            Line ln;
            int indLn;
            portInOut inout;
            inout = pt.inout;
            //if (pt.parent.isElement)
            //    if (((Element)pt.parent).elementType == ElementType.ExternPort)
            //    {
            //        if (pt.inout == portInOut.In)
            //            inout = portInOut.Out;
            //        else if (pt.inout == portInOut.Out)
            //            inout = portInOut.In;
            //    }
            if (pt.isLine)
            {
                ln = pt.line;
                indLn = lines.IndexOf(ln);
                if (inout == portInOut.In)
                {
                    if (ln.bus)
                    {
                        for (i = 0; i < ln.assign.Count; i++)
                        {
                            if (!InList.Contains(lineAccess[indLn][i]))
                                InList.Add(lineAccess[indLn][i]);
                        }
                    }
                    else
                        if (!InList.Contains(lineAccess[indLn][0]))
                            InList.Add(lineAccess[indLn][0]);
                }
                else if (inout == portInOut.Out)
                {
                    if (ln.bus)
                    {
                        for (i = 0; i < ln.assign.Count; i++)
                        {
                            if (!OutList.Contains(lineAccess[indLn][i]))
                                OutList.Add(lineAccess[indLn][i]);
                        }
                    }
                    else
                        if (!OutList.Contains(lineAccess[indLn][0]))
                            OutList.Add(lineAccess[indLn][0]);
                }
                else if (inout == portInOut.InOut)
                {
                    for (i = 0; i < ln.assign.Count; i++)
                        {
                            if (!InOutList.Contains(lineAccess[indLn][i]))
                                InOutList.Add(lineAccess[indLn][i]);
                            else
                                if (!InOut2List.Contains(lineAccess[indLn][i]))
                                    InOut2List.Add(lineAccess[indLn][i]);
                        }
                }
                else
                    if (!InOutList.Contains(lineAccess[indLn][0]))
                        InOutList.Add(lineAccess[indLn][0]);
                    else
                        if (!InOut2List.Contains(lineAccess[indLn][0]))
                            InOut2List.Add(lineAccess[indLn][0]);
            }
        }        

        public string export(string EntityName)
        {
            /*
             * экспорт схеммы в VHDL-описание (тип string, переменная rez)
             * 
             * ================ Локальные переменные
             * 
             * lineAccess    - Список, в котором элемент соотносится к индексу линии, и содержит для этой линии
             *                 сортированный список номеров проводов (как ключ)с цветом этих проводов (как значение) - все провода, которые имеют
             *                 электрическое соединение (без учитывания соединения через элементы) окрашиваются в один цвет (см. функцию volna())
             * 
             * lineSort      - Отсортированный список - линии, отсортированные по ширине шины. Линии с одинаковой шириной шины помещаются 
             *                 в список, который помещается в lineSort как значение, в то время как ключом является ширина шины.
             *     
             * lineDecsribe  - Отсортированный список - помещаются линии с уникальными именами, начиная с самых толстых (существует предположение, 
             *                 что все более тонкие линии с таким же именем являются отводами от более толстой линии, это возможно при запрете
             *                 создания двух различных сигналов (без видимого электрического содинения) с одним именем).
             *                 
             * extPorts      - Список внешних портов схеммы (вход, выход или вход-выход), индекс - тип порта (вход\выход\вход-выход и индексы,
             *                 если ширина больше 1 провода), элемент - список имен портов.
             *                 
             * components    - Список компонентов схеммы, индекс - имя компонента, элемент - список со структурой как у extPorts и содержащий
             *                 список портов компонента, отсортированных по типу.
             *                  
             */
            string rez;//сюда пишется результат экспорта
            rez = "library ieee;\n";
            rez += "use ieee.std_logic_1164.all;\n\n";
            int i, j, q, d;
            //List<Line> lineAccess = new List<Line>();
            SortedList<int, SortedList<int, int>> lineAccess = new SortedList<int, SortedList<int, int>>();
            SortedList<int, List<Line>> lineSort = new SortedList<int, List<Line>>();//линии отсортированные по толщине шины
            for (i = 0; i < lines.Count; i++)
            {
                lineAccess.Add(i, new SortedList<int, int>());
                d = lines[i].bus ? Math.Abs(lines[i].LeftBusBound - lines[i].RightBusBound) : 0;
                if (!lineSort.ContainsKey(d))
                    lineSort.Add(d, new List<Line>());
                lineSort[d].Add(lines[i]);
            }
            int ind = 0;
            Line ln;
            int indLine;
            SortedList<string, Line> lineDescribe = new SortedList<string, Line>();//список сигналов
            for (q = lineSort.Count - 1; q >= 0; q--)//перебор толщин линий, начиная с самых толстых
            {
                //lineSort.Values[q].Sort();
                for (i = 0; i < lineSort.Values[q].Count; i++)//перебор списка линий толщины q
                {
                    ln = lineSort.Values[q][i];
                    indLine = lines.IndexOf(ln);
                    if (!lineDescribe.Keys.Contains(ln.name))
                        lineDescribe.Add(ln.name, ln);
                    //if (!lineAccess.ContainsKey(ln))
                    //{
                    //    lineAccess.Add(ln, new SortedList<int,int>());
                    //}
                    if (!ln.bus)
                    {
                        if (!lineAccess[indLine].Keys.Contains(0))
                            volna(indLine, 0, ind++, ref lineAccess);
                    }
                    else
                    {
                        for (j = 0; j < ln.assign.Count; j++)
                            if (!lineAccess[indLine].Keys.Contains(j))
                                volna(indLine, j, ind++, ref lineAccess);
                    }
                }
            }
            List <int> InColor, OutColor, InOutColor, InOut2Color;
            InColor = new List<int>();
            OutColor = new List<int>();
            InOutColor = new List<int>();
            InOut2Color = new List<int>();
            Port pt;

            SortedList<string, List<String>> extPorts = new SortedList<string, List<string>>();
            SortedList<string, SortedList<string, List<string>>> components = new SortedList<string, SortedList<string, List<string>>>(); ;
            for (i = 0; i < elements.Count; i++)
                if (elements[i].elementType == ElementType.ExternPort)
                {
                    addColorInOut(elements[i].ports[0], ref InColor, ref OutColor, ref InOutColor, ref InOut2Color, lineAccess);
                    if (extPorts.ContainsKey(elements[i].ports[0].fullTypeWithInout))
                    {
                        if (!extPorts.Values[extPorts.IndexOfKey(elements[i].ports[0].fullTypeWithInout)].Contains(elements[i].ports[0].name))
                            extPorts.Values[extPorts.IndexOfKey(elements[i].ports[0].fullTypeWithInout)].Add(elements[i].ports[0].name);
                    }
                    else
                    {
                        extPorts.Add(elements[i].ports[0].fullTypeWithInout, new List<string>());
                        extPorts.Values[extPorts.IndexOfKey(elements[i].ports[0].fullTypeWithInout)].Add(elements[i].ports[0].name);
                    }
                }
                else if (elements[i].elementType == ElementType.Element)
                    if (!components.Keys.Contains(elements[i].name))
                    {
                        components.Add(elements[i].name, new SortedList<string,List<string>>());
                        for (j = 0; j < elements[i].ports.Count; j++)
                        {
                            pt = elements[i].ports[j];
                            addColorInOut(pt, ref InColor, ref OutColor, ref InOutColor, ref InOut2Color, lineAccess);
                            if (!components[elements[i].name].ContainsKey(pt.fullTypeWithInout))
                                components[elements[i].name].Add(pt.fullTypeWithInout, new List<string>());
                            components[elements[i].name][pt.fullTypeWithInout].Add(pt.name);
                        }
                    }
            rez += "entity " + EntityName.Trim() + " is\n";//начало экспорта - запись имени entity
            if (extPorts.Count > 0)//запись портов схеммы
            {
                rez += "     port(\n";
                for (i = 0; i < extPorts.Count; i++)
                {
                    if (i != 0)
                        rez += ";\n";
                    rez += "          ";
                    extPorts.Values[i].Sort();
                    for (j = 0; j < extPorts.Values[i].Count; j++)
                    {
                        if (j != 0)
                            rez += ", ";
                        rez += extPorts.Values[i][j];
                    }
                    rez += ": " + extPorts.Keys[i];
                }
                rez += "\n     );\n";
            }
            rez += "end " + EntityName + ";\n";//конец описания entity
            rez += "\narchitecture " + EntityName + " of " + EntityName + " is\n";//описание архитектуры entity покомпонентно
            for (i = 0; i < components.Count; i++)
            {
                rez += "component " + components.Keys[i] + " is\n";
                if (components.Values[i].Count > 0)
                {
                    rez += "     port(\n";
                    for (j = 0; j < components.Values[i].Count; j++)
                    {
                        if (j != 0)
                            rez += ";\n";
                        rez += "          ";
                        components.Values[i].Values[j].Sort();
                        for (q = 0; q < components.Values[i].Values[j].Count; q++)
                        {
                            if (q != 0)
                                rez += ", ";
                            rez += components.Values[i].Values[j][q];
                        }
                        rez += ": " + components.Values[i].Keys[j];
                    }
                    rez += "\n     );\n";
                }
                rez += "end component;\n";
            }
            if (ind > 0)
                rez += "     signal InternalNet: std_logic_vector(0 to " + (ind - 1).ToString() + ");\n";
            for (i = 0; i < lineDescribe.Count; i++)
                rez += "     signal " + lineDescribe.Keys[i] + ": " + lineDescribe.Values[i].fullType + ";\n";
            rez += "begin\n";
            List<string> l1 = new List<string>();
            List<string> l2 = new List<string>();
            for (i = 0; i < lineDescribe.Count; i++)
            {
                if (!lineDescribe.Values[i].bus)
                {
                    lineDescribe.Values[i].LeftBusBound = 0;
                    lineDescribe.Values[i].RightBusBound = 0;
                }
                porting(lineDescribe.Keys[i], "InternalNet", lineAccess[lines.IndexOf(lineDescribe.Values[i])], 
                    lineDescribe.Values[i].LeftBusBound, lineDescribe.Values[i].RightBusBound, ref l1, ref l2);
                rez += "     " + lineDescribe.Keys[i] + " <= " + l2[0] + ";\n";
            }
            rez += "\n";
            SortedList <int, int> ElAssigned = new SortedList<int,int>();

            int UUU = 1;

            for (i = 0; i < elements.Count; i++)
            {
                if (elements[i].elementType == ElementType.Element)
                {
                    if (elements[i].ports.Count > 0)
                    {
                        rez += "     UUU" + (UUU++).ToString() + ": " + elements[i].name + " port map(\n";
                        for (j = 0; j < elements[i].ports.Count; j++)
                        {
                            if (j != 0)
                                rez += ",\n";
                            if (!elements[i].ports[j].isLine)
                                rez += "         " + elements[i].ports[j].fullName + " => " + "open";
                            else
                            {
                                pt = elements[i].ports[j];
                                ln = elements[i].ports[j].line;
                                indLine = lines.IndexOf(ln);
                                ElAssigned.Clear();
                                d = Math.Sign(pt.RightBusBound - pt.LeftBusBound);
                                if (d == 0 || !pt.bus)
                                {
                                    if (pt.bus)
                                        if (ln.parentBegin == pt)
                                        {
                                            if (ln.assign.ContainsKey(pt.LeftBusBound))
                                                ElAssigned.Add(0, lineAccess[indLine][ln.assign.IndexOfKey(pt.LeftBusBound)]);
                                        }
                                        else
                                        {
                                            if (ln.assign.ContainsValue(pt.LeftBusBound))
                                                ElAssigned.Add(0, lineAccess[indLine][ln.assign.IndexOfValue(pt.LeftBusBound)]);
                                        }
                                    else
                                        if (ln.parentBegin == pt)
                                        {
                                            if (ln.assign.ContainsKey(0))
                                                ElAssigned.Add(0, lineAccess[indLine][ln.assign.IndexOfKey(0)]);
                                        }
                                        else
                                        {
                                            if (ln.assign.ContainsValue(0))
                                                ElAssigned.Add(0, lineAccess[indLine][ln.assign.IndexOfValue(0)]);
                                        }
                                }
                                else
                                    if (ln.parentBegin == pt)
                                    {
                                        for (q = pt.LeftBusBound; q != pt.RightBusBound + d; q += d)
                                            if (ln.assign.ContainsKey(q))
                                                ElAssigned.Add(q, lineAccess[indLine][ln.assign.IndexOfKey(q)]);
                                    }
                                    else
                                    {
                                        for (q = pt.LeftBusBound; q != pt.RightBusBound + d; q += d)
                                            if (ln.assign.ContainsValue(q))
                                                ElAssigned.Add(q, lineAccess[indLine][ln.assign.IndexOfValue(q)]);
                                    }
                                porting(pt.name, "InternalNet", ElAssigned, pt.LeftBusBound, pt.RightBusBound, ref l1, ref l2);
                                if (!pt.bus)
                                    rez += "         " + pt.name + " => " + l2[0];
                                else
                                    for (q = 0; q < l1.Count; q++)
                                    {
                                        if (q != 0)
                                            rez += ",\n";
                                        rez += "         " + l1[q] + " => " + l2[q];
                                    }
                            }
                        }
                        rez += ");\n\n";
                    }
                }
            }

            SortedList<int, int> InPortAssign = new SortedList<int, int>();
            SortedList<int, int> OutPortAssign = new SortedList<int,int>();
            int min;
            int max;

            for (i = 0; i < elements.Count; i++)
            {
                if (elements[i].elementType == ElementType.ExternPort)
                {
                    if (elements[i].ports[0].isLine)                                
                    {
                        pt = elements[i].ports[0];
                        ln = elements[i].ports[0].line;
                        indLine = lines.IndexOf(ln);
                        InPortAssign.Clear();
                        OutPortAssign.Clear();
                        d = Math.Sign(pt.RightBusBound - pt.LeftBusBound);
                        if (d == 0 || !pt.bus)
                        {
                            if (pt.bus)
                                if (ln.parentBegin == pt)
                                {
                                    if (ln.assign.ContainsKey(pt.LeftBusBound))
                                        AddElPortAssign(pt.inout, lineAccess[indLine][ln.assign.IndexOfKey(pt.LeftBusBound)], InColor, OutColor,
                                            InOutColor, InOut2Color, ref InPortAssign, ref OutPortAssign, q);
                                }
                                else
                                {
                                    if (ln.assign.ContainsValue(pt.LeftBusBound))
                                        AddElPortAssign(pt.inout, lineAccess[indLine][ln.assign.IndexOfValue(pt.LeftBusBound)], InColor, OutColor,
                                            InOutColor, InOut2Color, ref InPortAssign, ref OutPortAssign, q);
                                }
                            else
                                if (ln.parentBegin == pt)
                                {
                                    if (ln.assign.ContainsKey(0))
                                        AddElPortAssign(pt.inout, lineAccess[indLine][ln.assign.IndexOfKey(0)], InColor, OutColor,
                                            InOutColor, InOut2Color, ref InPortAssign, ref OutPortAssign, q);
                                }
                                else
                                {
                                    if (ln.assign.ContainsValue(0))
                                        AddElPortAssign(pt.inout, lineAccess[indLine][ln.assign.IndexOfValue(0)], InColor, OutColor,
                                            InOutColor, InOut2Color, ref InPortAssign, ref OutPortAssign, q);
                                }
                        }
                        else
                            if (ln.parentBegin == pt)
                            {
                                for (q = pt.LeftBusBound; q != pt.RightBusBound + d; q += d)
                                    if (ln.assign.ContainsKey(q))
                                        AddElPortAssign(pt.inout, lineAccess[indLine][ln.assign.IndexOfKey(q)], InColor, OutColor,
                                            InOutColor, InOut2Color, ref InPortAssign, ref OutPortAssign, q);
                            }
                            else
                            {
                                for (q = pt.LeftBusBound; q != pt.RightBusBound + d; q += d)
                                    if (ln.assign.ContainsValue(q))
                                        AddElPortAssign(pt.inout, lineAccess[indLine][ln.assign.IndexOfValue(q)], InColor, OutColor,
                                            InOutColor, InOut2Color, ref InPortAssign, ref OutPortAssign, q);
                            }



                        min = int.MaxValue;
                        max = int.MinValue;
                        for (j = 0; j < InPortAssign.Count; j++)
                        {
                            if (InPortAssign.Keys[j] > max)
                                max = InPortAssign.Keys[j];
                            if (InPortAssign.Keys[j] < min)
                                min = InPortAssign.Keys[j];
                        }
                        if (InPortAssign.Count > 0)
                        {
                            porting("InternalNet", pt.name, InPortAssign, min, max, ref l1, ref l2);
                            if (!pt.bus)
                            {
                                if (l2[0] != "open")
                                    rez += "     " + l1[0] + " <= " + pt.name + ";\n";
                            }
                            else
                            {
                                for (q = 0; q < l1.Count; q++)
                                {
                                    if (l2[q] != "open")
                                    {
                                        rez += "     " + l1[q] + " <= " + l2[q] + ";\n";
                                    }
                                }
                            }
                        }

                        if (OutPortAssign.Count > 0)
                        {
                            porting(pt.name, "InternalNet", OutPortAssign, pt.LeftBusBound, pt.RightBusBound, ref l1, ref l2);
                            if (!pt.bus)
                            {
                                if (l2[0] != "open")
                                    rez += "     " + pt.name + " <= " + l2[0] + ";\n";
                            }
                            else
                            {
                                for (q = 0; q < l1.Count; q++)
                                {
                                    if (l2[q] != "open")
                                    {
                                        rez += "     " + l1[q] + " <= " + l2[q] + ";\n";
                                    }
                                }
                            }
                        }
                        if (InPortAssign.Count > 0 || OutPortAssign.Count > 0)
                            rez += "\n";
                    }
                }
            }

            rez += "end "+ EntityName + ";";
            return rez;
        }

        private void AddElPortAssign(portInOut inout, int color, List<int> InColor, List<int> OutColor, List<int> InOutColor, List<int> InOut2Color,
            ref SortedList<int, int> InAss, ref SortedList<int, int> OutAss, int key)
        {            
            if (inout == portInOut.In)
            {
                if (OutColor.Contains(color) || InOutColor.Contains(color))
                    if (!OutAss.ContainsKey(key))
                        OutAss.Add(key, color);
            }
            else if (inout == portInOut.Out)
            {
                if (InColor.Contains(color) || InOutColor.Contains(color))
                    if (!InAss.ContainsKey(color))
                        InAss.Add(color, key);
            }
            else if (inout == portInOut.InOut)
            {
                if (OutColor.Contains(color) || InOut2Color.Contains(color))
                    if (!OutAss.ContainsKey(key))
                        OutAss.Add(key, color);
                if (InColor.Contains(color) || InOut2Color.Contains(color))
                    if (!InAss.ContainsKey(color))
                        InAss.Add(color, key);
            }
        }
    }
}
