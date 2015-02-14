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
    public class My_Figure
    {
        public int zIndex;
        [NonSerialized]
        protected EntityDrawningCore core;
        public EntityDrawningCore Core
        {
            get { return core; }
            set { core = value; }
        }

        public My_Brush brush;
        public My_Pen pen;

        public virtual Point CenterPoint
        {
            get { return new Point(0, 0); }
            set { }
        }

        public My_Figure(EntityDrawningCore core)
            :this()
        {
            this.core = core;
        }

        public My_Figure()
        {
            points = new Point[0];
            Selected = false;
            zIndex = 1;
            pen = new My_Pen(Color.Black);
            brush = new My_SolidBrush(Color.Beige);
        }

        public My_Figure(My_Figure item)
        {
            this.core = item.core;
            this.selected = item.selected;
            this.zIndex = item.zIndex;
            this.pen = item.pen;
            this.brush = item.brush;
            points = (Point[])item.points.Clone();
        }

        private Point[] points;
        public virtual Point[] Points
        {
            get
            {
                return points;
            }
        }

        private bool Selected;
        public bool selected
        {
            get
            {
                return Selected;
            }
            set
            {
                if (value == false)
                {
                    if (core == null)
                    {
                        Selected = false;
                        return;
                    }
                    if(core.SelectedFigures.Contains(this) == false)
                        Selected = false;
                }
                else
                    Selected = true;
            }
        }

        //Делегаты - Без них никак :)
        public delegate void MouseMoveDelegate(object sender, MouseEventArgs e);
        public MouseMoveDelegate mouse_move;
        public delegate void MouseUpDelegate(object sender, MouseEventArgs e);
        public MouseUpDelegate mouse_up;
        public delegate void MouseDownDelegate(object sender, MouseEventArgs e);
        public MouseDownDelegate mouse_down;
        public delegate void DrawDelegate(object sender, PaintEventArgs e);
        public DrawDelegate draw;

        protected virtual void Draw(object sender, PaintEventArgs e)
        { }
        protected virtual void MouseMove(object sender, MouseEventArgs e)
        { }
        protected virtual void MouseMoveCreateNew(object sender, MouseEventArgs e)
        { }
        protected virtual void MouseDown(object sender, MouseEventArgs e)
        { }
        protected virtual void MouseDownCreateNew(object sender, MouseEventArgs e)
        { }
        protected virtual void MouseUp(object sende, MouseEventArgs e)
        {
            brush.UpdateBrush(this);
        }
        protected virtual void MouseUpCreateNew(object sende, MouseEventArgs e)
        { }

        public virtual bool IsSelected(Point pt)
        {
            Point[] points = Points;
            foreach (Point p in points)
                if ((Math.Abs(pt.X - p.X) <= 2) && (Math.Abs(pt.Y - p.Y) <= 2)) // вправо и вниз
                {
                    return true;
                }
            return false;
        }

        public virtual bool IsSelected(Rectangle rt)
        {
            return false;
        }

        public static float RoundAngle(float angle) // используется для округления угла
        {
            float res = angle;
            if (((res % 30.0f) < 10.0f) || ((res % 30.0f) > 20.0f))
            {
                if ((res % 30.0f) < 10.0f)
                    res -= res % 30.0f;
                else
                    res += 30.0f - res % 30.0f;
            }
            else
            {
                if (res % 45.0f < 22.5f)
                {
                    res -= res % 45.0f;
                }
                else
                {
                    res += 45.0f - (res % 45.0f);
                }
            }
            return res;
        }

        public virtual void AddToGraphicsPath(System.Drawing.Drawing2D.GraphicsPath path)
        {
        }
    }
}