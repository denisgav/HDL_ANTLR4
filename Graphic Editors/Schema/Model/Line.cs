//#define build_logic //включение дополнительной логики (в процессе тестировани€)

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;

namespace csx
{
    public class Line : Object //лини€
    {
        public bool isSignal = false;
        private static int count = 0;
        private bool invis = false; //флаг невидимости
        public string name;
        public string fullType
        {
            get
            {
                if (!bus)
                    return "std_logic";
                else
                    return "std_logic_vector" + " (" + LeftBusBound + (RightBusBound > LeftBusBound ? " to " : " downto ") + RightBusBound + ")";
            }
        }
        public bool invisible //переменна€ возврата флага невидимости
        {
            get
            {
                return invis;
            }
        }
        private bool change = false; //флаг изменени€ невидимости
        public bool toChange //переменна€ возврата флага изменени€
        {
            get
            {
                return change;
            }
            set
            {
                change = value;
            }
        }
        //public int width = 1; //ширина (дл€ шины >1)
        public SortedList<int, int> assign;
        public bool bus = false; //флаг шины
        public int LeftBusBound = 0; //лева€ граница шины
        public int RightBusBound = 0; //права€ граница шины
        public PointF[] points; //точки трасировки
        public Port parentBegin; //начальный порт
        public Port parentEnd; //конечный порт

        public PointF begin //точка начала
        {
            get
            {
                return new PointF(parentBegin.location.X + parentBegin.parent.border.X,
                    parentBegin.location.Y + parentBegin.parent.border.Y);
            }
        }

        public PointF end //конец линии
        {
            get
            {
                return new PointF(parentEnd.location.X + parentEnd.parent.border.X,
                    parentEnd.location.Y + parentEnd.parent.border.Y);
            }
        }

        public Line(Port parentBegin, Port parentEnd)
        {
            this.name = "Net_" + count.ToString();
            count++;
            assign = new SortedList<int, int>();
            if ((parentBegin.parent.isElement && parentBegin.bus ||
                parentEnd.parent.isElement && parentEnd.bus) &&
                !((parentBegin.parent.isElement && parentEnd.parent.isElement) &&
                    (!parentBegin.bus || !parentEnd.bus)))
                this.bus = true;
            this.parentBegin = parentBegin;
            this.parentEnd = parentEnd;
        }

        public Line(Port parentBegin, Port parentEnd, bool isBus, int LeftBusBound, int RightBusBound)
        {
            this.name = "Net_" + count.ToString();
            count++;
            assign = new SortedList<int, int>();
            if ((parentBegin.parent.isElement && parentBegin.bus ||
                parentEnd.parent.isElement && parentEnd.bus) &&
                !((parentBegin.parent.isElement && parentEnd.parent.isElement) &&
                    (!parentBegin.bus || !parentEnd.bus)))
                this.bus = true;
            this.parentBegin = parentBegin;
            this.parentEnd = parentEnd;
            this.bus = isBus;
            this.LeftBusBound = LeftBusBound;
            this.RightBusBound = RightBusBound;
        }

        public Line(Port parentBegin, Port parentEnd, string name)
        {
            this.name = name;
            assign = new SortedList<int, int>();
            if ((parentBegin.parent.isElement && parentBegin.bus ||
                parentEnd.parent.isElement && parentEnd.bus) &&
                !((parentBegin.parent.isElement && parentEnd.parent.isElement) &&
                    (!parentBegin.bus || !parentEnd.bus)))
                this.bus = true;
            this.parentBegin = parentBegin;
            this.parentEnd = parentEnd;
        }

        public Line(Port parentBegin, Port parentEnd, bool isBus, int LeftBusBound, int RightBusBound, string name)
        {
            this.name = name;
            assign = new SortedList<int, int>();
            if ((parentBegin.parent.isElement && parentBegin.bus ||
                parentEnd.parent.isElement && parentEnd.bus) &&
                !((parentBegin.parent.isElement && parentEnd.parent.isElement) &&
                    (!parentBegin.bus || !parentEnd.bus)))
                this.bus = true;
            this.parentBegin = parentBegin;
            this.parentEnd = parentEnd;
            this.bus = isBus;
            this.LeftBusBound = LeftBusBound;
            this.RightBusBound = RightBusBound;
        }

        public void setToChange()
        {
            change = true;
        }

        public void changeInvisible(bool value)
        {
            change = false;
            invis = value;
        }        

        public void simpling(int connectDistance) //удаление лишних точек, сглаживание изломов мешьших размером чем connectDistance
        {
            if (points.Length < 4)
                return;
            ArrayList newp = new ArrayList();
            int i;
            newp.Add(0);
            for (i = 1; i < points.Length; i++)
                if (points[i].X != points[(int)newp[newp.Count - 1]].X || points[i].Y != points[(int)newp[newp.Count - 1]].Y)
                    newp.Add(i);
            for (i = 1; i < newp.Count - 1; i++)
            {
                if ((points[(int)newp[i]].X == points[(int)newp[i - 1]].X && points[(int)newp[i]].X == points[(int)newp[i + 1]].X) ||
                    (points[(int)newp[i]].Y == points[(int)newp[i - 1]].Y && points[(int)newp[i]].Y == points[(int)newp[i + 1]].Y))
                {
                    newp.RemoveAt(i);
                    i--;
                }
            }
            if (points[(int)newp[0]].X == points[(int)newp[1]].X)
            {
                if (Math.Abs(points[(int)newp[0]].Y - points[(int)newp[1]].Y) <= connectDistance)
                {
                    points[(int)newp[2]].Y = points[(int)newp[0]].Y;
                    newp.RemoveAt(1);
                }
            }
            else
                if (Math.Abs(points[(int)newp[0]].X - points[(int)newp[1]].X) <= connectDistance)
                {
                    points[(int)newp[2]].X = points[(int)newp[0]].X;
                    newp.RemoveAt(1);
                }
            for (i = 1; i < newp.Count - 4; i++)
            {
                if (points[(int)newp[i]].X == points[(int)newp[i + 1]].X)
                {
                    if (Math.Abs(points[(int)newp[i]].Y - points[(int)newp[i + 1]].Y) <= connectDistance)
                    {
                        points[(int)newp[i + 2]].Y = points[(int)newp[i]].Y;
                        newp.RemoveRange(i, 2);
                    }
                }
                else
                    if (Math.Abs(points[(int)newp[i]].X - points[(int)newp[i + 1]].X) <= connectDistance)
                    {
                        points[(int)newp[i + 2]].X = points[(int)newp[i]].X;
                        newp.RemoveRange(i, 2);
                    }
            }
            if (newp.Count > 3)
                if (points[(int)newp[newp.Count - 1]].X == points[(int)newp[newp.Count - 2]].X)
                {
                    if (Math.Abs(points[(int)newp[newp.Count - 1]].Y - points[(int)newp[newp.Count - 2]].Y) <= connectDistance)
                    {
                        points[(int)newp[newp.Count - 3]].Y = points[(int)newp[newp.Count - 1]].Y;
                        newp.RemoveAt(newp.Count - 2);
                    }
                }
                else
                    if (Math.Abs(points[(int)newp[newp.Count - 1]].X - points[(int)newp[newp.Count - 2]].X) <= connectDistance)
                    {
                        points[(int)newp[newp.Count - 3]].X = points[(int)newp[newp.Count - 1]].X;
                        newp.RemoveAt(newp.Count - 2);
                    }
            if (newp.Count > 4)
                if (points[(int)newp[newp.Count - 2]].X == points[(int)newp[newp.Count - 3]].X)
                {
                    if (Math.Abs(points[(int)newp[newp.Count - 2]].Y - points[(int)newp[newp.Count - 3]].Y) <= connectDistance)
                    {
                        points[(int)newp[newp.Count - 4]].Y = points[(int)newp[newp.Count - 2]].Y;
                        newp.RemoveRange(newp.Count - 3, 2);
                    }
                }
                else
                    if (Math.Abs(points[(int)newp[newp.Count - 2]].X - points[(int)newp[newp.Count - 3]].X) <= connectDistance)
                    {
                        points[(int)newp[newp.Count - 4]].X = points[(int)newp[newp.Count - 2]].X;
                        newp.RemoveRange(newp.Count - 3, 2);
                    }
            PointF[] p = points;
            points = new PointF[newp.Count];
            for (i = 0; i < newp.Count; i++)
                points[i] = p[(int)newp[i]];
        }

        private void testRect(RectangleF rct, ref long[,] matrix, ref float[] horW, ref float[] vertW, int horCnt, int vertCnt, int mark)
        {
            rct.X -= Construct.interval - 1;
            rct.Y -= Construct.interval - 1;
            rct.Width += 2 * Construct.interval - 2;
            rct.Height += 2 * Construct.interval - 2;
            int a, b, c = 0;
            int iL, iR, iT, iB;
            a = 1;
            b = horCnt - 2;
            while (Math.Abs(a - b) > 1)
            {
                c = (int)(a + b) / 2;
                if (horW[c] >= rct.Left)
                    b = c;
                else
                    a = c;
            }
            if (Math.Abs(a - b) == 1)
                iL = b;
            else
                iL = c;
            a = 1;
            b = horCnt - 2;
            while (Math.Abs(a - b) > 1)
            {
                c = (int)(a + b) / 2;
                if (horW[c] <= rct.Right)
                    a = c;
                else
                    b = c;
            }
            if (Math.Abs(a - b) == 1)
                iR = a;
            else
                iR = c;
            a = 1;
            b = vertCnt - 2;
            while (Math.Abs(a - b) > 1)
            {
                c = (int)(a + b) / 2;
                if (vertW[c] >= rct.Top)
                    b = c;
                else
                    a = c;
            }
            if (Math.Abs(a - b) == 1)
                iT = b;
            else
                iT = c;
            a = 1;
            b = vertCnt - 2;
            while (Math.Abs(a - b) > 1)
            {
                c = (int)(a + b) / 2;
                if (vertW[c] <= rct.Bottom)
                    a = c;
                else
                    b = c;
            }
            if (Math.Abs(a - b) == 1)
                iB = a;
            else
                iB = c;
            int i, j;
            for (i = iL; i <= iR; i++)
                for (j = iT; j <= iB; j++)
                    matrix[i, j] = mark;
        }

        private void matrixWave(Stack<Point> start, ref long[,] matrix, ref float[] horW, ref float[] vertW, ref int horCnt, ref int vertCnt)
        {
            Stack<Point> st1 = start;
            Stack<Point> st2 = new Stack<Point>();
            Point pt;
            int ind = 1, i;
            while (st1.Count > 0)
            {
                do
                {
                    pt = st1.Pop();
                    i = pt.X - 1;
                    while (matrix[i, pt.Y] == -3 || matrix[i, pt.Y] == -1 || matrix[i, pt.Y] == ind)
                    {
                        if (matrix[i, pt.Y] == -1 && matrix[i, pt.Y] != ind)
                        {
                            matrix[i, pt.Y] = ind;
                            st2.Push(new Point(i, pt.Y));
                        }
                        i--;
                    }
                    i = pt.X + 1;
                    while (matrix[i, pt.Y] == -3 || matrix[i, pt.Y] == -1 || matrix[i, pt.Y] == ind)
                    {
                        if (matrix[i, pt.Y] == -1 && matrix[i, pt.Y] != ind)
                        {
                            matrix[i, pt.Y] = ind;
                            st2.Push(new Point(i, pt.Y));
                        }
                        i++;
                    }
                    i = pt.Y - 1;
                    while (matrix[pt.X, i] == -4 || matrix[pt.X, i] == -1 || matrix[pt.X, i] == ind)
                    {
                        if (matrix[pt.X, i] == -1 && matrix[pt.X, i] != ind)
                        {
                            matrix[pt.X, i] = ind;
                            st2.Push(new Point(pt.X, i));
                        }
                        i--;
                    }
                    i = pt.Y + 1;
                    while (matrix[pt.X, i] == -4 || matrix[pt.X, i] == -1 || matrix[pt.X, i] == ind)
                    {
                        if (matrix[pt.X, i] == -1 && matrix[pt.X, i] != ind)
                        {
                            matrix[pt.X, i] = ind;
                            st2.Push(new Point(pt.X, i));
                        }
                        i++;
                    }
                } while (st1.Count > 0);
                st1 = st2;
                st2 = new Stack<Point>();
                ind++;
            }
        }

        private void passPointF(int x, int y, int xe, int ye, ref long[,] matrix, ref float[] horW, ref float[] vertW, ref ArrayList ptFm)
        {
            ptFm.Clear();
            ptFm.Add(new PointF(horW[x], vertW[y]));
            int dx, dy, i;
            long ind;
            while (true)
            {
                ind = matrix[x, y];
                dx = Math.Sign(xe - x);
                dy = Math.Sign(ye - y);
                if (dx != 0)
                {
                    i = x + dx;
                    while (matrix[i, y] == ind || matrix[i, y] == -3)
                        i += dx;
                    if (matrix[i, y] == ind - 1)
                    {
                        x = i;
                        goto findm;
                    }
                }
                if (dy != 0)
                {
                    i = y + dy;
                    while (matrix[x, i] == ind || matrix[x, i] == -4)
                        i += dy;
                    if (matrix[x, i] == ind - 1)
                    {
                        y = i;
                        goto findm;
                    }
                }
                if (dx != 0)
                {
                    i = x - dx;
                    while (matrix[i, y] == ind || matrix[i, y] == -3)
                        i -= dx;
                    if (matrix[i, y] == ind - 1)
                    {
                        x = i;
                        goto findm;
                    }
                }
                if (dy != 0)
                {
                    i = y - dy;
                    while (matrix[x, i] == ind || matrix[x, i] == -4)
                        i -= dy;
                    if (matrix[x, i] == ind - 1)
                    {
                        y = i;
                        goto findm;
                    }
                }
                if (dx == 0)
                    for (dx = -1; dx <= 1; dx += 2)
                    {
                        i = x + dx;
                        while (matrix[i, y] == ind || matrix[i, y] == -3)
                            i += dx;
                        if (matrix[i, y] == ind - 1)
                        {
                            x = i;
                            goto findm;
                        }
                        i = x - dx;
                        while (matrix[i, y] == ind || matrix[i, y] == -3)
                            i -= dx;
                        if (matrix[i, y] == ind - 1)
                        {
                            x = i;
                            goto findm;
                        }
                    }
                if (dy == 0)
                    for (dy = -1; dy <= 1; dy += 2)
                    {
                        i = y + dy;
                        while (matrix[x, i] == ind || matrix[x, i] == -4)
                            i += dy;
                        if (matrix[x, i] == ind - 1)
                        {
                            y = i;
                            goto findm;
                        }
                        i = y - dy;
                        while (matrix[x, i] == ind || matrix[x, i] == -4)
                            i -= dy;
                        if (matrix[x, i] == ind - 1)
                        {
                            y = i;
                            goto findm;
                        }
                    }
                break;
            findm:
                ptFm.Add(new PointF(horW[x], vertW[y]));
            }
        }

        public void PassFinding() //поиск пути прокладки линии между начальным и конечным портами
        {
            int horCnt = parentEnd.parent.parent.horGrid.Count + 4;
            int vertCnt = parentEnd.parent.parent.vertGrid.Count + 4;
            float[] horW = new float[horCnt];
            float[] vertW = new float[vertCnt];
            long[,] matrix = new long[horCnt, vertCnt];
            int i, j;
            float buf;
            int iHB = 0, iVB = 0, iHE = 0, iVE = 0; //координаты начала и конца линии (горизонталь и вертикаль)    
            //PointF bP = parentBegin.location;
            //PointF eP = parentEnd.location;
            PointF bP = parentBegin.getDistance();
            PointF eP = parentEnd.getDistance();
            bool sorted = false;
            if (eP.X < bP.X)
            {
                sorted = true;
                buf = eP.X;
                eP.X = bP.X;
                bP.X = buf;
                buf = eP.Y;
                eP.Y = bP.Y;
                bP.Y = buf;
            }
            #region ‘ормирование сетки
            horW[1] = bP.X;
            horW[2] = eP.X;
            i = 0;
            foreach (KeyValuePair<float, int> ind in parentEnd.parent.parent.horGrid)
                horW[i++ + 3] = ind.Key;
            vertW[1] = bP.Y;
            vertW[2] = eP.Y;
            i = 0;
            foreach (KeyValuePair<float, int> ind in parentEnd.parent.parent.vertGrid)
                vertW[i++ + 3] = ind.Key;
            for (i = 2; i < horCnt - 1; i++)
                for (j = i; j > 1; j--)
                    if (horW[j] < horW[j - 1])
                    {
                        buf = horW[j];
                        horW[j] = horW[j - 1];
                        horW[j - 1] = buf;
                    }
                    else
                        break;
            for (i = 2; i < vertCnt - 1; i++)
                for (j = i; j > 1; j--)
                    if (vertW[j] < vertW[j - 1])
                    {
                        buf = vertW[j];
                        vertW[j] = vertW[j - 1];
                        vertW[j - 1] = buf;
                    }
            for (i = 1; i < horCnt - 1; i++)
            {
                if (horW[i] == bP.X)
                    iHB = i;
                if (horW[i] == eP.X)
                    iHE = i;
            }
            for (i = 1; i < vertCnt - 1; i++)
            {
                if (vertW[i] == bP.Y)
                    iVB = i;
                if (vertW[i] == eP.Y)
                    iVE = i;
            }
            horW[0] = 0;
            vertW[0] = 0;
            horW[horCnt - 1] = horW[horCnt - 2];
            vertW[vertCnt - 1] = vertW[vertCnt - 2];
            #endregion
            #region ѕостроение матрицы
            PointF p1, p2;
            for (i = horCnt - 2; i > 0; i--)
                for (j = vertCnt - 2; j > 0; j--)
                    matrix[i, j] = -1;
            for (i = 0; i < horCnt; i++)
            {
                matrix[i, 0] = -2;
                matrix[i, vertCnt - 1] = -2;
            }
            for (i = vertCnt - 2; i > 0; i--)
            {
                matrix[0, i] = -2;
                matrix[horCnt - 1, i] = -2;
            }
            for (i = 0; i < parentBegin.parent.parent.ElementsCount; i++)
                testRect(parentBegin.parent.parent.ReturnElement(i).border, ref matrix, ref horW, ref vertW, horCnt, vertCnt, -2);
            RectangleF rct;
            for (i = 0; i < parentBegin.parent.parent.ConnectsCount; i++)
            {
                rct = parentBegin.parent.parent.ReturnConnect(i).border;
                testRect(new RectangleF(rct.X, rct.Y, 0, 0), ref matrix, ref horW, ref vertW, horCnt, vertCnt, -2);
            }
            Line ln;
            for (i = 0; i < parentBegin.parent.parent.LinesCount; i++)
            {
                ln = parentBegin.parent.parent.ReturnLine(i);
                if (ln.points != null && !ln.invisible)
                {
                    p2 = ln.points[0];
                    for (j = 1; j < ln.points.Length; j++)
                    {
                        p1 = p2;
                        p2 = ln.points[j];
                        if (p1.X == p2.X && Math.Abs(p1.Y - p2.Y) > 2 * Construct.interval)
                            testRect(new RectangleF(p1.X, p1.Y < p2.Y ? p1.Y + Construct.interval : p2.Y + Construct.interval, 0,
                                Math.Abs(p1.Y - p2.Y) - 2 * Construct.interval), ref matrix, ref horW, ref vertW, horCnt, vertCnt, -3);
                        if (p1.Y == p2.Y && Math.Abs(p1.X - p2.X) > 2 * Construct.interval)
                            testRect(new RectangleF(p1.X < p2.X ? p1.X + Construct.interval : p2.X + Construct.interval, p1.Y,
                                Math.Abs(p1.X - p2.X) - 2 * Construct.interval, 0), ref matrix, ref horW, ref vertW, horCnt, vertCnt, -4);                        
                    }
                    //p1 = ln.points[0];
                    //p2 = ln.points[ln.points.Length - 1];
                    for (j = 1; j < ln.points.Length - 1; j++)
                    {
                        //p3 = ln.points[j];
                        p1 = ln.points[j];
                        //if ((p1.LineNumber != p3.LineNumber || p1.ColumnNumber != p3.ColumnNumber) && (p2.LineNumber != p3.LineNumber || p2.ColumnNumber != p3.ColumnNumber))
                            testRect(new RectangleF(p1.X, p1.Y, 0, 0), ref matrix, ref horW, ref vertW, horCnt, vertCnt, -2);
                    }
                    //testRect(new Rectangle(p2.LineNumber, p2.ColumnNumber, 0, 0), ref matrix, ref horW, ref vertW, horCnt, vertCnt, -2);
                }
            }
            //int[] ddx = { -1, 0, 1, 0};
            //int[] ddy = { 0, -1, 0, 1};
            //if (sorted)
            //{
            //    while (matrix[iHE, iVE] != -1)
            //    {
            //        iHE += ddx[parentBegin.napr];
            //        iVE += ddy[parentBegin.napr];
            //    }
            //    while (matrix[iHB, iVB] != -1)
            //    {
            //        iHB += ddx[parentEnd.napr];
            //        iVB += ddy[parentEnd.napr];
            //    }
            //}
            //else
            //{
            //    while (matrix[iHB, iVB] != -1)
            //    {
            //        iHE += ddx[parentEnd.napr];
            //        iVE += ddy[parentEnd.napr];
            //    }
            //    while (matrix[iHB, iVB] != -1)
            //    {
            //        iHB += ddx[parentBegin.napr];
            //        iVB += ddy[parentBegin.napr];
            //    }
            //}
            #endregion
            #region ѕрохождение волны
            //if ((sorted && (parentEnd.napr == 0 || parentEnd.napr == 2)) || (!sorted && (parentBegin.napr == 0 || parentBegin.napr == 2)))
            //{
            //    matrix[iHB, iVB - 1] = -2;
            //    matrix[iHB, iVB + 1] = -2;
            //}
            //if ((sorted && (parentEnd.napr == 1 || parentEnd.napr == 3)) || (!sorted && (parentBegin.napr == 1 || parentBegin.napr == 3)))
            //{
            //    matrix[iHB - 1, iVB] = -2;
            //    matrix[iHB + 1, iVB] = -2;
            //}
            matrix[iHB, iVB] = -1;
            //matrix[iHE, iVE] = 0;
            Stack<Point> st = new Stack<Point>();
            switch (sorted ? parentBegin.napr : parentEnd.napr)
            {
                case 0:
                    i = iHE;
                    while (matrix[i, iVE] == -1 || matrix[i, iVE] == -3)
                    {
                        if (matrix[i, iVE] == -1)
                        {
                            matrix[i, iVE] = 0;
                            st.Push(new Point(i, iVE));
                        }
                        i++;
                    }
                    break;
                case 1:
                    i = iVE;
                    while (matrix[iHE, i] == -1 || matrix[iHE, i] == -3)
                    {
                        if (matrix[iHE, i] == -1)
                        {
                            matrix[iHE, i] = 0;
                            st.Push(new Point(iHE, i));
                        }
                        i++;
                    }
                    break;
                case 2:
                    i = iHE;
                    while (matrix[i, iVE] == -1 || matrix[i, iVE] == -3)
                    {
                        if (matrix[i, iVE] == -1)
                        {
                            matrix[i, iVE] = 0;
                            st.Push(new Point(i, iVE));
                        }
                        i--;
                    }
                    break;
                case 3:
                    i = iVE;
                    while (matrix[iHE, i] == -1 || matrix[iHE, i] == -3)
                    {
                        if (matrix[iHE, i] == -1)
                        {
                            matrix[iHE, i] = 0;
                            st.Push(new Point(iHE, i));
                        }
                        i--;
                    }
                    break;
            }
            matrixWave(st, ref matrix, ref horW, ref vertW, ref horCnt, ref vertCnt);
            #endregion
            #region ‘ормирование массива точек
            //if (matrix[iHB, iVB] != -1)
            //if (true)
            //{
                if ((sorted && (parentEnd.napr == 0 || parentEnd.napr == 2)) || (!sorted && (parentBegin.napr == 0 || parentBegin.napr == 2)))
                {
                    //matrix[iHB, iVB - 1] = -4;
                    //matrix[iHB, iVB + 1] = -4;
                    #region ≈сли не найден путь
                    if (matrix[iHB, iVB] == -1)
                    {
                        long fm = -1, min = -1;
                        i = iVB - 1;
                        while (matrix[iHB, i] == -4)
                            i--;
                        if (matrix[iHB, i] >= 0)
                        {
                            fm = matrix[iHB, i];
                            min = fm + 1;
                            i--;
                            while (matrix[iHB, i] == -4 || matrix[iHB, i] == fm)
                                i--;
                            if (matrix[iHB, i] >= 0)
                            {
                                if (matrix[iHB, i] == fm - 1)
                                    min = fm;
                            }
                        }
                        i = iVB + 1;
                        while (matrix[iHB, i] == -4)
                            i++;
                        if (matrix[iHB, i] >= 0)
                        {
                            fm = matrix[iHB, i];
                            if (min > fm + 1 || min < 0)
                                min = fm + 1;
                            i++;
                            while (matrix[iHB, i] == -4 || matrix[iHB, i] == fm)
                                i++;
                            if (matrix[iHB, i] >= 0)
                            {
                                if (matrix[iHB, i] == fm - 1)
                                    if (fm < min || min < 0)
                                        min = fm;
                            }
                        }
                        if (min >= 0)
                        {
                            matrix[iHB, iVB] = min;
                            //matrix[iHB, iVB - 1] = min;
                            //matrix[iHB, iVB + 1] = min;
                        }
                        else
                        {
                            i = iVB - 1;
                            fm = -1;
                            while (matrix[iHB, i] < 0)
                            {
                                i--;
                                if (i < 0)
                                {
                                    i = 0;
                                    break;
                                }
                            }
                            if (matrix[iHB, i] >= 0)
                                fm = i;
                            i = iVB + 1;
                            while (matrix[iHB, i] < 0 && i < vertCnt)
                            {
                                i++;
                                if (i >= vertCnt)
                                {
                                    i--;
                                    break;
                                }
                            }
                            if (matrix[iHB, i] >= 0)
                            {
                                if (fm < 0)
                                    fm = i;
                                else
                                {
                                    if (matrix[iHB, fm] > matrix[iHB, i] || fm < 0)
                                        fm = i;
                                }
                            }
                            if (fm >= 0)
                                for (i = iVB; i != fm; i += Math.Sign(fm - iVB))
                                    matrix[iHB, i] = matrix[iHB, fm];
                        }
                    }
                    #endregion;
                }
                if ((sorted && (parentEnd.napr == 1 || parentEnd.napr == 3)) || (!sorted && (parentBegin.napr == 1 || parentBegin.napr == 3)))
                {
                    //matrix[iHB - 1, iVB] = -3;
                    //matrix[iHB + 1, iVB] = -3;
                    #region ≈сли не найден путь
                    if (matrix[iHB, iVB] == -1)
                    {
                        long fm = -1, min = -1;
                        i = iHB - 1;
                        while (matrix[i, iVB] == -3)
                            i--;
                        if (matrix[i, iVB] >= 0)
                        {
                            fm = matrix[i, iVB];
                            min = fm;
                            i--;
                            while (matrix[i, iVB] == -3 || matrix[i, iVB] == fm)
                                i--;
                            if (matrix[i, iVB] >= 0)
                            {
                                if (matrix[i, iVB] == fm - 1)
                                    min = fm;
                            }
                        }
                        i = iHB + 1;
                        while (matrix[i, iVB] == -3)
                            i++;
                        if (matrix[i, iVB] >= 0)
                        {
                            fm = matrix[i, iVB];
                            if (fm + 1 < min || min < 0)
                                min = fm + 1;
                            i++;
                            while (matrix[i, iVB] == -3 || matrix[i, iVB] == fm)
                                i++;
                            if (matrix[i, iVB] >= 0)
                            {
                                if (matrix[i, iVB] == fm - 1)
                                    if (fm < min || min < 0)
                                        min = fm;
                            }
                        }
                        if (min >= 0)
                            matrix[iHB, iVB] = min;
                        else
                        {
                            i = iHB - 1;
                            fm = -1;
                            while (matrix[i, iVB] < 0)
                            {
                                i--;
                                if (i < 0)
                                {
                                    i = 0;
                                    break;
                                }
                            }
                            if (matrix[i, iVB] >= 0)
                                fm = i;
                            i = iHB + 1;
                            while (matrix[i, iVB] < 0 && i < horCnt)
                            {
                                i++;
                                if (i >= horCnt)
                                {
                                    i--;
                                    break;
                                }
                            }
                            if (matrix[i, iVB] >= 0)
                            {
                                if (fm < 0)
                                    fm = i;
                                else
                                {
                                    if (matrix[fm, iVB] > matrix[i, iVB] || fm < 0)
                                        fm = i;
                                }
                            }
                            if (fm >= 0)
                                for (i = iHB; i != fm; i += Math.Sign(fm - iHB))
                                    matrix[i, iVB] = matrix[fm, iVB];
                        }
                    }
                    #endregion;
                }
                ArrayList ptFm = new ArrayList();
                passPointF(iHB, iVB, iHE, iVE, ref matrix, ref horW, ref vertW, ref ptFm);

                points = new PointF[ptFm.Count + 2];
                if (!sorted)
                    points[0] = begin;
                else
                    points[0] = end;
                for (i = 0; i < ptFm.Count; i++)
                    points[i + 1] = (PointF)ptFm[i];
                if (!sorted)
                    points[ptFm.Count + 1] = end;
                else
                    points[ptFm.Count + 1] = begin;
            //}
            //else
            //{
            //    points = new PointF[3];
            //    points[0] = new PointF(horW[iHB], vertW[iVB]);
            //    points[1] = new PointF(horW[iHE], vertW[iVB]);
            //    points[2] = new PointF(horW[iHE], vertW[iVE]);
            //}
            #endregion
            //parentBegin.parent.parent.events.horp = horW;
            //parentBegin.parent.parent.events.verp = vertW;
            //parentBegin.parent.parent.events.mat = matrix;
            //parentBegin.parent.parent.events.hcnt = horCnt;
            //parentBegin.parent.parent.events.vcnt = vertCnt;
            simpling(Events.connectDistance);
        }

        public bool checkSegment(PointF a1, PointF a2, PointF b, ref PointF c) //проверка пересечени€ отрезка a1-a2 точкой b
        //в случае пересечени€ выдаетс€ максимально возможный отрезок, не конфликтующий с точкой b (a1-c)
        {
            if (a1.Y == a2.Y)
                if (b.Y > a1.Y - Construct.interval && b.Y < a1.Y + Construct.interval)
                    if ((b.X > a1.X - Construct.interval && b.X < a2.X + Construct.interval) ||
                        (b.X > a2.X - Construct.interval && b.X < a1.X + Construct.interval))
                    {
                        c = new PointF(b.X - Math.Sign(a2.X - a1.X) * Construct.interval, a1.Y);
                        return false;
                    }
                    else
                        return true;
                else
                    return true;
            else
                if (b.X > a1.X - Construct.interval && b.X < a1.X + Construct.interval)
                    if ((b.Y > a1.Y - Construct.interval && b.Y < a2.Y + Construct.interval) ||
                        (b.Y > a2.Y - Construct.interval && b.Y < a1.Y + Construct.interval))
                    {
                        c = new PointF(a1.X, b.Y - Math.Sign(a2.Y - a1.Y) * Construct.interval);
                        return false;
                    }
                    else
                        return true;
                else
                    return true;
        }
        
        public void Draw(Graphics e)
        {
            if (!invisible && points != null)
                if (bus)
                    e.DrawLines(new Pen(Color.Black, 3), points);
                else
                    e.DrawLines(new Pen(Color.Black, 1), points);
        }

        //public bool Equals(Line ln)
        //{
        //    if (this.name != ln.name)
        //        return false;
        //    if (this.parentBegin != ln.parentBegin)
        //        return false;
        //    if (this.parentEnd != ln.parentEnd)
        //        return false;
        //    if (this.bus != ln.bus)
        //        return false;
        //    if (this.LeftBusBound != ln.LeftBusBound)
        //        return false;
        //    if (this.RightBusBound != ln.RightBusBound)
        //        return false;
        //    return true;
        //} 
    }
}