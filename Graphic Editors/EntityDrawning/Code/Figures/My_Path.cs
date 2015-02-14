using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;

namespace Schematix.EntityDrawning
{
    [Serializable]
    public class My_Path : My_Figure
    {
        public List<My_Figure> Figures;

        public FillMode fillMode = FillMode.Winding;

        private My_Figure selected_figure;

        GraphicsPath path
        {
            get
            {
                GraphicsPath res = new GraphicsPath();
                res.FillMode = fillMode;
                res.StartFigure();
                foreach (My_Figure fig in Figures)
                {
                    fig.AddToGraphicsPath(res);
                }
                res.CloseFigure();
                return res;
            }
        }

        public override Point[] Points
        {
            get
            {
                GraphicsPath p = path;
                Point[] res = new Point[p.PointCount];
                if (p.PointCount != 0)
                {
                    for (int i = 0; i < res.Length; i++)
                    {
                        res[i].X = (int)p.PathPoints[i].X;
                        res[i].Y = (int)p.PathPoints[i].Y;
                    }
                }
                return res;
            }
        }

        public override Point CenterPoint
        {
            get
            {
                RectangleF rect = path.GetBounds();
                Point pt = new Point();
                pt.X = (int)rect.Location.X + ((int)rect.Width / 2);
                pt.Y = (int)rect.Location.Y + ((int)rect.Height / 2);
                return pt;
            }
            set
            {
                base.CenterPoint = value;
            }
        }

        public My_Path(EntityDrawningCore core)
            : base(core)
        {
            this.Figures = new List<My_Figure>();

            zIndex = -1;

            mouse_move = MouseMoveCreateNew;
            mouse_down = MouseDownCreateNew;
            mouse_up = MouseUpCreateNew;
            draw = Draw;
        }

        public My_Path(My_Path item)
            : base(item)
        {
            this.Figures = new List<My_Figure>();

            zIndex = -1;

            mouse_move = MouseMove;
            mouse_down = MouseDown;
            mouse_up = MouseUp;
            draw = Draw;
        }

        public void Fix()
        {
            draw = Draw;
            mouse_move = MouseMove;
            mouse_down = MouseDown;
            mouse_up = MouseUp;

            core.Lock = false;
            core.Picture.Sort();

            //core.AddToHistory();
            core.Form.Invalidate();
        }

        public override void AddToGraphicsPath(System.Drawing.Drawing2D.GraphicsPath path)
        {
            path.AddPath(this.path, false);
        }

        private new void Draw(object sender, PaintEventArgs e)
        {
            Graphics dc = e.Graphics;
            if (selected == false)
            {
                GraphicsPath p = path;
                dc.FillPath(brush, p);
                dc.DrawPath(pen, p);
            }
            else
            {
                GraphicsPath p = path;
                Brush brush2 = new HatchBrush(HatchStyle.LargeCheckerBoard, EntityDrawningCore.SelectedColor, Color.Empty);
                Pen pen2 = new Pen(brush2);
                dc.FillPath(brush2, p);
                dc.DrawPath(pen2, p);
            }
        }

        private new void MouseMoveCreateNew(object sender, MouseEventArgs e)
        {
            My_Figure fig = core.Picture.GetSelectedFigure(new Point(e.X, e.Y));
            if (((selected_figure == null) || (selected_figure != fig)) && (fig != null))
            {
                if (selected_figure != null)
                    selected_figure.selected = false;
                selected_figure = fig;
                selected_figure.selected = true;
                core.Form.Invalidate();
            }
            else
            {
                if (selected_figure != null)
                {
                    if (selected_figure == fig)
                        return;

                    selected_figure.selected = false;
                    selected_figure = null;
                    core.Form.Invalidate();
                }
            }
        }

        private new void MouseMove(object sender, MouseEventArgs e)
        {
            foreach (My_Figure fig in Figures)
                fig.mouse_move(sender, e);
        }

        private new void MouseUp(object sender, MouseEventArgs e)
        {
            foreach (My_Figure fig in Figures)
                fig.mouse_up(sender, e);
            brush.UpdateBrush(this);
        }

        private new void MouseUpCreateNew(object sende, MouseEventArgs e)
        {
            core.Lock = true;
            core.Form.Invalidate();
        }

        private new void MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                core.Form.contextMenuStripPath.Show(Control.MousePosition);
            else
            {
                foreach (My_Figure fig in Figures)
                    fig.mouse_down(sender, e);
            }
        }

        private new void MouseDownCreateNew(object sender, MouseEventArgs e)
        {
            Point pt = new Point(e.X, e.Y);
            if (e.Button == MouseButtons.Left)
            {
                My_Figure fig = core.Picture.GetSelectedFigure(pt);
                if ((fig != null) && (fig != this))
                    if (Figures.Contains(fig) == false)
                    {
                        Figures.Add(fig);
                    }
            }
        }

        public override bool IsSelected(Point pt)
        {
            GraphicsPath p = path;
            bool res = p.IsVisible(pt) || p.IsOutlineVisible(pt, pen);
            foreach (My_Figure fig in Figures)
            {
                if (fig.IsSelected(pt) == true)
                    return true;
            }
            return res;
        }

        public override bool IsSelected(Rectangle rt)
        {
            foreach (My_Figure fig in Figures)
            {
                if (fig.IsSelected(rt) == false)
                    return false;
            }
            return true;
        }
    }
}